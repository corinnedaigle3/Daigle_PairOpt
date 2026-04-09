using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [Range(0, 50)][SerializeField] float attackRange = 2, sightRange = 20, timeBetweenAttacks = 3;
    [Range(0, 20)][SerializeField] int atkDamage = 5;

    [SerializeField] NavMeshAgent navEnemy;
    private Transform playerPos;
    private bool isAttacking;

    [SerializeField] Health health;
    [SerializeField] PlayerController playerController;
    public GameObject bullet;

    private IObjectPool<Enemy> enemyPool;

    public void SetPool(IObjectPool<Enemy> pool)
    {
        enemyPool = pool;
    }

    private enum EnemyState
    {
        Chase,
        Attack,
        Idle,
        Dead
    }

    private void OnEnable()
    {
        if (navEnemy == null)
            navEnemy = GetComponent<NavMeshAgent>();

        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        playerPos = playerController.transform;

        isAttacking = false;

        navEnemy.isStopped = false;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, transform.position);

        EnemyState currentState = GetState(distanceFromPlayer);

        switch (currentState)
        {
            case EnemyState.Chase:
                isAttacking = false;
                navEnemy.isStopped = false;
                StopAllCoroutines();
                ChasePlayer();
                break;

            case EnemyState.Attack:
                navEnemy.isStopped = true;
                if (!isAttacking)
                    StartCoroutine(AttackPlayer());
                break;

            case EnemyState.Idle:
                navEnemy.isStopped = true;
                break;

            case EnemyState.Dead:
                Debug.Log("I am dead");
                enemyPool.Release(this);
                break;
        }
    }

    private EnemyState GetState(float distance)
    {
        if (health.isDead)
            return EnemyState.Dead;

        if (distance <= attackRange && !isAttacking)
            return EnemyState.Attack;

        if (distance <= sightRange && distance > attackRange)
            return EnemyState.Chase;

        return EnemyState.Idle;
    }

    private void ChasePlayer()
    {
        navEnemy.SetDestination(playerPos.position);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        yield return new WaitForSeconds(timeBetweenAttacks);

        Debug.Log("hit Player");
        health.TakeDamage(atkDamage);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}