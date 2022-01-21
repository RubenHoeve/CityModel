using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public Transform[] goal;
    public NavMeshAgent agent;
    public float averageSpeed = 14;
    public float averageSteering = 250;
    public float averageAcceleration = 3.5f;
    public float averageDeceleration = 4.5f;
    public float RandomVMult = 0.5f;
    public float RandomAMult = 0.5f;
    public float RandomDMult = 0.5f;
    public bool AI;
    public int numTimes=12;
    float maxSpeed;
    float maxAcceleration;
    float maxDeceleration;
    float speed;
    Transform target;

    float time = 0;
    public DataCollector data;

    Vector3 vel;

    void Start()
    {
        agent.isStopped = true;
        if (!AI)
        {
            float r = Random.Range(-1f, 1f);
            maxSpeed = averageSpeed - ( r * averageSpeed * RandomVMult);
            agent.angularSpeed = averageSteering - (r * averageSteering * RandomVMult);
            maxAcceleration = averageAcceleration - (Random.Range(-1f, 1f) * averageAcceleration * RandomAMult);
            maxDeceleration = averageDeceleration - (Random.Range(-1f, 1f) * averageDeceleration * RandomDMult);
        }
        else
        {
            maxSpeed = averageSpeed;
            maxAcceleration = averageAcceleration;
            maxDeceleration = averageDeceleration;
        }
        agent.acceleration = maxAcceleration;
    }

    public void FixedUpdate()
    {

        time += Time.fixedDeltaTime;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath)
                {
                    newdest();
                    time = 0f;
                }
                else if(agent.velocity.sqrMagnitude <= 2f)
                {
                    newdest();
                    sendTime();
                    time = 0f;
                }
            }
        }
        if (agent.hasPath&&agent.path.corners.Length>=2)
        {
            
            if (agent.isOnOffMeshLink)
            {
                vel = (agent.currentOffMeshLinkData.endPos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vel, Vector3.up), agent.angularSpeed * Time.fixedDeltaTime * 3f);
                agent.Move(transform.forward*speed*Time.fixedDeltaTime);

                float t = speed / maxDeceleration;
                float stoppingD = 0.5f * maxDeceleration * t * t;
                RaycastHit Hit;

                bool hit = Physics.Raycast(transform.position + (transform.up * 0.5f), transform.forward, out Hit, stoppingD + 3);

                if (!hit && agent.remainingDistance >= stoppingD)
                {
                    speed += maxAcceleration * Time.fixedDeltaTime;
                }
                else
                {
                    speed -= maxDeceleration * Time.fixedDeltaTime;
                }

                speed = Mathf.Clamp(speed, 0f, maxSpeed);

                if ((agent.currentOffMeshLinkData.endPos - transform.position).magnitude <= Time.fixedDeltaTime * maxSpeed * 1.5f)
                {
                    agent.CompleteOffMeshLink();
                }
            }
            else
            {
                vel = (agent.path.corners[1] - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vel, Vector3.up), agent.angularSpeed * Time.fixedDeltaTime);
                agent.Move(transform.forward * speed * Time.fixedDeltaTime);
                float t = speed / maxDeceleration;
                float stoppingD = 0.5f * maxDeceleration * t * t;
                RaycastHit Rhit;
                RaycastHit Lhit;

                bool rhit = Physics.Raycast(transform.position + (transform.up * 0.5f) + (transform.right * 0.8f), transform.forward, out Rhit, stoppingD + 3);
                bool lhit = Physics.Raycast(transform.position + (transform.up * 0.5f) + (transform.right * -0.8f), transform.forward, out Lhit, stoppingD + 3);

                if (!rhit && !lhit && agent.remainingDistance >= stoppingD)
                {
                    speed += maxAcceleration * Time.fixedDeltaTime;
                }
                else
                {
                    speed -= maxDeceleration * Time.fixedDeltaTime;
                }

                speed = Mathf.Clamp(speed, 0f, maxSpeed);
            }
        }
        

        
        
        
    }

    private void sendTime()
    {
        if (numTimes > 0)
        {
            data.Send(time);
            numTimes--;
        }
        
    }

    private void newdest()
    {
        bool found = true;
        Vector3 dest = Vector3.zero;
        while (found)
        {
            dest = goal[Mathf.FloorToInt(Random.Range(0, goal.Length))].position;
            found = (transform.position - dest).magnitude < 75;
        }
        agent.destination = dest;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "agent" && !agent.isOnOffMeshLink)
        {
            data.Collision();
        }
        
    }

}
