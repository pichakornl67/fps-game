using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();   // CALL THE FUNCTION
        }
    }

    void Die()
    {
        ScoreManager.instance.AddScore(1);
        Destroy(gameObject);
    }
}