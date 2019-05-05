using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform[] navGoals = new Transform[6];
    public int currentNavGoalIndex = 0;
    public Transform currentNavGoal;
    NavMeshAgent agent;

    private static float PAUSE_TIME_MIN = 200f;
    private static float PAUSE_TIME_MAX = 1000f;
    public float defaultPauseTime = 200f;
    public float pauseTime;

    private static float MOVE_SPEED_MIN = 1f;
    private static float MOVE_SPEED_MAX = 3.5f;
    public float moveSpeed = 3f;

    private float reachGoalDist = 1f;

    private bool paused = false;

    private void Start()
    {
        currentNavGoalIndex = RandomGoalIndex();
        this.gameObject.transform.position = navGoals[currentNavGoalIndex].position;

        currentNavGoal = NextGoal();

        Debug.Log("MoveTo.Start(), gameObject = " + this.gameObject.name + ", currentNavGoal = " + currentNavGoal.ToString());

        agent = GetComponent<NavMeshAgent>();
        agent.destination = currentNavGoal.position;
        agent.speed = RandomMoveSpeed();

        defaultPauseTime = RandomPauseTime();
        pauseTime = defaultPauseTime;

    }

    private void FixedUpdate()
    {
        
        if (ReachedGoal() && !paused)
        {
            Debug.Log("MoveTo.Update() REACHED GOAL condition");
            pauseTime = RandomPauseTime();
            paused = true;

            currentNavGoal = NextGoal();
            agent.destination = currentNavGoal.position;

        }

        if (paused)
        {
            UpdateNavPause();
        }
    }

    private Transform NextGoal()
    {
        Debug.Log("MoveTo.NextGoal()");
        if (currentNavGoalIndex == navGoals.Length - 1)
        {
            currentNavGoalIndex = 0;
        }
        else
        {
            currentNavGoalIndex++;
        }
        return navGoals[currentNavGoalIndex];

    }

    private void UpdateNavPause()
    {
        Debug.Log("MoveTo.UpdateNavPause()");

        if (pauseTime > 0)
        {
            agent.speed = 0;
            pauseTime--;
        }

        // If we've elapsed the wait time, then set the speed back to 0
        if (pauseTime <= 0)
        {
            Debug.Log("Elapsed Nav Pause");
            agent.speed = RandomMoveSpeed();
            paused = false;
        }
    }

    private bool ReachedGoal()
    {
        Debug.Log("Checking if reached goal");

        float distance = (this.gameObject.transform.position - currentNavGoal.position).magnitude;

        if (distance <= reachGoalDist)
        {
            Debug.Log("MoveTo.ReachedGoal() TRUE");
        }


        return distance <= reachGoalDist;
    }

    private int RandomGoalIndex()
    {
        return Random.Range(0, navGoals.Length - 1);
    }

    private float RandomPauseTime()
    {
        return Random.Range(PAUSE_TIME_MIN, PAUSE_TIME_MAX);
    }

    private float RandomMoveSpeed()
    {
        return Random.Range(MOVE_SPEED_MIN, MOVE_SPEED_MAX);
    } 
}