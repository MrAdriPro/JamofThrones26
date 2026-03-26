using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Enemy_SO data;
    [SerializeField] GameObject coinObject;
    float currentHealth;
    private EnemyController enemyController;
    private bool isDead;

    void Awake()
    {
        currentHealth = data.health;
        enemyController = GetComponent<EnemyController>();
    }
    private void Start()
    {
    }

    public void TakeDamage(float amount)
    {
        if(isDead) return;
        currentHealth -= amount;
        VisualEnemyFeedback enemyFeedback = GetComponentInChildren<VisualEnemyFeedback>();
        if (enemyFeedback != null) 
        { 
            enemyFeedback.PlayDamageFeedBack();
        }
        //feedbackenemigo
        print("Enemy took damage, current health: " + currentHealth);
        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        Instantiate(coinObject);
        if(enemyController != null) enemyController.enabled = false;
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;


        enemyController._animator.SetTrigger("isDead");
        Destroy(gameObject, 1.4f);
    }
}