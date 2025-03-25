using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;
using Unity.VisualScripting;

public class TrapHandler : MonoBehaviour
{
    public GameObject TrapContainer;
    public Sprite[] trapSprites;
    public string[] trapNames;

    private List<GameObject> trapList = new List<GameObject>();
    private List<BigInteger> basePrices = new List<BigInteger>();
    public Transform trapParent; // Parent container for the traps
    private int nextUnlockIndex = 3;

    void Start()
    {
        BigInteger StartingPrice = 100;
        BigInteger StartingProductionRate = 1;

        for (int i = 0; i < trapSprites.Length; i++)
        {
            BigInteger BasePrice;
            BigInteger CoinProductionRate;

            if (i == 0)
            {
                BasePrice = 11;
                CoinProductionRate = 1;
            }
            else
            {
                BasePrice = RoundToTwoDigits(StartingPrice * (BigInteger)Math.Pow(10.75, i - 1));
                CoinProductionRate = StartingProductionRate * (BigInteger)Math.Pow(3, i - 1);
            }

            basePrices.Add(BasePrice);

            CreateTrap(trapNames[i], BasePrice, CoinProductionRate, trapSprites[i], i);
        }

        for(int i = 3; i < trapList.Count; i++)
        {
            trapList[i].SetActive(false);
        }
    }

    void Update() 
    {
        if (nextUnlockIndex < basePrices.Count - 1)
        {
            Debug.Log(basePrices[nextUnlockIndex]);
            if (basePrices[nextUnlockIndex] / 10 < (GlobalVariables.Instance.TotalCoinsEarned))
            {
                trapList[nextUnlockIndex].SetActive(true);
                nextUnlockIndex++;
            }
        }
    }  

    BigInteger RoundToTwoDigits(BigInteger number)
    {

        int magnitude = (int)BigInteger.Log10(number) - 2; // Get (digits - 1)
        BigInteger roundFactor = BigInteger.Pow(10, magnitude); // 10^(magnitude)

        return (number / roundFactor) * roundFactor; // Round to nearest
    }

    public void CreateTrap(string name, BigInteger price, BigInteger productionRate, Sprite icon, int i)
    {
        UnityEngine.Vector3 pos = UnityEngine.Vector3.down * (108 * i);
        GameObject trapObj = Instantiate(TrapContainer, trapParent);
        trapObj.transform.position += pos;
        Trap trap = trapObj.GetComponent<Trap>();
        trapList.Add(trapObj);
        if (trap != null)
        {
            trap.Initialize(name, price, productionRate, icon);
        }
    }
}
