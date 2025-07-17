using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour, IMonsterState
{
    // Behaviour variables
    public float patrolRadius = 10f;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public float nextPointMinDistance = 5f;
    public float nextPointMaxDistance = 50f;
    public float metTargetDistance = 1f;

    // Component/gameobject references
    private NavMeshAgent agent;

    // Runtime variables
    private Vector3 nextPoint;
    private bool isWaiting = false;

    // Stuck detection variables
    private Vector3 lastPosition;
    private float stuckCheckInterval = 3f;
    private float stuckTimer = 0f;
    private float minimalMovementThreshold = 0.1f;
    private int stuckRetries = 0;
    private int maxStuckRetries = 3;

    // Called when entering the state
    public void Enter(Monster monster)
    {
        agent = monster.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"{monster.name} is missing NavMeshAgent component!");
            return;
        }

        agent.speed = moveSpeed;
        agent.isStopped = false;

        PickNewPoint(monster);
        lastPosition = agent.transform.position;
        stuckTimer = 0f;
        stuckRetries = 0;
        isWaiting = false;
    }

    // Called every frame when this state is active
    public void BehaviourUpdate(Monster monster)
    {
        if (isWaiting || agent == null) return;

        if (!agent.pathPending && agent.remainingDistance <= metTargetDistance)
        {
            monster.StartCoroutine(WaitAndMove(monster));
        }

        StuckDetection(monster);
    }

    // Called when exiting the state
    public void Exit(Monster monster)
    {
        if (agent != null)
            agent.isStopped = true;
    }

    // Picks a new point from NavmeshPointGenerator.Points within min/max distance
    private void PickNewPoint(Monster monster)
    {
        var allPoints = NavmeshPointGenerator.Points;
        if (allPoints == null || allPoints.Count == 0)
        {
            Debug.LogWarning("No navmesh points available for patrol.");
            return;
        }

        Vector3 currentPos = monster.transform.position;
        Vector3 selectedPoint = currentPos;

        // Find a random valid point within min/max distance
        var candidates = new System.Collections.Generic.List<Vector3>();
        foreach (var point in allPoints)
        {
            float dist = Vector3.Distance(currentPos, point);
            if (dist >= nextPointMinDistance && dist <= nextPointMaxDistance)
            {
                candidates.Add(point);
            }
        }

        if (candidates.Count > 0)
        {
            selectedPoint = candidates[Random.Range(0, candidates.Count)];
        }
        else
        {
            // fallback: just pick any point
            selectedPoint = allPoints[Random.Range(0, allPoints.Count)];
        }

        nextPoint = selectedPoint;
        agent.SetDestination(nextPoint);
    }

    // Basic stuck detection: if agent barely moves over interval, retry or respawn
    private void StuckDetection(Monster monster)
    {
        stuckTimer += Time.deltaTime;

        if (stuckTimer >= stuckCheckInterval)
        {
            float distanceMoved = Vector3.Distance(agent.transform.position, lastPosition);
            if (distanceMoved < minimalMovementThreshold)
            {
                stuckRetries++;
                if (stuckRetries >= maxStuckRetries)
                {
                    // Respawn or kill monster if stuck too long
                    Debug.Log($"{monster.name} got stuck and will respawn.");
                    monster.TakeDamage(10000);
                }
                else
                {
                    Debug.Log($"{monster.name} got stuck, picking new patrol point.");
                    PickNewPoint(monster);
                }
            }

            stuckTimer = 0f;
            lastPosition = agent.transform.position;
        }
    }

    // Coroutine to wait at the point before moving to the next
    private IEnumerator WaitAndMove(Monster monster)
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        PickNewPoint(monster);
        isWaiting = false;
    }
}
