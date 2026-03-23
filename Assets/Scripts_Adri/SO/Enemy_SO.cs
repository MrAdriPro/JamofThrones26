using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "New Enemy/Enemy_SO")]
public class Enemy_SO : ScriptableObject
{
    [SerializeField] private string enemyName;
    public GameObject enemyPrefab;
    public float health;
    public float speed;
    public float damage;
    public float attackRate;
    public bool flier;
    public float attackRange;


}
