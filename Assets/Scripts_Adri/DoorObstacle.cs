using UnityEngine;

public class DoorObstacle : MonoBehaviour
{
    #region Variables
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] float _currentEscudo;
    float _damageEscudo;
    public bool destroyed = false;
    public GameObject doorPrefab;
    [Header("Contacto")]
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    Collider[] colliders = new Collider[1];
    #endregion




    #region Funciones de Unity
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        Contacto();
        TakeDamage(0.001f);

    }
    void OnDrawGizmos()
    {
        //Gizmos para la caja de contacto
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
    #endregion




    #region Funciones
    //this is for the enemies to call when they attack the door
    public void TakeDamage(float damage)
    {
        _damageEscudo = damage;

        if (destroyed || _currentEscudo > 0) return;

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



        int cantidad = Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);


        if (cantidad == 0)
        {
            _currentEscudo = 0f;

            return;
        }


        for (int i = 0; i < cantidad; i++)
        {
            Collider other = colliders[i];
            if (other == null) continue;

            if (other.TryGetComponent(out PlayerController playerController))
            {
                RepairDoor(playerController._reparacionCantidad);
                ActivarScudo(playerController);
            }
            else
            {
                _currentEscudo = 0f;
            }
        }
    }



    private void ActivarScudo(PlayerController _playerController)
    {
        if (!_playerController._aguantandoLaPuerta)
        {
            _currentEscudo = 0;
            return;
        }
        _playerController.stamina -= _damageEscudo;
        _currentEscudo = _playerController.stamina;
    }
        
  
        
            


    #endregion
}







