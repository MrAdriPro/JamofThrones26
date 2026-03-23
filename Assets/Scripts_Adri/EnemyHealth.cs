using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Enemy_SO data;
    float currentHealth;

    void Awake()
    {
        currentHealth = data.health;
    }
    private void Start()
    {
        
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;
        currentHealth -= amount;
        print("Enemy took damage, current health: " + currentHealth);
        if (currentHealth <= 0f) Die();
    }

    void Die()
    {

        Destroy(gameObject);
    }
}