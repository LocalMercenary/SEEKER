using UnityEngine;

public class CircularChase : MonoBehaviour
{
    public Transform player; // Assign the Player Transform in the Inspector
    public float radius = 5f; // Radius of the circular path
    public float rotationSpeed = 30f; // Speed of circular movement
    public float moveSpeed = 2f; // Speed of movement towards the player

    private float angle = 0f; // Initial angle for rotation

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Assign Player Transform in the Inspector!");
            return;
        }
    }

    void Update()
    {
        MoveInCircle();
    }

    void MoveInCircle()
    {
        // Calculate the target position on the circular path
        Vector3 directionToPlayer = (player.position - transform.parent.position).normalized; // Direction to player
        float targetAngle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x) * Mathf.Rad2Deg; // Get angle to player
        angle = Mathf.MoveTowardsAngle(angle, targetAngle, rotationSpeed * Time.deltaTime); // Smoothly move towards the target angle

        // Calculate the new position in circular motion
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        // Set the new position based on circular motion around the parent's position
        transform.position = new Vector3(transform.parent.position.x + x, transform.position.y, transform.parent.position.z + z);
    }
}
