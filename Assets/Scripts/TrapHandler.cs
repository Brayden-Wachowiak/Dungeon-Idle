using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;

public class TrapHandler : MonoBehaviour
{
    public GameObject TrapContainer;
    public Sprite[] trapSprites;
    public string[] trapNames;

    private List<GameObject> trapList = new List<GameObject>();
    public Transform trapParent; // Parent container for the traps

    void Start()
    {

        BigInteger StartingPrice = 100;

        for (int i = 0; i < trapSprites.Length; i++)
        {
            BigInteger BasePrice;

            if (i == 0)
            {
                BasePrice = 11;
            }
            else
            {
                BasePrice = RoundToTwoDigits(StartingPrice * (BigInteger)Math.Pow(10.75, i - 1));
            }

                CreateTrap(trapNames[i], BasePrice, trapSprites[i], i);
        }
    }

    BigInteger RoundToTwoDigits(BigInteger number)
    {

        int magnitude = (int)BigInteger.Log10(number) - 2; // Get (digits - 1)
        BigInteger roundFactor = BigInteger.Pow(10, magnitude); // 10^(magnitude)

        return (number / roundFactor) * roundFactor; // Round to nearest
    }

    public void CreateTrap(string name, BigInteger price, Sprite icon, int i)
    {
        UnityEngine.Vector3 pos = UnityEngine.Vector3.down * (108 * i);
        GameObject trapObj = Instantiate(TrapContainer, trapParent);
        trapObj.transform.position += pos;
        Trap trap = trapObj.GetComponent<Trap>();
        trapList.Add(trapObj);
        if (trap != null)
        {
            trap.Initialize(name, price, icon);
        }
    }
}
