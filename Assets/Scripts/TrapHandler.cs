using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;

public class TrapHandler : MonoBehaviour
{
    // Public Variables
    public GameObject TrapContainer;  // Prefab for traps
    public RectTransform TrapsContainer; // Containter holding all of the traps
    public Transform trapParent;      // Parent container for the traps

    public string[] trapNames;        // Array of trap names
    public Sprite[] trapSprites;      // Array of trap icons

    // Private Variables
    private int nextUnlockIndex = 3;  // Index for unlocking new traps
    private int spacer = 12;

    private List<BigInteger> basePrices = new List<BigInteger>(); // List of base prices for traps
    private List<GameObject> trapList = new List<GameObject>();  // List of created traps
    private List<Trap> traps = new List<Trap>();

    void Start()
    {
        // Define starting values for the first trap
        BigInteger StartingPrice = 100;
        BigInteger StartingProductionRate = 1;

        float newHeight = (nextUnlockIndex + 1) * 116 + spacer;

        TrapsContainer.sizeDelta = new UnityEngine.Vector2(TrapsContainer.sizeDelta.x, newHeight);

        // Loop through each trap and initialize them
        for (int i = 0; i < trapSprites.Length; i++)
        {
            BigInteger BasePrice;
            BigInteger CoinProductionRate;

            // Special case for the first trap (Clicker)
            if (i == 0)
            {
                BasePrice = 11;
                CoinProductionRate = 1;
            }
            else
            {
                // Calculate price and production rate using logarithmic scaling
                BasePrice = RoundToTwoDigits(StartingPrice * (BigInteger)Math.Pow(10.75, i - 1));
                CoinProductionRate = StartingProductionRate * (BigInteger)Math.Pow(3, i - 1);
            }

            // Store base prices for later reference
            basePrices.Add(BasePrice);

            // Create and initialize the trap
            CreateTrap(trapNames[i], BasePrice, CoinProductionRate, trapSprites[i], i);
        }

        // Lock traps beyond the initial 3
        for (int i = 3; i < trapList.Count; i++)
        {
            trapList[i].SetActive(false);
        }
    }

    void Update()
    {
        // Unlock new traps based on the player's total earnings
        if (nextUnlockIndex < basePrices.Count - 1)
        {
            // Unlock the next trap when the player's total earnings exceed 10% of the trap price
            if (basePrices[nextUnlockIndex] / 10 < GlobalVariables.Instance.TotalCoinsEarned)
            {
                trapList[nextUnlockIndex].SetActive(true);

                float newHeight = (nextUnlockIndex + 1) * 116 + spacer;

                TrapsContainer.sizeDelta = new UnityEngine.Vector2(TrapsContainer.sizeDelta.x, newHeight);

                GlobalVariables.Instance.AdjustAllTrapsBuyPrice();

                nextUnlockIndex++;
            }
        }
    }

    // Public Methods

    /// <summary>
    /// Creates a new trap and positions it in the UI.
    /// </summary>
    public void CreateTrap(string name, BigInteger price, BigInteger productionRate, Sprite icon, int i)
    {
        // Set position based on index (spacing traps vertically)
        UnityEngine.Vector3 pos = UnityEngine.Vector3.down * (116 * i);

        // Instantiate trap UI element
        GameObject trapObj = Instantiate(TrapContainer, trapParent);
        trapObj.transform.position += pos;

        // Get Trap component and initialize it
        Trap trap = trapObj.GetComponent<Trap>();
        trapList.Add(trapObj);

        if (trap != null)
        {
            trap.Initialize(name, price, productionRate, icon);
            traps.Add(trap);
        }
    }

    public int GetTrapQuanity(int index) 
    {
        if (index < traps.Count) 
        {
            return traps[index].quantity;
        }

        return 0;
    }

    public BigInteger GetTrapProductionRate(int index)
    {
        if (index < traps.Count)
        {
            return traps[index].coinProductionRate;
        }

        return 0;
    }

    // Private Methods

    /// <summary>
    /// Rounds a number to the two most significant digits.
    /// </summary>
    private BigInteger RoundToTwoDigits(BigInteger number)
    {
        int magnitude = (int)BigInteger.Log10(number) - 2; // Get (digits - 1)
        BigInteger roundFactor = BigInteger.Pow(10, magnitude); // 10^(magnitude)

        return (number / roundFactor) * roundFactor; // Round to nearest
    }
}
