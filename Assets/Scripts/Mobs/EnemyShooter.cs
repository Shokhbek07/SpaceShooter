using UnityEngine;

public class SlimeShooter : MonoBehaviour
{
    public GameObject slimeBulletPrefab; // Prefab for the slime's bullet
    public Transform firePoint; // Point from where the bullet is fired
    public float shootingForce = 50f; // Force applied to the bullet
    public float fireRate = 10f; // Time interval between bullet shots in seconds
    private float nextFireTime = 0f; // Time control for continuous shooting
    private float randomOffset; // Random offset for the firing time

    private void Start()
    {
        // Set a random offset for the initial fire time
        randomOffset = Random.Range(0f, fireRate); // Randomize the start time within the fireRate
        nextFireTime = Time.time + randomOffset; // Set the first firing time with the random offset
    }

    private void Update()
    {
        HandleShooting(); // Continuously check for firing bullets
    }

    private void HandleShooting()
    {
        // Check if it's time to fire the bullet
        if (Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate; // Reset the fire timer using time interval
        }
    }

    private void ShootBullet()
    {
        // Instantiate the bullet prefab
        GameObject bullet = Instantiate(slimeBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        // Apply force to the bullet to make it move downwards
        rb.AddForce(-firePoint.up * shootingForce); // Shoot downwards from the firePoint
    }
}
