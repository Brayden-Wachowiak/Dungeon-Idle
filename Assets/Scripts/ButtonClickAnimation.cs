using UnityEngine;
using UnityEngine.UI;
public class ButtonClickAnimation : MonoBehaviour
{
    // Public Variables
    public Sprite clickedSprite; // Sprite to display when button is clicked

    // Private Variables
    private Image buttonImage;     // Reference to the button's image component
    private Sprite defaultSprite;  // Stores the default button sprite
    private RectTransform buttonText; // Reference to the button's text transform
    private Vector3 originalPosition; // Stores the original text position

    void Start()
    {
        // Get references to required components
        buttonImage = GetComponent<Image>();
        buttonText = transform.Find("ButtonText")?.GetComponent<RectTransform>();

        // Store the default sprite
        defaultSprite = buttonImage.sprite;

        // Add click listener to button
        GetComponent<Button>().onClick.AddListener(ChangeButtonSprite);

        // Store the original text position
        originalPosition = buttonText.localPosition;
    }

    // Private Methods

    /// <summary>
    /// Changes the button's sprite and moves the text when clicked.
    /// </summary>
    private void ChangeButtonSprite()
    {
        buttonText.transform.localPosition = originalPosition + (Vector3.down * 8); // Move text down slightly
        buttonImage.sprite = clickedSprite; // Change sprite

        // Reset sprite after 0.2 seconds
        Invoke(nameof(ResetSprite), 0.2f);
    }

    /// <summary>
    /// Resets the button's sprite and text position.
    /// </summary>
    private void ResetSprite()
    {
        buttonText.transform.localPosition = originalPosition; // Reset text position
        buttonImage.sprite = defaultSprite; // Reset sprite to default
    }
}