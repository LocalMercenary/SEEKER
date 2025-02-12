using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("FOV")]
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    [Header("Movement")]
    [SerializeField]
    bool sendHome = false;
    Vector3 home;
    GameObject player;
    NavMeshAgent agent;

    [Header("Wandering")]
    [SerializeField]
    bool wander = true;
    public float playerRadius;
    private bool isWandering = false; // Prevents multiple wandering coroutines from running

    private void Start()
    {
        // FOV setup
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());

        // Movement setup
        home = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;

        if (!canSeePlayer && !sendHome && !wander && !isWandering)
        {
            wander = true;
        }

        if (canSeePlayer)
        {
            agent.speed = 13f;
            agent.destination = player.transform.position; // Chase player
            wander = false;
            isWandering = false; // Stop wandering
            StopCoroutine(WanderRoutine()); // Ensure wandering stops
        }
        else if (sendHome)
        {
            agent.speed = 7f;
            agent.destination = home; // Return home when not seeing the player
        }

        if (wander && !isWandering)
        {
            agent.speed = 7f;
            StartCoroutine(WanderRoutine()); // Start wandering if not already
        }
    }

    private IEnumerator WanderRoutine()
    {
        isWandering = true;

        while (wander && !canSeePlayer)
        {
            Vector3 wanderPoint = GetRandomPointNearPlayer();
            agent.SetDestination(wanderPoint);

            // Wait until the path is calculated
            yield return new WaitUntil(() => !agent.pathPending);

            // Wait until the agent reaches the target
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

            // Wait before picking a new point
            yield return new WaitForSeconds(0.1f);
        }

        isWandering = false;
    }

    private Vector3 GetRandomPointNearPlayer()
    {
        // Generate random point in the X-Z plane instead of X-Y
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * playerRadius;
        Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y) + player.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, playerRadius, NavMesh.AllAreas))
        {
            return hit.position; // Return a valid NavMesh point
        }
        return transform.position; // Fallback to the current position
    }

    // Field of View Routine
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    return;
                }
            }
        }

        canSeePlayer = false;
    }
}
