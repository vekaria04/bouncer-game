using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent henchman;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("LowPolyMan").transform;
        henchman = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckForPlayerRanges();

        if (playerInSightRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    private void CheckForPlayerRanges()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    private void ChasePlayer()
    {
        henchman.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        henchman.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            PerformAttack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PerformAttack()
    {
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5.5f, ForceMode.Impulse);
        rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}