using System.Collections;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public Puzzle2 puzzle; // Reference to Puzzle2 script

    private bool hasPlayed = false; // Prevent multiple triggers

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Assign Animator if not set
        }
        if (puzzle == null)
        {
            puzzle = FindObjectOfType<Puzzle2>(); // Automatically find Puzzle2 in scene
        }
    }

    void Update()
    {
        if (puzzle != null && puzzle.EnemyDead && !hasPlayed)
        {
            hasPlayed = true; // Ensure it only runs once
            StartCoroutine(PlayDeathAnimation());
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (animator != null)
        {
            animator.SetInteger("moving", 13); // Set animation
        }
    }
}
