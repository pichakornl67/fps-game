using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnRadius = 15f;
    public float minSpawnDistance = 5f;

    public float spawnDelay = 1.0f;
    public float timeBetweenWaves = 5f;

    public RectTransform dotPrefab;
    public int maxDots = 5;

    public Camera cam;

    private int waveNumber = 0;
    private int enemiesToSpawn = 15;
    private int enemiesRemaining;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private List<RectTransform> dots = new List<RectTransform>();

    public int EnemiesLeft
    {
        get { return enemiesRemaining; }
    }

    public int CurrentWave
    {
        get { return waveNumber; }
    }

    void Start()
    {
        for (int i = 0; i < maxDots; i++)
        {
            RectTransform newDot = Instantiate(dotPrefab, dotPrefab.parent);
            newDot.gameObject.SetActive(false);
            dots.Add(newDot);
        }

        dotPrefab.gameObject.SetActive(false); 

        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            waveNumber++;

            int spawnCount = enemiesToSpawn;
            enemiesRemaining = spawnCount;
            aliveEnemies.Clear();

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            while (enemiesRemaining > 0)
                yield return null;

            enemiesToSpawn += 5;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        Vector2 random2D = Random.insideUnitCircle * spawnRadius;

        if (random2D.magnitude < minSpawnDistance)
            random2D = random2D.normalized * minSpawnDistance;

        Vector3 spawnPos = new Vector3(
            player.position.x + random2D.x,
            player.position.y,
            player.position.z + random2D.y
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 5f, NavMesh.AllAreas))
        {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            aliveEnemies.Add(enemy);

            Enemyhealth eh = enemy.GetComponent<Enemyhealth>();
            if (eh != null)
                eh.onDeath += EnemyDied;
        }
    }

    void EnemyDied()
    {
        enemiesRemaining--;
        aliveEnemies.RemoveAll(e => e == null);

        Debug.Log("Enemies left: " + enemiesRemaining);
    }

    void Update()
    {
        if (enemiesRemaining > 5)
        {
            foreach (var d in dots)
                d.gameObject.SetActive(false);
            return;
        }

        int dotIndex = 0;

        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemy == null) continue;
            if (dotIndex >= maxDots) break;

            Vector3 screenPos = cam.WorldToScreenPoint(enemy.transform.position + Vector3.up * 2f);

            if (screenPos.z > 0)
            {
                dots[dotIndex].gameObject.SetActive(true);
                dots[dotIndex].position = new Vector3(screenPos.x, screenPos.y, 0);
                dotIndex++;
            }
        }

        for (int i = dotIndex; i < maxDots; i++)
        {
            dots[i].gameObject.SetActive(false);
        }
    }
}
