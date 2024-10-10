using UnityEngine;

public class DoubleBullet : MonoBehaviour
{
    public float powerUpDuration = 5f; // Duration of the power-up effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with has the Cannon component
        Cannon cannon = collision.GetComponent<Cannon>();
        if (cannon != null)
        {
            cannon.ActivateDoubleShooting(powerUpDuration); // Trigger double shooting mode
            Destroy(gameObject); // Destroy the power-up object after activation
        }
    }
}
