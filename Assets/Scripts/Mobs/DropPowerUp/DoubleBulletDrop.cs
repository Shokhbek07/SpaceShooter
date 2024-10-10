using UnityEngine;

public class DropOnCollision : MonoBehaviour
{
    public GameObject dropPrefab; // Prefab to drop
    public GameObject replacementPrefab; // Prefab to replace with
    [Range(0, 1)] public float dropRate = 0.05f; // 5% drop rate

    private SlimeSpawner slimeSpawner; // Reference to the SlimeSpawner

    private void Start()
    {
        // Get reference to SlimeSpawner
        slimeSpawner = FindObjectOfType<SlimeSpawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is tagged as "CannonBall"
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            if (Application.isPlaying)
            {
                TryDropItem();
                ReplaceWithPrefab(); // Replace this object with the new prefab
                IncreaseScore(); // Increase the score when a slime is defeated
            }
        }
    }

    private void TryDropItem()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= dropRate)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        Instantiate(dropPrefab, transform.position, Quaternion.identity);
    }

    private void ReplaceWithPrefab()
    {
        if (replacementPrefab != null)
        {
            Instantiate(replacementPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void IncreaseScore()
    {
        // Increase the score in the SlimeSpawner
        if (slimeSpawner != null)
        {
            slimeSpawner.IncreaseScore(1); // Increase by 1 (or any value you want)
        }
    }
}
