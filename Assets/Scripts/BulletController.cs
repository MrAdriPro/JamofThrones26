using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform objetivo; 
    public float velocidad = 10f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);
    }
}
