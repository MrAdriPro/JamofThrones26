using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTowerController : MonoBehaviour
{
    [Header("Tower Settings")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public Transform firePoint;

    [Header("Target Settings")]
    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform target;
    public SphereCollider towerRange;


    void Update()
    {
        // Se quitan de la lista los enemigos destruidos
        // enemiesInRange.RemoveAll(e => e == null);

        // Se asigna como target el primer enemigo que ha entrado en el rango de la torre
        if(enemiesInRange.Count > 0)
        {
            target = enemiesInRange[0];
        }

        if(target != null)
        {
            LockTarget();
            Shoot();
        }
    }

    void LockTarget()
    {
        // Función para que la torre mire al player;
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = lookRotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
        
    }


    // Detección de enemigos
    void OnTriggerEnter(Collider other)
    {
        // Añadir el enemigo que entra en el rango de la torre a la lista
        if(other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Quitar el enemigo que sale del rango de la torre de la lista
        if(other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }
}
