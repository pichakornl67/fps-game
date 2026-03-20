using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnRadius = 15f;
    public float minSpawnDistance = 5f;

    public float spawnDelay = 1.0f;
    public float timeBetweenWaves = 5f;

    private int waveNumber = 0;
    private int enemiesToSpawn = 15;

    private int enemiesRemaining; // 👈 THIS is what you want

    public int CurrentWave => waveNumber;
    public int EnemiesLeft => enemiesRemaining;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            waveNumber++;

            int spawnCount = enemiesToSpawn;

            // 👇 reset remaining count at start of wave
            enemiesRemaining = spawnCount;

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            // 👇 wait until all enemies are dead
            while (enemiesRemaining > 0)
            {
                yield return null;
            }

            enemiesToSpawn += 5;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        Vector2 random2D = Random.insideUnitCircle * spawnRadius;

        if (random2D.magnitude < minSpawnDistance)
        {
            random2D = random2D.normalized * minSpawnDistance;
        }

        Vector3 spawnPos = new Vector3(
            player.position.x + random2D.x,
            player.position.y,
            player.position.z + random2D.y
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 5f, NavMesh.AllAreas))
        {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);

            Enemyhealth eh = enemy.GetComponent<Enemyhealth>();
            if (eh != null)
            {
                eh.onDeath += EnemyDied;
            }
        }
    }

    void EnemyDied()
    {
        enemiesRemaining--; // 👈 THIS makes it go 15 → 14 → 13...

        Debug.Log("Enemies left: " + enemiesRemaining);
    }
}