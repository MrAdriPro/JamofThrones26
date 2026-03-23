using UnityEngine;

public class DoorObstacle : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool destroyed = false;
    public GameObject doorPrefab;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            RepairDoor(2);
        }
    }
    //this is for the enemies to call when they attack the door
    public void TakeDamage(float damage)
    {
        if (destroyed) return;
        currentHealth -= damage;
        print("Door took damage: " + damage + ", current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            DestroyDoor();
        }
    }
    //this is for the player to call when they want to repair the door
    public void RepairDoor(float repairAmount)
    {
        if (destroyed) return;
        currentHealth += repairAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        print("Door repaired: " + repairAmount + ", current health: " + currentHealth);
    }
    //this is called when the door is destroyed
    void DestroyDoor()
    {
        destroyed = true;
        currentHealth = 0;
        doorPrefab.SetActive(false);
        print("Door destroyed");
        GetComponent<Collider>().enabled = false;
        
    }
}
