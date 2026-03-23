using UnityEngine;
using System.Collections.Generic;

public class TowerController : MonoBehaviour
{
    [SerializeField] public List<Transform> enemiesInRange = new List<Transform>();
    public Transform target;

    [Header("Atributos de la Torre")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public Transform firePoint;

    void Update()
    {
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count > 0) target = enemiesInRange[0];
        else target = null;

        if (target != null)
        {
            LockOnTarget();
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        var entity = PoolManager.Instance.Pull("Bullet", firePoint.position, firePoint.rotation);
        if (entity is BulletController bullet)
        {
            bullet.objetivo = target;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Add(other.transform);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Remove(other.transform);
    }
}