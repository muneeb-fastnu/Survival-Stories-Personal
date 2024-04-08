using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public SurroundingCheckAction surroundCheck;
    public State currentState;
    public State remainState;
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 lastKnownMyLocation;
    public bool _isActive;
    public Animator characterAnim;
    public Vector3 patrolPoint;
    public string enemyType;
    private void Awake()
    {
        enemyType = "Enemy";
        agent = this.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lastKnownMyLocation = transform.position;
        GeneratePatrolPoint();
        InitializeAI();
    }
    private void Start()
    {
        if (gameObject.tag == "Mole")
        {
            agent.speed *= agent.speed * 10;
        }
        
    }
    void Update()
    {
        if (!_isActive) return;
        characterAnim.SetFloat("Blend", currentState.animationBlendTreeFloat);
        if (this != null)
        {
            currentState.UpdateState(this);
        }
        
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;

            

        }
    }
    public void InitializeAI()
    {

        _isActive = true;
        agent.enabled = _isActive;
    }
    public int patrolRange = 50;
    public void GeneratePatrolPoint()
    {
        int[] numbers = new int[8];
        for (int i = 0; i < numbers.Length; i++)
        {
            int randomNumber = Random.Range(1, 5); // 5 is exclusive
            numbers[i] = randomNumber;
        }
        // Define the four patrol points relative to the lastKnownMyLocation
        Vector3[] patrolPoints = {
        new Vector3(lastKnownMyLocation.x + patrolRange, lastKnownMyLocation.y, lastKnownMyLocation.z),
        new Vector3(lastKnownMyLocation.x - patrolRange, lastKnownMyLocation.y, lastKnownMyLocation.z),
        new Vector3(lastKnownMyLocation.x, lastKnownMyLocation.y + patrolRange, lastKnownMyLocation.z),
        new Vector3(lastKnownMyLocation.x, lastKnownMyLocation.y - patrolRange, lastKnownMyLocation.z)
        };

        Vector3 selectedPoint = patrolPoints[0];
        for (int i = 0; i < numbers.Length; i++)
        {
            selectedPoint = patrolPoints[numbers[i] - 1];

            // Check if the selected patrol point is walkable
            if (NavMesh.SamplePosition(selectedPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                // If walkable, set as patrol point
                patrolPoint = hit.position;
                return;
            }
            
        }
        patrolPoint = selectedPoint;
    }
}
