using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("FOV")]
    public float radius;
    [Range (0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;



    [Header ("Movement")]
    
    [SerializeField]
    bool sendHome = true;
    Vector3 home;
    GameObject player;
    NavMeshAgent agent;

    [Header ("wandering")]
    [SerializeField]
    bool wandering = true;
    public float playerRadius;

    private void Start()
    {

        //fov
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());

        //movement
        home = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {

        if (canSeePlayer && sendHome)
        {
            wandering = true;
        }
        //movement
        Vector3 dir = player.transform.position - transform.position;
        if (canSeePlayer && dir.magnitude < radius)
        {
            agent.destination = player.transform.position;
        }
        else if (sendHome == true)
        {
            agent.destination = home;
        }
        if (wandering == true)
        {
            Vector3 centerOfRadius = player.transform.position;

            Vector3 target = centerOfRadius + (Vector3)(playerRadius * UnityEngine.Random.insideUnitCircle);
        }
    }

    //fov
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    //fov
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
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
            
    }
}
