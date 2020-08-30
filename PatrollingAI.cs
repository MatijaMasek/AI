using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAI : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] Transform[] Waypoints;
    [SerializeField] float changeTime;
    
    [Header("Target Chase")]
    [SerializeField] Transform target;
    [SerializeField] float detectionRange;

    [Header("Patrolling Without Waypoints")]
    [SerializeField] bool usingWaypoints = true;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float y;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    Vector3 patrollingPosition;

    float targetDistance;
    float waitTime;

    int randomWaypoint;

    bool patrol = true;

    NavMeshAgent agent;

    void Start()
    {
        waitTime = changeTime;
        agent = GetComponent<NavMeshAgent>();
        if(usingWaypoints)
        {
            randomWaypoint = Random.Range(0, Waypoints.Length);
        }
        else
        {
            patrollingPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
        }
        
    }

    void Update()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);

        if (patrol)
        {
            //Patrolling using Waypoints
            if(usingWaypoints)
            {
                //Moving Towards Waypoint
                agent.destination = Waypoints[randomWaypoint].position;

                if (Vector3.Distance(transform.position, Waypoints[randomWaypoint].position) < 0.1f)
                {
                    if (waitTime <= 0)
                    {
                        randomWaypoint = Random.Range(0, Waypoints.Length);

                        waitTime = changeTime;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }
            }

            //Patrolling without using Waypoints
            else
            {
                //Moving Towards PatrollingPosition
                agent.destination = patrollingPosition;

                if (Vector3.Distance(transform.position, patrollingPosition) < 0.1f)
                {
                    if (waitTime <= 0)
                    {
                        patrollingPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));

                        waitTime = changeTime;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }
            }
            
        }

        //ChasingTarget
        if (targetDistance < detectionRange)
        {         
            patrol = false;
            agent.destination = target.position;
        }
        else
        {          
            patrol = true;
        }
    } 
    
}
