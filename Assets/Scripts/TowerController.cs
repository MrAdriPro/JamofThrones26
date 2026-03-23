using UnityEngine;
using System.Collections.Generic;

public class TowerController : MonoBehaviour
{
    [SerializeField] public List<Transform> enemiesInRange = new List<Transform>();
    public Transform target;
    
    [Header("Atributos de la Torre")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        enemiesInRange.RemoveAll(e => e == null);

        // Actualizar el target al primero de la lista
        if (enemiesInRange.Count > 0)
        {
            target = enemiesInRange[0];
        }
        else
        {
            target = null;
        }

        // Si tenemos un target, apuntar y disparar
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
        // Rotación suave hacia el enemigo
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); 
        BulletController scriptBala = bullet.GetComponent<BulletController>();

        scriptBala.objetivo = target;
    }

    // --- DETECCIÓN ---
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Añadir al final de la lista
            enemiesInRange.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Quitar de la lista cuando sale del rango
            enemiesInRange.Remove(other.transform);
        }
    }
}