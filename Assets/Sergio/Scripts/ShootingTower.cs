using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShootingTower : MonoBehaviour
{
    [SerializeField] public List<Transform> enemiesInRange = new List<Transform>();

    [Tooltip("Index 0 = base model, index 1 = upgrade 1, etc. All must be children of this GameObject.")]
    public GameObject[] towerModels;
    [SerializeField] RandoSoundEffecs _randomSoundEffects;
    public Transform target;

    [Header("Atributos de la Torre")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public Transform firePoint;
    public GameObject[] bulletPrefabs;

    private Animator _currentAnimator;
    private int _lastLevel = -1;

    void Start()
    {
        SetupTowerModels();
        UpdateTowerModel();
    }

    void SetupTowerModels()
    {
        Transform body = transform.Find("Body");
        if (body != null)
        {
            towerModels = new GameObject[body.childCount];
            for (int i = 0; i < body.childCount; i++)
            {
                towerModels[i] = body.GetChild(i).gameObject;
            }
        }
    }

    void Update()
    {
        if (ShopManager.shopInstance.tekLevel != _lastLevel)
        {
            UpdateTowerModel();
        }

        enemiesInRange.RemoveAll(e => e == null);

        enemiesInRange = enemiesInRange
            .OrderByDescending(e => e.GetComponent<EnemyController>()?.data?.flying ?? false)
            .ToList();

        target = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;

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
        if (_currentAnimator != null)
        {
            _currentAnimator.SetTrigger("Shoot");
        }

        GameObject bullet = Instantiate(bulletPrefabs[ShopManager.shopInstance.tekLevel], firePoint.position, firePoint.rotation);
        _randomSoundEffects.PlayRandomAttackClip();

        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
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

    void UpdateTowerModel()
    {
        _lastLevel = ShopManager.shopInstance.tekLevel;

        if (towerModels == null || towerModels.Length == 0) SetupTowerModels();

        for (int i = 0; i < towerModels.Length; i++)
        {
            bool isActive = (i == _lastLevel);
            towerModels[i].SetActive(isActive);

            if (isActive)
            {
                _currentAnimator = towerModels[i].GetComponent<Animator>();
            }
        }
    }
}