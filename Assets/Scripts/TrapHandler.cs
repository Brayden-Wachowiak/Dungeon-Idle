using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;

public class TrapHandler : MonoBehaviour
{
    // Public Variables
    public List<Trap> traps = new List<Trap>();
    public RectTransform TrapsContainer; // Containter holding all of the traps

    // Private Variables
    private int nextUnlockIndex = 3;  // Index for unlocking new traps

    void Start()
    {
        InitializeTraps();

        // Lock traps beyond the initial 3
        for (int i = 3; i < traps.Count; i++)
        {
            traps[i].gameObject.SetActive(false);
        }

        UpdateHeight();
    }

    void Update()
    {
        // Unlock new traps based on the player's total earnings
        if (nextUnlockIndex < traps.Count - 1)
        {
            // Unlock the next trap when the player's total earnings exceed 10% of the trap price
            if ((BigInteger)traps[nextUnlockIndex].basePrice / 10 < GlobalVariables.Instance.TotalCoinsEarned)
            {
                traps[nextUnlockIndex].gameObject.SetActive(true);

                GlobalVariables.Instance.AdjustAllTrapsBuyPrice();

                UpdateHeight();

                nextUnlockIndex++;
            }
        }
    }

    // Private Methods

    private void InitializeTraps()
    {
        // Define starting values for the first trap
        double StartingPrice = 100;
        BigInteger StartingProductionRate = 1;

        // Loop through each trap and initialize them
        for (int i = 0; i < traps.Count; i++)
        {
            double basePrice;
            BigInteger coinProductionRate;

            // Special case for the first trap (Clicker)
            if (i == 0)
            {
                basePrice = 11;
                coinProductionRate = 1;
            }
            else
            {
                // Calculate price and production rate using logarithmic scaling
                basePrice = RoundToTwoDigits(StartingPrice * Math.Pow(10.75, i - 1));
                coinProductionRate = StartingProductionRate * (BigInteger)Math.Pow(6, i - 1);
            }

            traps[i].basePrice = basePrice;
            traps[i].coinProductionRate = coinProductionRate;
            traps[i].AdjustBuyPrice(GlobalVariables.Instance.BuyAmount);
        }
    }

    /// <summary>
    /// Rounds a number to the two most significant digits.
    /// </summary>
    private double RoundToTwoDigits(double number)
    {
        double magnitude = Math.Log10(number) - 2; // Get (digits - 1)
        double roundFactor = Math.Pow(10, magnitude); // 10^(magnitude)

        return (number / roundFactor) * roundFactor; // Round to nearest
    }

    private void UpdateHeight() 
    {
        float newHeight = (nextUnlockIndex + 1) * 116 + 12;

        TrapsContainer.sizeDelta = new UnityEngine.Vector2(TrapsContainer.sizeDelta.x, newHeight);
    }
}
