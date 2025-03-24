using UnityEngine;
using UnityEngine.UI;
public class ButtonClickAnimation : MonoBehaviour
{
    public Sprite clickedSprite;
    private Image buttonImage;
    private Sprite defaultSprite;
    private RectTransform buttonText;
    private Vector3 originaPosition;
    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonText = transform.Find("ButtonText")?.GetComponent<RectTransform>();
        defaultSprite = buttonImage.sprite;
        GetComponent<Button>().onClick.AddListener(ChangeButtonSprite);
        originaPosition = buttonText.localPosition;
    }
    void ChangeButtonSprite()
    {
        buttonText.transform.localPosition = originaPosition + (Vector3.down * 4);
        buttonImage.sprite = clickedSprite;
        Invoke(nameof(ResetSprite), 0.2f); // Reset after 0.2s
    }
    void ResetSprite()
    {
        buttonText.transform.localPosition = originaPosition;
        buttonImage.sprite = defaultSprite;
    }
}
