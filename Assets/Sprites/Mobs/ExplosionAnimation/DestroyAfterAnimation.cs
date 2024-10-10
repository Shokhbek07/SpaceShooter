using UnityEngine;
using System.Collections;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this object!");
        }
        else
        {
            Debug.Log("Animator component found. Waiting for animation to finish.");
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the animation is complete
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log($"Waiting for animation to finish. Duration: {animationLength} seconds.");
        yield return new WaitForSeconds(animationLength);
        
        Debug.Log("Animation finished. Destroying object.");
        Destroy(gameObject); // Destroy the game object after the animation
    }
}
