using UnityEngine;
using UnityEngine.UI; // Include this to access UI components
using System.Collections; // Add this line for IEnumerator

public class SlimeSpawner : MonoBehaviour
{
    public GameObject[] slimePrefabs; // Array of slime prefabs to choose from
    public int rows = 2; // Number of rows
    public int slimesPerRow = 5; // Number of slimes per row
    public float spacing = 1.5f; // Space between slimes

    public int currentWave = 0; // Current wave number
    public int initialSlimesInWave = 5; // Initial number of slimes to spawn per wave
    public int slimesInWave; // Number of slimes to spawn per wave
    public int waveIncrease = 2; // Number of slimes to add per wave

    public Text scoreText; // UI Text element to display score
    public Text waveWarningText; // UI Text element to warn about the new wave
    private int score = 0; // Player's score
    private bool isSpawningWave = false; // Flag to check if a wave is currently spawning

    private string[] formations = { "Straight", "Staggered", "V-Shape" }; // Different formations to choose from
    private string lastFormation; // Track the last formation used

    void Start()
    {
        waveWarningText.gameObject.SetActive(false); // Hide the warning text initially
        slimesInWave = initialSlimesInWave; // Set initial slimes in wave
        StartWave();
    }

    void Update()
    {
        // Check if all slimes are defeated and if a new wave can be spawned
        if (transform.childCount == 0 && !isSpawningWave) // No slimes left and not currently spawning
        {
            RespawnSlimes();
        }
    }

    void StartWave()
    {
        currentWave++;
        slimesInWave += waveIncrease; // Increase slimes per wave
        SpawnSlimes();
    }

    public void RespawnSlimes()
    {
        // Clear existing slimes
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Start the countdown for a new wave after a delay
        StartCoroutine(NewWaveCountdown());
    }

    private IEnumerator NewWaveCountdown()
    {
        isSpawningWave = true; // Set the flag to true to prevent spawning new waves
        waveWarningText.gameObject.SetActive(true); // Show the warning text
        waveWarningText.text = $"Wave {currentWave + 1} Incoming!"; // Set the warning message

        // Wait for a few seconds before spawning the next wave
        yield return new WaitForSeconds(2f); // Adjust this duration as needed

        waveWarningText.gameObject.SetActive(false); // Hide the warning text
        StartWave(); // Start the new wave

        isSpawningWave = false; // Reset the flag after starting the wave
    }

    void SpawnSlimes()
{
    // Choose a random formation that is not the same as the last formation
    string formation;
    do
    {
        formation = formations[Random.Range(0, formations.Length)];
    } while (formation == lastFormation); // Ensure it's different from the last formation

    lastFormation = formation; // Update the last formation

    // Adjust slimesInWave across all rows
    int slimesSpawned = 0; // Track how many slimes have been spawned

    float totalWidth = (slimesPerRow - 1) * spacing;
    float startX = totalWidth / 2;
    float startY = (rows - 1) * spacing / 2;

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < slimesPerRow; col++)
        {
            // Ensure not to exceed the total slimes in the wave
            if (slimesSpawned >= slimesInWave) break;

            Vector3 spawnPosition;

            // Determine spawn position based on the formation
            switch (formation)
            {
                case "Straight":
                    spawnPosition = new Vector3(col * spacing - startX, row * spacing - startY, 0) + transform.position;
                    break;
                case "Staggered":
                    spawnPosition = new Vector3(col * spacing - startX + (row % 2 == 0 ? spacing / 2 : 0), row * spacing - startY, 0) + transform.position;
                    break;
                case "V-Shape":
                    spawnPosition = new Vector3(col * spacing / 2 - startX, row * spacing - startY, 0) + transform.position; // Adjust V shape here
                    break;
                default:
                    spawnPosition = Vector3.zero; // Fallback
                    break;
            }

            // Randomly select a slime prefab from the array
            int randomIndex = Random.Range(0, slimePrefabs.Length);
            GameObject selectedSlimePrefab = slimePrefabs[randomIndex];

            // Instantiate the selected slime prefab
            Instantiate(selectedSlimePrefab, spawnPosition, Quaternion.identity, transform);

            slimesSpawned++; // Increment the spawned slimes count
        }

        // Stop if all slimes have been spawned
        if (slimesSpawned >= slimesInWave) break;
    }

    UpdateScoreText(); // Update the score display
}


    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0; // Reset score to 0
        UpdateScoreText(); // Update the displayed score
    }

    public void ResetWave()
    {
        currentWave = 0; // Reset the wave number
        slimesInWave = initialSlimesInWave; // Reset slimes in wave to initial value
        lastFormation = null; // Reset last formation
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
