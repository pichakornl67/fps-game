using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float bulletSpeed = 40f;

    public GameObject muzzleFlash;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        StartCoroutine(Flash());

        GameObject newBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    IEnumerator Flash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }
}