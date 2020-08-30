using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAI : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] Transform[] Waypoints;
    [SerializeField] float changeTime;
    
    [Header("TargetChase")]
    [SerializeField] Transform target;
    [SerializeField] float detectionRange;

    float targetDistance;
    float waitTime;

    int randomWaypoint;

    bool patrol = true;

    NavMeshAgent agent;

    void Start()
    {
        waitTime = changeTime;
        agent = GetComponent<NavMeshAgent>();        
        randomWaypoint = Random.Range(0, Waypoints.Length);
    }

    void Update()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);     

        if(patrol)
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

        //ChasingTarget
        if(targetDistance < detectionRange)
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
