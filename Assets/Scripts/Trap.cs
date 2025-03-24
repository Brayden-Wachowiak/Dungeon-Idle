using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System;

public class Trap : MonoBehaviour
{
    public string trapName { get; private set; }
    public int quantity { get; private set; }
    public BigInteger currentPrice { get; private set; }
    public double basePrice { get; private set; }
    public int buyPrice { get; private set; }

    public Sprite icon { get; private set; }
    private TMP_Text trapNameText;
    private TMP_Text quantityOfTrapText;
    private TMP_Text priceText;
    private Image trapIcon;

    private static readonly string[] suffixes =
{
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc",
        "Ud", "Dd", "Td", "Qd", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg",
        "Uvg", "Dvg", "Tvg", "Qavg", "Qivg", "Sxvg", "Spvg", "Ocvg", "Novg", "Tg"
    };


    public void Initialize(string _name, BigInteger _basePrice, Sprite _icon)
    {
        this.trapName = _name;
        this.currentPrice = _basePrice;
        this.basePrice = (double)_basePrice;
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

        UpdateUI();
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
            double powerResult = Math.Pow(1.12, quantity + i);
            Debug.Log(powerResult);
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
}
