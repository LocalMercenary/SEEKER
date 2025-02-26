using UnityEngine;

public class CircularChase : MonoBehaviour
{
    public Transform player; // Assign the Player Transform in the Inspector
    public float maxRadius = 5f; // Maximum radius of the circular path
    public float minRadius = 1f; // Minimum radius when the player is very close
    public float rotationSpeed = 30f; // Speed of circular movement
    public float moveSpeed = 2f; // Speed of movement towards the player

    private float angle = 0f; // Initial angle for rotation
    private float currentRadius; // Dynamic radius

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Assign Player Transform in the Inspector!");
            return;
        }
        currentRadius = maxRadius; // Start at max radius
    }

    void Update()
    {
        AdjustRadius();
        MoveInCircle();
    }

    void AdjustRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If player is within 5 units, decrease radius based on distance
        if (distanceToPlayer < 5f)
        {
            currentRadius = Mathf.Lerp(minRadius, maxRadius, distanceToPlayer / 5f);
        }
        else
        {
            currentRadius = maxRadius;
        }
    }

    void MoveInCircle()
    {
        // Calculate the target position on the circular path
        Vector3 directionToPlayer = (player.position - transform.parent.position).normalized;
        float targetAngle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x) * Mathf.Rad2Deg;
        angle = Mathf.MoveTowardsAngle(angle, targetAngle, rotationSpeed * Time.deltaTime);

        // Calculate the new position in circular motion with the adjusted radius
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * currentRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * currentRadius;

        // Set the new position based on circular motion around the parent's position
        transform.position = new Vector3(transform.parent.position.x + x, transform.position.y, transform.parent.position.z + z);
    }
}
