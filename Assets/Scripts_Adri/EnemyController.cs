using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy_SO datos;
    private int indicePunto = 0;
    private Transform[] camino;
    private DoorObstacle puertaActual;
    private float tiempoSiguienteAtaque;

    void Start()
    {
    }

    void Update()
    {
        if (puertaActual != null)
        {
            AtacarPuerta();
        }
        else
        {
            Mover();
            DetectarPuerta();
        }
    }

    void Mover()
    {
        if (indicePunto >= camino.Length) return;

        Vector3 destino = camino[indicePunto].position;
        transform.position = Vector3.MoveTowards(transform.position, destino, datos.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.2f)
        {
            indicePunto++;
        }
    }

    void DetectarPuerta()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            DoorObstacle obs = hit.collider.GetComponent<DoorObstacle>();
            if (obs != null)
            {
                puertaActual = obs;
            }
        }
    }

    void AtacarPuerta()
    {
        if (Time.time >= tiempoSiguienteAtaque)
        {
            puertaActual.TakeDamage(datos.damage);
            tiempoSiguienteAtaque = Time.time + datos.attackRate;
        }
    }

}
