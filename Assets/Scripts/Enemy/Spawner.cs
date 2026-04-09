using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBetweenSpawns = 5;
    private float timeSinceLastSpawn;

    [SerializeField] Enemy enemyPrefab;
    private IObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeEnemyFromPool);
    }

    private void OnTakeEnemyFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }


    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            Enemy enemy = enemyPool.Get();

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //enemy.transform.position = spawnPoint.position;

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            agent.Warp(spawnPoint.position);

            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
}
