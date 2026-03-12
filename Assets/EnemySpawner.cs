using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnRadius = 15f;
    public float minDistanceFromPlayer = 5f;
    public float spawnTime = 4f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnTime);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
            return;

        Vector2 random2D = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPos = new Vector3(
            player.position.x + random2D.x,
            0.5f,
            player.position.z + random2D.y
        );

        float distance = Vector3.Distance(spawnPos, player.position);

        if (distance < minDistanceFromPlayer)
        {
            Vector3 dir = (spawnPos - player.position).normalized;
            spawnPos = player.position + dir * minDistanceFromPlayer;
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}