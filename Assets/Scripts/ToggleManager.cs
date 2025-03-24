using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    // References to the Toggle buttons
    public Toggle toggle1X;
    public Toggle toggle10X;
    public Toggle toggle100X;

    // References to the Image components for the background of the toggles
    public Image background1X;
    public Image background10X;
    public Image background100X;

    // Sprites for the toggled and non-toggled states
    public Sprite toggledSprite;
    public Sprite untoggledSprite;


    private RectTransform toggle1XText;
    private RectTransform toggle10XText;
    private RectTransform toggle100XText;

    private Vector3 originalPosition;
    private Vector3 positionAdjustment;

    private Color pressedColor = new Color(.3803922f, 0.7372549f, 0.9058824f, 1f);
    private Color originalColor = Color.white;

    private Toggle lastSelectedToggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle1XText = transform.Find("1X/ToggleText")?.GetComponent<RectTransform>();
        toggle10XText = transform.Find("10X/ToggleText")?.GetComponent<RectTransform>();
        toggle100XText = transform.Find("100X/ToggleText")?.GetComponent<RectTransform>();

        originalPosition = toggle1XText.localPosition;
        positionAdjustment = originalPosition + (Vector3.down * 4);

        toggle1X.isOn = true;
        toggle10X.isOn = false;
        toggle100X.isOn = false;

        lastSelectedToggle = toggle1X;

        // Ensure only one toggle is active initially
        toggle1X.onValueChanged.AddListener(delegate { ToggleChanged(toggle1X); });
        toggle10X.onValueChanged.AddListener(delegate { ToggleChanged(toggle10X); });
        toggle100X.onValueChanged.AddListener(delegate { ToggleChanged(toggle100X); });

        // Initialize the toggles state
        ToggleChanged(lastSelectedToggle);
    }

    // This method is called when any of the toggles is clicked
    void ToggleChanged(Toggle selectedToggle)
    {
        if (!selectedToggle.isOn)
        {
            selectedToggle.isOn = true;
            return;
        }

        // Turn off the previous toggle
        if (lastSelectedToggle != null && lastSelectedToggle != selectedToggle)
        {
            lastSelectedToggle.isOn = false;
        }

        if (selectedToggle == toggle1X)
        {
            GlobalVariables.Instance.BuyAmount = 1;
            UpdateToggleVisuals(toggle1X, background1X, toggle1XText, true);
            UpdateToggleVisuals(toggle10X, background10X, toggle10XText, false);
            UpdateToggleVisuals(toggle100X, background100X, toggle100XText, false);
        }
        else if (selectedToggle == toggle10X)
        {
            GlobalVariables.Instance.BuyAmount = 10;
            UpdateToggleVisuals(toggle1X, background1X, toggle1XText, false);
            UpdateToggleVisuals(toggle10X, background10X, toggle10XText, true);
            UpdateToggleVisuals(toggle100X, background100X, toggle100XText, false);
        }
        else if (selectedToggle == toggle100X)
        {
            GlobalVariables.Instance.BuyAmount = 100;
            UpdateToggleVisuals(toggle1X, background1X, toggle1XText, false);
            UpdateToggleVisuals(toggle10X, background10X, toggle10XText, false);
            UpdateToggleVisuals(toggle100X, background100X, toggle100XText, true);
        }

        // Store the newly selected toggle
        lastSelectedToggle = selectedToggle;
    }

    void UpdateToggleVisuals(Toggle toggle, Image background, RectTransform text, bool isSelected)
    {
        background.sprite = isSelected ? toggledSprite : untoggledSprite;
        background.color = isSelected ? pressedColor : originalColor;
        text.localPosition = isSelected ? positionAdjustment : originalPosition;
    }
}