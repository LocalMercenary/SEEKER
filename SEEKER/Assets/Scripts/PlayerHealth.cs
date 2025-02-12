using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1; // Player's starting health
    public float damageRadius = 2f; // Radius around player for enemy damage
    public int damageAmount = 1; // Damage per hit
    public float damageInterval = 1f; // Time between damage ticks

    private float damageTimer = 0f;

    void Update()
    {
        damageTimer += Time.deltaTime;

        if (health <= 0)
        {
            // Reload the level when health is depleted
            SceneManager.LoadScene("Lose");
        }

        CheckForEnemies();
    }

    void CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && damageTimer >= damageInterval)
            {
                TakeDamage(damageAmount);
                damageTimer = 0f; // Reset damage timer
            }
        }
    }

    void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player Health: " + health);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
