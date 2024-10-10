using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera

    private void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckIfOutOfView();
    }

    private void CheckIfOutOfView()
    {
        // Get the viewport position of the cannonball (relative to camera view)
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the cannonball is outside the view (viewport coordinates are between 0 and 1)
        if (viewportPosition.x < 0 || viewportPosition.x > 1 ||
            viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject); // Destroy the cannonball if it is out of view
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add any additional functionality for when the cannonball enters a trigger collider, if needed
        // This could include hitting other objects, etc.
    }
}
