using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    [Header("FOV")]
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    private Coroutine loseSightCoroutine; // Reference to sight loss coroutine
    public float memoryDuration = 3f; // Time the enemy "remembers" the player

    [Header("Movement")]
    [SerializeField]
    bool sendHome = false;
    Vector3 home;
    GameObject player;
    NavMeshAgent agent;

    [Header("Wandering")]
    [SerializeField]
    bool wander = true;
    private RaycastScript interact;
    public float playerRadius;
    private bool isWandering = false; // Prevents multiple wandering coroutines from running
    public bool Restart = true;
    public int wanderingSpeed = 6;
    public int chaseSpeed = 13;
    public int homeSpeed = 5;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // FOV setup
        playerRef = GameObject.FindGameObjectWithTag("Player");

        // Movement setup
        home = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        interact = FindObjectOfType<RaycastScript>(); // Find the RaycastScript in the scene
        if (interact == null)
        {
            Debug.LogError("RaycastScript not found in the scene!");
        }
    }

    void Update()
    {
        if (Restart)
        {
            StartCoroutine(FOVRoutine()); // Restart FOV checking
            Restart = false;
        }
        if (canSeePlayer)
        {
            agent.speed = chaseSpeed;
            agent.destination = player.transform.position; // Chase player
            wander = false;
            isWandering = false; // Stop wandering
            StopCoroutine(WanderRoutine()); // Ensure wandering stops
        
        }
        else if (sendHome)
        {
            agent.speed = homeSpeed;
            agent.destination = home; // Return home when not seeing the player
        }
        if (!canSeePlayer && !sendHome && !wander && !isWandering)
        {
            wander = true;
        }
        if (wander && !isWandering)
        {
            agent.speed = wanderingSpeed;
            StartCoroutine(WanderRoutine()); // Restart wandering
        }
        int collectedCount2 = 0;
        if (interact != null)
        {
            if (interact.hasCollectedItem1) collectedCount2++;
            if (interact.hasCollectedItem2) collectedCount2++;
            if (interact.hasCollectedItem3) collectedCount2++;
            if (interact.hasCollectedItem4) collectedCount2++;
        }
        if (collectedCount2 >= 3)
        {
            wanderingSpeed = 6;
            chaseSpeed =  14;
        }
        if (collectedCount2 >= 4)
        {
            wanderingSpeed = 7;
            chaseSpeed = 16;
        }

        anim.SetInteger("moving", agent.velocity.magnitude > 0.1f ? 1 : 0);
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

                    // If we were about to lose sight, cancel the coroutine
                    if (loseSightCoroutine != null)
                    {
                        StopCoroutine(loseSightCoroutine);
                        loseSightCoroutine = null;
                    }
                    
                    return;
                }
            }
        }

        // Start memory timer only if the player was seen before
        if (canSeePlayer && loseSightCoroutine == null)
        {
            loseSightCoroutine = StartCoroutine(LoseSightDelay());
        }
    }

    private IEnumerator LoseSightDelay()
    {
        yield return new WaitForSeconds(memoryDuration);
        canSeePlayer = false;
        loseSightCoroutine = null;
        
    }
    void OnEnable()
    {
        Restart = true; // Restart FOV checking when re-enabled
        wander = true; // Allow wandering again
        isWandering = false; // Ensure wandering restarts properly

        if (agent != null)
        {
            agent.enabled = true; // Ensure NavMeshAgent is enabled
        }
    }


}

