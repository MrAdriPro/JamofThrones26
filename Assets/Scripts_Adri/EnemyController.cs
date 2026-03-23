using UnityEngine;

public class EnemyController : PoolEntity
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
        if(!IsActive) return;
        
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

    // This method handles the movement of the enemy along the path. It calculates the direction to the next point, rotates the enemy towards that direction, and moves it forward. If the enemy is close enough to the next point, it advances to the next point in the path.
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
    // This method uses a raycast to detect if there is a door obstacle in front of the enemy within its attack range. If it detects a door, it checks if the door is not already destroyed and sets it as the current target for attacking.
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
    //Enemy died Fuction
 #region Pool Entity
    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Deactivate()
    {
        base.Deactivate();
    }
    #endregion
}
