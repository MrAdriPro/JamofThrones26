using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AreaDamageTower : MonoBehaviour
{
    [SerializeField] public List<Transform> enemiesInRange = new List<Transform>();

    [Tooltip("Index 0 = base model, index 1 = upgrade 1, etc. All must be children of this GameObject.")]
    public GameObject[] towerModels;

    public Transform target;
    [SerializeField] RandoSoundEffecs _randomSoundEffect;

    [Header("Atributos de la Torre")]
    public float towerDamage;
    public float fireRate = 1f;
    private float fireCountdown = 0f;


    void Start()
    {
        UpdateTowerModel();
    }

    public void ApplyUpgradeLevel(int level)
    {
        if (towerModels == null || towerModels.Length == 0)
        {
            Debug.LogWarning($"[AreaDamageTower] No towerModels assigned on {gameObject.name}.");
            return;
        }

        int clamped = Mathf.Clamp(level, 0, towerModels.Length - 1);

        for (int i = 0; i < towerModels.Length; i++)
        {
            if (towerModels[i] != null)
                towerModels[i].SetActive(i == clamped);
        }
    }

    void Update()
    {

        Transform body = transform.Find("Body");
    
        towerModels = new GameObject[body.childCount];
    
        for (int i = 0; i < body.childCount; i++)
        {
            towerModels[i] = body.GetChild(i).gameObject;
        }        

        UpdateTowerModel();

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
                Attack();
                _randomSoundEffect.PlayRandomAttackClip();
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

    void Attack()
    {
        foreach (Transform enemy in enemiesInRange.ToList())
        {
            if (enemy == null) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth == null) continue;

            enemyHealth.TakeDamage(towerDamage * ShopManager.shopInstance.tekLevel + 1);
            

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

    void UpdateTowerModel()
    {
        int level = ShopManager.shopInstance.tekLevel;
        for (int i = 0; i < towerModels.Length; i++)
        {
            towerModels[i].SetActive(i == level);
        }
    }

}