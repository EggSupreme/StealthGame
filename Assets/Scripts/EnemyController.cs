using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Status
{
    navigating,
    waiting,
    combat
}

public class EnemyController : MonoBehaviour
{
    // general
    public Status currentStatus = Status.navigating;

    // navigation
    public List<Vector3> pathPoints = new List<Vector3>{ new Vector3(0.0f, 2.0f, 0.0f), new Vector3(2.0f, 2.0f, 0.0f), new Vector3(2.0f, 2.0f, 2.0f), new Vector3(0.0f, 2.0f, 2.0f) };
    NavMeshAgent agent;
    int goingTo = 1;

    // waiting
    const float waitTime = 5.0f;
    const float waitTimePlusMinus = 2.0f;
    float currentWaitTime = 0.0f;
    float currentWaitGoal = 2.0f;

    // combat
    GameObject combatTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(pathPoints[goingTo]);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, pathPoints[goingTo], new Color(1, 0, 1));
        
        switch (currentStatus)
        {
            case Status.navigating:
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentStatus = Status.waiting;
                    currentWaitGoal = waitTime + Random.Range(-waitTimePlusMinus, waitTimePlusMinus);
                    currentWaitTime = 0.0f;

                    goingTo = NextPathPoint();
                }

                break;

            case Status.waiting:
                if (currentWaitTime >= currentWaitGoal)
                {
                    currentStatus = Status.navigating;
                    agent.SetDestination(pathPoints[goingTo]);
                }
                else
                {
                    currentWaitTime += Time.deltaTime;
                }
                
                break;

            case Status.combat:

                break;
        }        
    }

    

    bool WithinRange(float _a, float _x, float _b)
    {
        if (_a > _b - _x && _a < _b + _x)
        {
            return true;
        }
        else { return false; }
    }

    int NextPathPoint()
    {
        int proposedNext = goingTo + 1;

        if (proposedNext >= pathPoints.Count)
        {
            proposedNext = 0;
        }

        return proposedNext;
    }
}
