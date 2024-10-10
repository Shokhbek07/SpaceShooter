using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab; // The cannonball prefab to be fired
    public Transform firePoint; // Point where cannonballs are spawned
    public Transform firePointLeft; // Left firing point for double shooting
    public Transform firePointRight; // Right firing point for double shooting
    public float shootingForce = 500f; // Force to shoot cannonball
    public float moveSpeed = 2f; // Speed of the cannon movement
    public float fireRate = 10f; // Time interval between cannonball shots (10 shots per second)
    private float nextFireTime = 0f; // Time control for continuous shooting
    private bool isDoubleShooting = false; // Whether double shooting mode is active
    private float doubleShootingEndTime = 0f; // End time for double shooting mode

    // Audio Clips
    public AudioClip shootingSFX; // Shooting sound effect
    public AudioClip powerUpStartSFX; // Power-up start sound effect
    public AudioClip powerUpEndSFX; // Power-up end sound effect
    private AudioSource audioSource; // Audio source component

    // Boundary variables
    public float minX = -8f, maxX = 8f;  // Horizontal boundary limits
    public float minY = -4.5f, maxY = 4.5f; // Vertical boundary limits

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleMovement();       // Handle cannon movement using ASWD keys
        HandleContinuousShooting();  // Continuously shoot cannonballs

        // Check if double shooting mode should end
        if (isDoubleShooting && Time.time >= doubleShootingEndTime)
        {
            isDoubleShooting = false; // End double shooting
            PlaySFX(powerUpEndSFX); // Play power-up end SFX
        }
    }

    private void HandleMovement()
    {
        // Get movement input from ASWD keys
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right keys
        float moveY = Input.GetAxis("Vertical");   // W/S or Up/Down keys

        // Apply movement to the cannon
        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Clamp the position to the boundaries
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        // Set the position within the boundary
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void HandleContinuousShooting()
    {
        // Check if it's time to shoot
        if (Time.time >= nextFireTime)
        {
            if (isDoubleShooting)
            {
                ShootDoubleCannonBalls(Vector2.up); // Shoot two cannonballs if power-up is active
            }
            else
            {
                ShootCannonBall(Vector2.up); // Regular single shooting
            }
            PlaySFX(shootingSFX); // Play shooting sound effect
            nextFireTime = Time.time + 1f / fireRate; // Reset the fire timer for continuous firing
        }
    }

    private void ShootCannonBall(Vector2 direction)
    {
        // Instantiate the cannonball prefab
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * shootingForce); // Apply force in the calculated direction
    }

    private void ShootDoubleCannonBalls(Vector2 direction)
    {
        // Instantiate and shoot from the left fire point
        GameObject cannonBallLeft = Instantiate(cannonBallPrefab, firePointLeft.position, firePointLeft.rotation);
        Rigidbody2D rbLeft = cannonBallLeft.GetComponent<Rigidbody2D>();
        rbLeft.AddForce(direction * shootingForce);

        // Instantiate and shoot from the right fire point
        GameObject cannonBallRight = Instantiate(cannonBallPrefab, firePointRight.position, firePointRight.rotation);
        Rigidbody2D rbRight = cannonBallRight.GetComponent<Rigidbody2D>();
        rbRight.AddForce(direction * shootingForce);
    }

    // Method to activate double shooting mode for a limited time
    public void ActivateDoubleShooting(float duration)
    {
        isDoubleShooting = true;
        doubleShootingEndTime = Time.time + duration; // Set the end time for double shooting mode
        PlaySFX(powerUpStartSFX); // Play power-up start sound effect
    }

    // Method to disable double shooting
    public void DisableDoubleShooting()
    {
        isDoubleShooting = false; // End double shooting
        PlaySFX(powerUpEndSFX); // Play power-up end sound effect
    }

    // Method to play sound effects
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect
        }
    }

    // Method to destroy all player bullets
    public void DestroyBullets()
    {
        // Find all player bullets and destroy them
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("CannonBall");
        foreach (GameObject bullet in playerBullets)
        {
            Destroy(bullet);
        }
    }
}
