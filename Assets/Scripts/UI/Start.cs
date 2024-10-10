using UnityEngine;
using TMPro; // Ensure you have the TextMeshPro namespace
using UnityEngine.UI; // Include this to access UI components
using System.Collections; // Add this line for IEnumerator

public class CountdownController : MonoBehaviour
{
    public GameObject uiPanel; // The panel to click to start the countdown
    public TextMeshProUGUI countdownText; // TextMeshProUGUI component for the countdown
    public float countdownTime = 3f; // Countdown duration in seconds
    public GameObject[] objectsToDeactivate; // Objects to deactivate during countdown
    public GameObject[] objectsToActivate; // Objects to activate after countdown

    public Button restartButton; // Button to restart the game
    public CannonHealth cannonHealth; // Reference to the CannonHealth script
    public SlimeSpawner slimeSpawner; // Reference to the SlimeSpawner script
    public Cannon cannon; // Reference to the Cannon script
    public Transform respawnPoint; // The empty GameObject for player respawn position

    private void Start()
    {
        // Set time scale to 0 at the start of the game
        Time.timeScale = 0f;

        // Hide the countdown text initially
        countdownText.gameObject.SetActive(false);

        // Ensure the uiPanel has a Button component
        Button startButton = uiPanel.GetComponent<Button>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartCountdown); // Add listener to the button
        }
        else
        {
            Debug.LogError("The uiPanel does not have a Button component!");
        }

        // Ensure the restart button is set up
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame); // Add listener to restart button
        }
        else
        {
            Debug.LogError("The restart button is not assigned!");
        }
    }

    private void StartCountdown()
    {
        // Show the countdown text
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Get Ready!";

        // Deactivate specified objects
        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }

        // Start the countdown coroutine
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float timeRemaining = countdownTime;

        while (timeRemaining > 0)
        {
            countdownText.text = $"Get Ready! {timeRemaining:F0}"; // Update countdown text
            yield return new WaitForSecondsRealtime(1f); // Wait for 1 second in real time
            timeRemaining--;
        }

        // Hide the countdown text
        countdownText.gameObject.SetActive(false);

        // Activate specified objects
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        // Set time scale back to 1 to start the game
        Time.timeScale = 1f;
    }

    private void RestartGame()
{
    // Reset health
    cannonHealth.ResetHealth();

    // Reset score
    slimeSpawner.ResetScore();

    // Reset wave
    slimeSpawner.ResetWave(); // Reset wave number and slimes in wave

    // Respawn enemies
    slimeSpawner.RespawnSlimes();

    // Destroy player bullets
    cannon.DestroyBullets();

    // Destroy enemy bullets
    DestroyAllEnemyBullets();

    // Destroy the double shooting effect if active
    cannon.DisableDoubleShooting();

    // Respawn the player at the defined position
    cannon.transform.position = respawnPoint.position;

    // Restart the countdown
    StartCountdown();
}


    private void DestroyAllEnemyBullets()
    {
        // Find all enemy bullets and destroy them
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in enemyBullets)
        {
            Destroy(bullet);
        }

        // Find all double bullets and destroy them
        GameObject[] doubleBullets = GameObject.FindGameObjectsWithTag("DoubleBullet");
        foreach (GameObject bullet in doubleBullets)
        {
            Destroy(bullet);
        }
    }
}
