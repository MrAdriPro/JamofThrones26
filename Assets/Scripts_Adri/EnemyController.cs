using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy_SO data;
    public float rotationSpeed = 720f; 

    private int indexPoint = 0;
    private Transform[] path;
    private DoorObstacle actualDoor;
    private float timeNextAttack;

    void Start()
    {
        path = PathManager.instance.pathPoints;
    }
    private void OnValidate()
    {
        
    }

    void Update()
    {
        if (actualDoor != null)
        {
            AttackDoor();
        }
        else
        {
            Move();
            DettectDoor();
        }
    }

    void Move()
    {
        if (indexPoint >= path.Length) return;

        Vector3 destine = path[indexPoint].position;
        Vector3 direction = destine - transform.position;

        Vector3 lookDirection = (data != null && data.flier) ? direction : new Vector3(direction.x, 0f, direction.z);

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, destine, data.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destine) < 0.4f)
        {
            indexPoint++;
        }
    }

    void DettectDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, data.attackRange))
        {
            DoorObstacle obs = hit.collider.GetComponent<DoorObstacle>();
            if (obs != null && !obs.destroyed)
            {
                actualDoor = obs;
            }
        }
    }

    void AttackDoor()
    {
        if (actualDoor == null) return;

        if (actualDoor.destroyed)
        {
            actualDoor = null;
            return;
        }

        if (Time.time >= timeNextAttack)
        {
            actualDoor.TakeDamage(data.damage);
            timeNextAttack = Time.time + data.attackRate;

            if (actualDoor.destroyed)
            {
                actualDoor = null;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (data != null)
        {
            Gizmos.DrawRay(transform.position, transform.forward * data.attackRange);
        }
    }

}
