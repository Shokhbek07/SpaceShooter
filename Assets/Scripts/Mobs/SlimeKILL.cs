using UnityEngine;

public class Slime : MonoBehaviour
{
    public string cannonBallTag = "CannonBall"; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object this slime collided with has the tag of a cannonball
        if (collision.gameObject.CompareTag(cannonBallTag))
        {
            Destroy(gameObject); // Destroy the slime on collision
            Destroy(collision.gameObject); // Optional: Destroy the cannonball as well
        }
    }
}
