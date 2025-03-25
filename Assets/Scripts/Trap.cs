using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System;
using System.Collections;
using Unity.VisualScripting;

public class Trap : MonoBehaviour
{
    public string trapName { get; private set; }
    public int quantity { get; private set; }
    public BigInteger currentPrice { get; private set; }
    public BigInteger coinProductionRate { get; private set; }
    public double basePrice { get; private set; }
    public int buyPrice { get; private set; }

    public Sprite icon { get; private set; }
    private TMP_Text trapNameText;
    private TMP_Text quantityOfTrapText;
    private TMP_Text priceText;
    private Image trapIcon;
    private Button buyButton;

    private static readonly string[] suffixes =
    {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc",
        "Ud", "Dd", "Td", "Qd", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg",
        "Uvg", "Dvg", "Tvg", "Qavg", "Qivg", "Sxvg", "Spvg", "Ocvg", "Novg", "Tg"
    };


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

    void Start()
    {
        trapNameText = transform.Find("TrapNameText")?.GetComponent<TMP_Text>();
        quantityOfTrapText = transform.Find("TrapIcon/QuantityOfTrapText")?.GetComponent<TMP_Text>();
        priceText = transform.Find("TrapNameText/CoinIcon/PriceText")?.GetComponent<TMP_Text>();
        trapIcon = transform.Find("TrapIcon")?.GetComponent<Image>();
        buyButton = transform.Find("BuyButton")?.GetComponent<Button>();

        buyButton.onClick.AddListener(OnButtonClick);

        UpdateUI();

        if (trapName != "Clicker") 
        {
            StartCoroutine(GenerateCoins());
        }
    }

    void Update()
    {
        if (GlobalVariables.Instance.CoinBalance < currentPrice) 
        { 
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public static string FormatLargeNumber(BigInteger number)
    {
        if (number < 1000) return number.ToString(); // Return full number if < 1K

        int magnitude = (int)(BigInteger.Log10(number) / 3); // Get index of suffix
        if (magnitude >= suffixes.Length) return "?"; // Prevent out-of-bounds errors

        BigInteger divisor = BigInteger.Pow(10, magnitude * 3);
        decimal roundedValue = (decimal)number / (decimal)divisor;

        return roundedValue.ToString("0.0") + suffixes[magnitude];
    }

    public void AdjustBuyPrice(int buyAmount) 
    {
        BigInteger newPrice = 0;
        int Counter = 1;
        for (int i = 0; i < buyAmount; i++) 
        {
            double powerResult = Math.Pow(1.14, quantity + i);
            newPrice += (BigInteger)(basePrice * powerResult);
            Counter++;
        }

        currentPrice = newPrice;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (trapNameText) trapNameText.text = trapName;
        if (quantityOfTrapText) quantityOfTrapText.text = quantity.ToString();
        if (priceText) priceText.text = FormatLargeNumber(currentPrice);
        if (trapIcon) trapIcon.sprite = icon;
    }

    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            // Add earnings to the player's coin total
            GlobalVariables.Instance.CoinBalance += (coinProductionRate * quantity);
            GlobalVariables.Instance.TotalCoinsEarned += (coinProductionRate * quantity);

            yield return new WaitForSeconds(1f); // Wait for 1 second before adding again
        }
    }

    private void OnButtonClick() 
    {
        if (GlobalVariables.Instance.CoinBalance >= currentPrice) 
        {
            quantity += GlobalVariables.Instance.BuyAmount;
            GlobalVariables.Instance.CoinBalance -= currentPrice;
            AdjustBuyPrice(GlobalVariables.Instance.BuyAmount);
        }
    } 
}
