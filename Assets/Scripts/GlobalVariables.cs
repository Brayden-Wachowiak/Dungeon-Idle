using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    private int buyAmount;
    private int lastBuyAmount;  // Tracks the last buyAmount used to calculate the price

    // Reference to the script that has AdjustBuyPrice

    // Singleton reference
    public static GlobalVariables Instance { get; private set; }

    public int BuyAmount
    {
        get { return buyAmount; }
        set
        {
            if (buyAmount != value)  // Only fire when the value of buyAmount changes
            {
                buyAmount = value;

                // Check if the buyAmount has led to a change in the price
                if (buyAmount != lastBuyAmount)
                {
                    lastBuyAmount = buyAmount;  // Update the last buy amount
                    AdjustAllTrapsBuyPrice();  // Trigger AdjustBuyPrice only if the price changes
                }
            }
        }
    }

    private void Awake()
    {

        // Ensure that there's only one instance of GlobalVariables
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void AdjustAllTrapsBuyPrice()
    {
        // Find all Trap instances in the scene
        Trap[] allTraps = FindObjectsOfType<Trap>();

        // Loop through each Trap and call AdjustBuyPrice
        foreach (Trap trap in allTraps)
        {
            trap.AdjustBuyPrice(buyAmount);
        }
    }
}