using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TowerController : MonoBehaviour
{
    [SerializeField] public List<Transform> enemiesInRange = new List<Transform>();
    public GameObject[] towerModels;
    public Transform target;

    [Header("Atributos de la Torre")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public Transform firePoint;
    public GameObject bulletPrefab;

    void Update()
    {
        enemiesInRange.RemoveAll(e => e == null);
        // Se ordena la lista para que los enemigos con la propiedad de flying en true estén siempre los primeros.
        enemiesInRange = enemiesInRange
        .OrderByDescending(e => e.GetComponent<EnemyController>()?.data?.flying ?? false)
        .ToList();


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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        bulletController.bulletTarget = target;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Add(other.transform);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Remove(other.transform);
    }


    private int currentLevel = 0;

    public void Upgrade()
    {
        if (currentLevel < towerModels.Length - 1)
        {
            currentLevel++;
            UpdateModel();
        }
    }

    public void UpdateModel()
    {
        for (int i = 0; i < towerModels.Length; i++)
        {
            towerModels[i].SetActive(i == currentLevel);
        }
    }
}

