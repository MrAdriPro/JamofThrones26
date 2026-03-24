using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public Transform bulletTarget;
    public float bulletSpeed;

    void Start()
    {
        enemyHealth = bulletTarget.GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (bulletTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, bulletTarget.position, bulletSpeed * Time.deltaTime);

        transform.LookAt(bulletTarget.position);

        if (Vector3.Distance(transform.position, bulletTarget.position) < 0.2f)
        {
            enemyHealth.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}