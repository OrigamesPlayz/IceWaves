using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public WaveSpawner waveSpawner;

    [Header("EnemyAI")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform playerHit;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public GameObject projectile;
    public Transform attackPoint;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;
    [SerializeField] private bool dead;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        playerHit = GameObject.Find("CameraPos").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(playerHit.position);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, attackPoint.position, attackPoint.rotation).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * attackRange);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(KillEnemy), .25f);
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
        waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
    }
}
