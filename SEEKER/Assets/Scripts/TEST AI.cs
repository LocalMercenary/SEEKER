using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TESTAI : MonoBehaviour
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
    bool sendHome = true;
    Vector3 home;
    GameObject player;
    NavMeshAgent agent;

    [Header("Wandering")]
    [SerializeField]
    bool wandering = true;
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

        if (canSeePlayer)
        {
            agent.destination = player.transform.position; // Chase player
            wandering = false;
            isWandering = false; // Stop wandering
            StopCoroutine(WanderRoutine()); // Ensure wandering stops
        }
        else if (sendHome)
        {
            agent.destination = home; // Return home when not seeing the player
        }

        if (wandering && !isWandering)
        {
            StartCoroutine(WanderRoutine()); // Start wandering if not already
        }
    }

    private IEnumerator WanderRoutine()
    {
        isWandering = true;

        while (wandering && !canSeePlayer)
        {
            Vector3 wanderPoint = GetRandomPointNearPlayer();
            agent.SetDestination(wanderPoint);

            // Wait until the enemy reaches the destination
            while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            // Wait for a short time before choosing another point
            yield return new WaitForSeconds(2f);
        }

        isWandering = false;
    }

    private Vector3 GetRandomPointNearPlayer()
    {
        Vector3 randomPoint = player.transform.position + (Vector3)(playerRadius * UnityEngine.Random.insideUnitCircle);

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
