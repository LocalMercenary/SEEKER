using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2 : MonoBehaviour
{
    [Header("EnemyChecks")]
    public float Radius = 2f; // Radius around player for enemy damage
    public GameObject objectToDestroy;


    [Header("PuzzleChecks")]
    public bool Rotated1 = false;
    public bool Rotated2 = false;
    public bool Rotated3 = false;
    public bool Rotated4 = false;
    public bool FullyRotated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Rotated1 &&  Rotated2 && Rotated3 && Rotated4)
        {
            FullyRotated = true;
        }
        else
        {
            FullyRotated = false;
        }
        CheckForEnemies();
    }
    void CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && FullyRotated)
            {
                gameObject.GetComponent<Animator>().SetTrigger("Die");
                Destroy(objectToDestroy);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
