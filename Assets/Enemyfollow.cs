using UnityEngine;
using UnityEngine.AI;

public class Enemyfollow : MonoBehaviour
{
    private Transform player;
    public int damage = 10;
    public float attackDistance = 1.5f;
    public float attackRate = 1f;

    private float nextAttackTime;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;

            if (Time.time >= nextAttackTime)
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();

                if (ph != null)
                    ph.TakeDamage(damage);

                nextAttackTime = Time.time + attackRate;
            }
        }
    }
}