using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannonHealth : MonoBehaviour
{
    public GameObject healthPrefab; // Prefab for the health bar UI element (should be a UI Image prefab)
    public Transform healthBarContainer; // Container for the health bar UI elements
    public Sprite healthSprite; // PNG image as a sprite for health bar
    public int totalHealth = 5; // Total number of health points
    public float spacing = 50f; // Adjustable spacing between health bar segments
    private Image[] healthImages; // Array to hold health images

    public GameObject[] uiObjectsToActivate; // UI objects to activate when health reaches 0
    public GameObject[] uiObjectsToDeactivate; // UI objects to deactivate when health reaches 0

    private bool isInvulnerable = false; // Flag to check if the player is invulnerable
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public float blinkInterval = 0.2f; // Time between blinks during invulnerability
    public float blinkDuration = 3f; // Total duration of the blinking

    private Coroutine invulnerableCoroutine; // Reference to the running coroutine

    // Audio Clips
    public AudioClip loseHealthSFX; // Sound effect for losing health
    public AudioClip gameOverSFX; // Sound effect for when health reaches 0
    private AudioSource audioSource; // Audio source to play sound effects

    private void Start()
    {
        // Initialize the health bar UI
        InitializeHealth();

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the object!");
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    private void InitializeHealth()
    {
        // Clear previous health images if they exist
        foreach (Transform child in healthBarContainer)
        {
            Destroy(child.gameObject);
        }

        // Initialize the health bar UI
        healthImages = new Image[totalHealth];
        for (int i = 0; i < totalHealth; i++)
        {
            // Create a clone of the health prefab and set its parent to the health bar container
            GameObject healthImage = Instantiate(healthPrefab, healthBarContainer);

            // Get the Image component of the instantiated prefab
            Image healthImageComponent = healthImage.GetComponent<Image>();

            // Set the sprite of the Image to the provided PNG sprite
            healthImageComponent.sprite = healthSprite;

            // Set position with interactive spacing
            RectTransform healthRectTransform = healthImage.GetComponent<RectTransform>();
            healthRectTransform.anchoredPosition = new Vector2(i * (spacing + 100), 0); // Adjusted for spacing and size
            healthRectTransform.sizeDelta = new Vector2(100, 100); // Set size to 100x100 pixels

            // Store reference to the Image for future use (e.g., destroying health)
            healthImages[i] = healthImageComponent;
        }

        // Deactivate UI objects when health is reset
        DeactivateUIObjects();
    }

    public void ResetHealth()
    {
        totalHealth = 5; // Reset total health

        // Reset the health bar UI
        for (int i = 0; i < healthImages.Length; i++)
        {
            // Ensure the image is active
            healthImages[i].gameObject.SetActive(true);
            // Reset color to make it fully visible
            Color healthColor = healthImages[i].color;
            healthColor.a = 1.0f; // Fully visible
            healthImages[i].color = healthColor;
        }

        // Deactivate the UI objects activated at health = 0
        DeactivateUIObjects();

        // Reset invulnerability and stop blinking coroutine
        ResetBlinking();

        // Ensure sprite is fully visible and invulnerability is off
        if (spriteRenderer != null)
        {
            Color finalColor = spriteRenderer.color;
            finalColor.a = 1.0f; // Set sprite alpha to fully visible
            spriteRenderer.color = finalColor;
        }
    }

    private void ResetBlinking()
    {
        // Stop the invulnerability coroutine if it's running
        if (invulnerableCoroutine != null)
        {
            StopCoroutine(invulnerableCoroutine);
        }

        // Reset invulnerability flag
        isInvulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with is an enemy bullet and player is not invulnerable
        if (collision.CompareTag("EnemyBullet") && !isInvulnerable)
        {
            invulnerableCoroutine = StartCoroutine(InvulnerableCoroutine());
            DestroyHealth();
            Destroy(collision.gameObject); // Optional: Destroy the enemy bullet as well
        }
    }

    private IEnumerator InvulnerableCoroutine()
    {
        isInvulnerable = true; // Set invulnerability flag to true

        // Start blinking effect
        float endTime = Time.time + blinkDuration;
        while (Time.time < endTime)
        {
            // Toggle the alpha (opacity) of the sprite between 0 and 1
            Color color = spriteRenderer.color;
            color.a = (color.a == 1.0f) ? 0.0f : 1.0f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(blinkInterval);
        }

        // Ensure sprite is fully visible after blinking
        Color finalColor = spriteRenderer.color;
        finalColor.a = 1.0f;
        spriteRenderer.color = finalColor;

        isInvulnerable = false; // Reset invulnerability flag
    }

    private void DestroyHealth()
    {
        // Check if there is health left to destroy
        if (totalHealth > 0)
        {
            totalHealth--;
            // Instead of destroying the health image, just disable it
            healthImages[totalHealth].gameObject.SetActive(false);

            // Play losing health sound effect
            PlaySFX(loseHealthSFX);
        }

        // If health reaches 0, stop time and activate UI objects
        if (totalHealth <= 0)
        {
            Time.timeScale = 0; // Stop the game

            // Activate the UI objects
            foreach (GameObject uiObject in uiObjectsToActivate)
            {
                uiObject.SetActive(true);
            }

            // Play game over sound effect
            PlaySFX(gameOverSFX);
        }
        else
        {
            // Deactivate UI objects when health is not 0
            foreach (GameObject uiObject in uiObjectsToDeactivate)
            {
                uiObject.SetActive(false);
            }
        }
    }

    private void DeactivateUIObjects()
    {
        // Deactivate the specified UI objects
        foreach (GameObject uiObject in uiObjectsToActivate)
        {
            uiObject.SetActive(false);
        }

        // Deactivate UI objects that should be deactivated when health reaches 0
        foreach (GameObject uiObject in uiObjectsToDeactivate)
        {
            uiObject.SetActive(false);
        }
    }

    // Method to play sound effects
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect
        }
    }
}
