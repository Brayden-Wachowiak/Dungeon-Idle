using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIImageEventTrigger : MonoBehaviour, IPointerClickHandler
{
    public GameObject coinPrefab; // Assign a coin prefab in the Inspector
    public RectTransform canvasRect;
    private float launchForce = 50f; // How fast the coin flies
    private float lifetime = 0.75f; // How long before the coin disappears

    private TrapHandler trapHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalVariables.Instance.CoinBalance += 1 + (trapHandler.traps[0].coinProductionRate * (BigInteger)(trapHandler.traps[0].quantity / 5));
        GlobalVariables.Instance.TotalCoinsEarned += 1 + (trapHandler.traps[0].coinProductionRate * (BigInteger)trapHandler.traps[0].quantity);
        SpawnCoin();
    }

    void Start()
    {
        // Initialize trapHandler in Start() to avoid UnityException
        trapHandler = FindObjectOfType<TrapHandler>();
    }

    void SpawnCoin()
    {
        UnityEngine.Vector2 spawnPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out spawnPosition);

        GameObject coin = Instantiate(coinPrefab, spawnPosition, UnityEngine.Quaternion.identity, transform);
        coin.GetComponent<RectTransform>().anchoredPosition = spawnPosition;
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            UnityEngine.Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction
            rb.AddForce(randomDirection * launchForce, ForceMode2D.Impulse);
        }

        Destroy(coin, lifetime); // Destroy the coin after 'lifetime' seconds
    }

}
