using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public Trap trap;
    public TMP_Text rateText;
    public TMP_Text metricText;

    private RectTransform toolTipRect;
    private RectTransform canvasRect;
    private bool isHovered = false;

    void Start()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        toolTipRect = tooltip.GetComponent<RectTransform>();
        tooltip.SetActive(false);
    }

    void Update() 
    {
        if (isHovered)
        {
            tooltip.SetActive(true);

            if (trap.trapName == "Clicker")
            {
                rateText.text = ((BigInteger)(trap.quantity / 5 * trap.coinProductionRate + 1)).ToString();
                metricText.text = "/click";
            }
            else 
            {
                rateText.text = ((BigInteger)trap.quantity * trap.coinProductionRate).ToString();
                metricText.text = "/sec";
            }

            UnityEngine.Vector3 localMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, Input.mousePosition, null, out localMousePos);
            toolTipRect.transform.position = localMousePos + new UnityEngine.Vector3(168, -16 ,0); // Offset tooltip slightly
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        tooltip.SetActive(false);
    }
}
