using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public Transform[] goal;
    public NavMeshAgent agent;
    public float baseSpeed = 20;
    Transform target;

    void Start()
    {
        
    }

    public void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.destination = goal[Mathf.FloorToInt(Random.Range(0,94.9f))].position;
                }
            }
        }
        if (agent.isOnOffMeshLink)
        {
            agent.speed = baseSpeed / 3;
            Debug.Log(agent.velocity);
        }
        else
        {
            agent.speed = baseSpeed;
        }
    }
    
}
