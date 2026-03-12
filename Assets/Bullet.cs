using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 3f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(90f, 0f, 0f);
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemyhealth enemy = collision.gameObject.GetComponent<Enemyhealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}