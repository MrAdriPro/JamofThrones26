using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private Vector3 _dropDir;
    public float maxDistance = 1f;
    public float minDistance = 0.5f;

    public int moneyValue = 4;

    public float dropForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Randomize direction
        float dirX = Random.Range(minDistance, maxDistance);
        float dirZ = Random.Range(minDistance, maxDistance);

        _dropDir = new Vector3(dirX, 1, dirZ);


        // drop item at direction
        _rb.AddForce(_dropDir * dropForce ,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // print("Nigga wat");
        // if(collision.gameObject.CompareTag("Player"))
        // {
        //     // ShopManager.shopInstance.GetCoin(moneyValue);
        //     Destroy(gameObject);
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Nigga wat");
        if(other.CompareTag("Player"))
        {
            ShopManager.shopInstance.GetCoin(moneyValue);
            Destroy(gameObject);
        }
    }
}
