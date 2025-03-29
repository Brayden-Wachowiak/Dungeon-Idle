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

    // References to the toggle text positions
    private RectTransform toggle1XText;
    private RectTransform toggle10XText;
    private RectTransform toggle100XText;

    // Stores original and adjusted text positions
    private Vector3 originalPosition;
    private Vector3 positionAdjustment;

    // Colors for pressed and unpressed states
    private Color pressedColor = new Color(.3803922f, 0.7372549f, 0.9058824f, 1f);
    private Color originalColor = Color.white;

    // Stores the last selected toggle
    private Toggle lastSelectedToggle;

    void Start()
    {
        // Get references to toggle text objects
        toggle1XText = transform.Find("1X/ToggleText")?.GetComponent<RectTransform>();
        toggle10XText = transform.Find("10X/ToggleText")?.GetComponent<RectTransform>();
        toggle100XText = transform.Find("100X/ToggleText")?.GetComponent<RectTransform>();

        // Set text movement positions
        originalPosition = toggle1XText.localPosition;
        positionAdjustment = originalPosition + (Vector3.down * 4);

        // Initialize toggle states
        toggle1X.isOn = true;
        toggle10X.isOn = false;
        toggle100X.isOn = false;
        lastSelectedToggle = toggle1X;

        // Add event listeners to toggles
        toggle1X.onValueChanged.AddListener(delegate { ToggleChanged(toggle1X); });
        toggle10X.onValueChanged.AddListener(delegate { ToggleChanged(toggle10X); });
        toggle100X.onValueChanged.AddListener(delegate { ToggleChanged(toggle100X); });

        // Apply initial visuals
        ToggleChanged(lastSelectedToggle);
    }

    // Private Methods

    /// <summary>
    /// Handles toggle state changes when a toggle is clicked.
    /// </summary>
    private void ToggleChanged(Toggle selectedToggle)
    {
        // Ensure at least one toggle remains on
        if (!selectedToggle.isOn)
        {
            selectedToggle.isOn = true;
            return;
        }

        // Turn off the previously selected toggle
        if (lastSelectedToggle != null && lastSelectedToggle != selectedToggle)
        {
            lastSelectedToggle.isOn = false;
        }

        // Update Global Buy Amount and visuals based on the selected toggle
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

        // Store the currently selected toggle
        lastSelectedToggle = selectedToggle;
    }

    /// <summary>
    /// Updates the visuals of a toggle based on its selected state.
    /// </summary>
    private void UpdateToggleVisuals(Toggle toggle, Image background, RectTransform text, bool isSelected)
    {
        background.sprite = isSelected ? toggledSprite : untoggledSprite;
        background.color = isSelected ? pressedColor : originalColor;
        text.localPosition = isSelected ? positionAdjustment : originalPosition;
    }
}