using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public Transform[] goal;
    public NavMeshAgent agent;
    public float averageSpeed = 20;
    public float RandomMult = 5;
    public bool randomSpeed;
    float maxSpeed;
    float speed;
    Transform target;

    float time = 0;
    public DataCollector data;

    Vector3 vel;

    void Start()
    {
        agent.isStopped = true;
        if (randomSpeed)
        {
            maxSpeed = averageSpeed + (Random.Range(0f,1f) * averageSpeed * RandomMult);
        }
        else
        {
            maxSpeed = averageSpeed;
        }  
    }

    public void Update()
    {

        time += Time.deltaTime;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath)
                {
                    agent.destination = goal[Mathf.FloorToInt(Random.Range(0, goal.Length))].position;
                    time = 0f;
                }
                else if(agent.velocity.sqrMagnitude == 0f)
                {
                    agent.destination = goal[Mathf.FloorToInt(Random.Range(0,goal.Length))].position;
                    data.Send(time);
                    time = 0f;
                }
            }
        }
        if (agent.hasPath)
        {
            
            if (agent.isOnOffMeshLink)
            {
                vel = (agent.currentOffMeshLinkData.endPos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vel, Vector3.up), agent.angularSpeed * Time.deltaTime * 3f);
                agent.Move(transform.forward*speed*Time.deltaTime);
                speed += agent.acceleration * Time.deltaTime;
                speed = Mathf.Clamp(speed, 0f, maxSpeed);

                if ((agent.currentOffMeshLinkData.endPos - transform.position).magnitude < Time.deltaTime * maxSpeed)
                {
                    agent.CompleteOffMeshLink();
                }
            }
            else
            {
                vel = (agent.path.corners[1] - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vel, Vector3.up), agent.angularSpeed * Time.deltaTime);
                agent.Move(transform.forward * speed * Time.deltaTime);
                float t = speed / agent.acceleration;
                float stoppingD = 0.5f * agent.acceleration * t * t;
                RaycastHit hit;
                if(Physics.Raycast(transform.position+transform.forward*5+transform.up, transform.forward, out hit, stoppingD + 1)||(agent.destination - transform.position).magnitude < stoppingD)
                {
                    speed -= agent.acceleration * Time.deltaTime;
                }
                else
                {
                    speed += agent.acceleration * Time.deltaTime;
                }
                
                speed = Mathf.Clamp(speed, 0f, maxSpeed);
            }
        }
        
        if (agent.path.corners.Length > 2)
        {
            
        }
        

        
        
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "agent")
        {
            data.Collision();
        }
        
    }

}
