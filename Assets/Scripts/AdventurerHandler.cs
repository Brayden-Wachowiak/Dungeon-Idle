using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIImageEventTrigger : MonoBehaviour, IPointerClickHandler
{
    private TrapHandler trapHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalVariables.Instance.CoinBalance += 1 + (trapHandler.GetTrapProductionRate(0) * (BigInteger)trapHandler.GetTrapQuanity(0));
        GlobalVariables.Instance.TotalCoinsEarned += 1 + (trapHandler.GetTrapProductionRate(0) * (BigInteger)trapHandler.GetTrapQuanity(0));
    }

    void Start()
    {
        // Initialize trapHandler in Start() to avoid UnityException
        trapHandler = FindObjectOfType<TrapHandler>();
    }
}
