using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State currentState = State.Patrol;

    [Header("Waypoints")]
    public Transform[] waypoints;
    private int waypointIndex = 0;

    [Header("Detection")]
    public float detectionRange = 12f;
    public float attackRange = 2f;
    public float losePlayerRange = 18f;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float waypointStopDistance = 0.5f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 spawnPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoint = transform.position;
        GoToNextWaypoint();
    }

    void Update()
    {
        if (DeathManager.Instance.isDead) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distToPlayer <= detectionRange)
                    EnterChase();
                break;

            case State.Chase:
                Chase();
                if (distToPlayer <= attackRange)
                    EnterAttack();
                else if (distToPlayer > losePlayerRange)
                    EnterPatrol();
                break;

            case State.Attack:
                Attack();
                if (distToPlayer > attackRange)
                    EnterChase();
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < waypointStopDistance)
            GoToNextWaypoint();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.destination = waypoints[waypointIndex].position;
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    void Chase()
    {
        agent.destination = player.position;
    }

    void Attack()
    {
        agent.destination = transform.position;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            DealDamage();
        }
    }

    void DealDamage()
    {
        DeathManager.Instance.TriggerDeath();
    }

    public void ResetEnemy()
    {
        StartCoroutine(ResetCoroutine());
    }

    public IEnumerator ResetCoroutine()
    {
        agent.enabled = false;
        transform.position = spawnPoint;
        yield return null;
        agent.enabled = true;
        currentState = State.Patrol;
        waypointIndex = 0;
        GoToNextWaypoint();
    }

    void EnterPatrol()
    {
        currentState = State.Patrol;
        agent.speed = patrolSpeed;
        GoToNextWaypoint();
    }

    void EnterChase()
    {
        currentState = State.Chase;
        agent.speed = chaseSpeed;
    }

    void EnterAttack()
    {
        currentState = State.Attack;
        agent.speed = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, losePlayerRange);
    }
}