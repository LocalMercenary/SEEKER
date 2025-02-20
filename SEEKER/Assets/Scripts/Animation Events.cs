using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private MonsterHide monsterHide;
    private RaycastScript interact;
    private Puzzle2 puzzle; // Reference to Puzzle2 script
    public List<GameObject> objectsToHide; // Assign other objects that need to be hidden

    public float enemyRadius = 2f;

    void Start()
    {
        interact = FindObjectOfType<RaycastScript>(); // Find the RaycastScript in the scene
        if (interact == null)
        {
            Debug.LogError("RaycastScript not found in the scene!");
        }
        monsterHide = FindObjectOfType<MonsterHide>(); // Find the RaycastScript in the scene
        if (interact == null)
        {
            Debug.LogError("MonsterHide not found in the scene!");
        }
        puzzle = FindObjectOfType<Puzzle2>(); // Find Puzzle2 in the scene
        if (puzzle == null)
        {
            Debug.LogError("Puzzle2 script not found in the scene!");
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRadius);
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void HideObjects()
    {
        // Hide player if assigned
        if ((interact != null) && interact.player != null)
        {
            interact.player.SetActive(false);
        }

        // Hide all other assigned objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void UnhideObjects()
    {
        
        // Unhide player if assigned
        if ((interact != null) && interact.player != null)
        {
            interact.player.SetActive(true);
        }

        // Unhide all other assigned objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    public void EnemyNear()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                interact.enemy.SetActive(false);
            }
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    public void EnemyNearEnd()
    {
        // Unhide objects immediately
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Start the coroutine to delay enemy activation
        StartCoroutine(DelayedEnemyActivation());
    }

    private IEnumerator DelayedEnemyActivation()
    {
        yield return new WaitForSeconds(20f); // Wait for 15 seconds

        if ((interact != null) && interact.enemy != null)
        {
            interact.enemy.SetActive(true);

            // Restart MoveImageRoutine in MonsterHide
            if (monsterHide != null)
            {
                foreach (RectTransform image in monsterHide.images)
                {
                    monsterHide.StartCoroutine(monsterHide.MoveImageRoutine(image));
                }
            }
        }
    }


    public void rotated1() { if (puzzle != null) puzzle.Rotated1 = true; }
    public void Unrotated1() { if (puzzle != null) puzzle.Rotated1 = false; }
    public void rotated2() { if (puzzle != null) puzzle.Rotated2 = true; }
    public void Unrotated2() { if (puzzle != null) puzzle.Rotated2 = false; }
    public void rotated3() { if (puzzle != null) puzzle.Rotated3 = true; }
    public void Unrotated3() { if (puzzle != null) puzzle.Rotated3 = false; }
    public void rotated4() { if (puzzle != null) puzzle.Rotated4 = true; }
    public void Unrotated4() { if (puzzle != null) puzzle.Rotated4 = false; }
}
