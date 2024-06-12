using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] wayPoints;

    int currentWayPointIndex;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[0].position);
    }

    private void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
            agent.SetDestination(wayPoints[currentWayPointIndex].position);
        }
    }
}
