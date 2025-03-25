using UnityEngine;
using TMPro;
using System.Numerics;

public class TopBarHandler : MonoBehaviour
{
    public TMP_Text coinBalanceText;

    private static readonly string[] suffixes =
    {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc",
        "Ud", "Dd", "Td", "Qd", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg",
        "Uvg", "Dvg", "Tvg", "Qavg", "Qivg", "Sxvg", "Spvg", "Ocvg", "Novg", "Tg"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinBalanceText = transform.Find("CoinIcon/TotalCoins")?.GetComponent<TMP_Text>();

        coinBalanceText.text = FormatLargeNumber(GlobalVariables.Instance.CoinBalance);
    }

    // Update is called once per frame
    void Update()
    {
        coinBalanceText.text = FormatLargeNumber(GlobalVariables.Instance.CoinBalance);
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
}
