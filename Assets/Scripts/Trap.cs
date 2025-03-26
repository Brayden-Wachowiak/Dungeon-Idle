using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System;
using System.Collections;

public class Trap : MonoBehaviour
{
    // Public Variables
    public int quantity;                      // Number of this trap owned
    public BigInteger coinProductionRate;    // Coins generated per second per trap

    // Private Variables

    private string trapName;                  // Name of the trap
    private double basePrice;                 // Base price before scaling

    private BigInteger currentPrice;          // Current purchase price of the trap

    private Sprite icon;                      // Trap icon sprite
    private Image trapIcon;                   // UI Image component for trap icon
    private Image trapIconShaow;              // UI Image component for trap icon shadow
    private Button buyButton;                 // UI Button to purchase the trap
    private TMP_Text trapNameText;            // UI Text component for trap name
    private TMP_Text quantityOfTrapText;      // UI Text component for quantity display
    private TMP_Text priceText;               // UI Text component for price display

    // Suffixes for formatting large numbers (e.g., K = Thousand, M = Million, etc.)
    private static readonly string[] suffixes =
    {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc",
        "Ud", "Dd", "Td", "Qd", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg",
        "Uvg", "Dvg", "Tvg", "Qavg", "Qivg", "Sxvg", "Spvg", "Ocvg", "Novg", "Tg"
    };

    void Start()
    {
        // Locate UI components inside the Trap GameObject
        trapNameText = transform.Find("TrapNameText")?.GetComponent<TMP_Text>();
        quantityOfTrapText = transform.Find("TrapIcon/QuantityOfTrapText")?.GetComponent<TMP_Text>();
        priceText = transform.Find("TrapNameText/CoinIcon/PriceText")?.GetComponent<TMP_Text>();
        trapIcon = transform.Find("TrapIcon")?.GetComponent<Image>();
        trapIconShaow = transform.Find("TrapIconShadow")?.GetComponent<Image>();
        buyButton = transform.Find("BuyButton")?.GetComponent<Button>();

        // Add event listener for the buy button
        buyButton.onClick.AddListener(OnButtonClick);

        // Initialize UI elements
        SetUpUI();

        // Start coin generation coroutine if this trap isn't a manual clicker
        if (trapName != "Clicker")
        {
            StartCoroutine(GenerateCoins());
        }
    }

    void Update()
    {
        // Disable the buy button if the player can't afford the trap
        buyButton.interactable = GlobalVariables.Instance.CoinBalance >= currentPrice;
    }

    // Public Methods

    /// <summary>
    /// Initializes the trap with given properties.
    /// </summary>
    public void Initialize(string _name, BigInteger _basePrice, BigInteger _coinProductionRate, Sprite _icon)
    {
        this.trapName = _name;
        this.currentPrice = _basePrice;
        this.basePrice = (double)_basePrice;
        this.coinProductionRate = _coinProductionRate;
        this.icon = _icon;
        this.quantity = 0;

        UpdateUI();
    }

    /// <summary>
    /// Adjusts the price of the trap based on the number of purchases.
    /// Implements exponential price scaling.
    /// </summary>
    public void AdjustBuyPrice(int buyAmount)
    {
        BigInteger newPrice = 0;

        // Calculate new price based on the number of traps owned
        for (int i = 0; i < buyAmount; i++)
        {
            double powerResult = Math.Pow(1.14, quantity + i);
            newPrice += (BigInteger)(basePrice * powerResult);
        }

        currentPrice = newPrice;
        UpdateUI();
    }

    // Private Methods

    /// <summary>
    /// Sets up initial UI elements (trap name, icon, and price).
    /// </summary>
    private void SetUpUI()
    {
        if (trapNameText) trapNameText.text = trapName;
        if (trapIcon) trapIcon.sprite = icon;
        if (trapIconShaow) trapIconShaow.sprite = icon;

        UpdateUI();
    }

    /// <summary>
    /// Updates the UI text fields with the current trap quantity and price.
    /// </summary>
    private void UpdateUI()
    {
        if (quantityOfTrapText) quantityOfTrapText.text = quantity.ToString();
        if (priceText) priceText.text = FormatLargeNumber(currentPrice);
    }

    /// <summary>
    /// Handles the trap purchase when the buy button is clicked.
    /// Deducts coins and updates the price accordingly.
    /// </summary>
    private void OnButtonClick()
    {
        if (GlobalVariables.Instance.CoinBalance >= currentPrice)
        {
            // Increase trap quantity based on the current buy amount
            quantity += GlobalVariables.Instance.BuyAmount;

            // Deduct the cost from the player's balance
            GlobalVariables.Instance.CoinBalance -= currentPrice;

            // Adjust the price for the next purchase
            AdjustBuyPrice(GlobalVariables.Instance.BuyAmount);
        }
    }

    /// <summary>
    /// Formats a large number into a readable format with suffixes (e.g., 1.5M, 2.3B).
    /// </summary>
    private static string FormatLargeNumber(BigInteger number)
    {
        if (number < 1000) return number.ToString();

        int magnitude = (int)(BigInteger.Log10(number) / 3);
        if (magnitude >= suffixes.Length) return "?"; // Prevent out-of-bounds errors

        BigInteger divisor = BigInteger.Pow(10, magnitude * 3);
        decimal roundedValue = (decimal)number / (decimal)divisor;

        return roundedValue.ToString("0.0") + suffixes[magnitude];
    }

    /// <summary>
    /// Generates coins automatically every second, based on trap quantity.
    /// </summary>
    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            // Add earnings to the player's coin balance
            BigInteger totalEarnings = (coinProductionRate * quantity);
            GlobalVariables.Instance.CoinBalance += totalEarnings;
            GlobalVariables.Instance.TotalCoinsEarned += totalEarnings;

            yield return new WaitForSeconds(1f);
        }
    }
}