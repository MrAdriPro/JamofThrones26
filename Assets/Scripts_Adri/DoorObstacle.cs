using UnityEngine;

public class DoorObstacle : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool destroyed = false;
    public GameObject doorPrefab;
    [Header("Contacto")]
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        Contacto();
        
    }
    void OnDrawGizmos()
    {
        //Gizmos para la caja de contacto
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
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

    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);

        Collider[] colliders = new Collider[1];

        Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);

        foreach (var other in colliders)
        {
            if (other == null) return;
            if ((_interacteables & (1 << other.gameObject.layer)) != 0)
            {
                if(other.TryGetComponent(out PlayerController playerController))
                {
                    
                    RepairDoor(playerController._reparacionCantidad);
                    
                }
                
            }

        }
    }
}







