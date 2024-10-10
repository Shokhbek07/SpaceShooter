using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1.5f; // Speed at which the background moves
    public float resetPositionY = -25f; // Y position where the background should reset
    public float startPositionY = 10f; // Y position to reset the background to

    private Vector3 startPosition;

    private void Start()
    {
        // Store the initial position of the background
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move the background downwards based on the scroll speed
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // If the background has moved past the reset point, reset its position to start
        if (transform.position.y <= resetPositionY)
        {
            transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z);
        }
    }
}
