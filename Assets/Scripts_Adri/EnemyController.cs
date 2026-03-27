using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 720f;

    [Header("Wander & movement")]
    public float wanderRadius = 1f;
    public float arrivalThreshold = 0.4f;
    public float swayAmplitude = 0.2f;
    public float swayFrequency = 1f;

    private int indexPoint = 0;
    private Transform[] path;
    private DoorObstacle actualDoor;
    public Animator _animator;
    [SerializeField] float timeNextAttack;
    private Vector3 currentTargetPos;
    private float swayTimer;

    [SerializeField] RandoSoundEffecs _randoSoundEffecs;
    public Enemy_SO data;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        path = PathManager.instance.pathPoints;
        if (path != null && path.Length > 0)
        {
            indexPoint = 0;
            SetTargetForCurrentIndex();
        }
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if (wanderRadius < 0f) wanderRadius = 0f;
        if (arrivalThreshold < 0.01f) arrivalThreshold = 0.01f;
        if (swayAmplitude < 0f) swayAmplitude = 0f;
        if (swayFrequency < 0f) swayFrequency = 0f;
    }

    void Update()
    {
        if (!this.enabled) return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool estaEnAnimacionDeAtaque = stateInfo.IsName("Attack") || _animator.GetBool("Attacking");

        if (actualDoor != null)
        {
            if (actualDoor.estaAbierta || actualDoor.destroyed)
            {
                actualDoor = null;
                _animator.SetBool("Attacking", false);
            }
            else
            {
                Vector3 dirToDoor = actualDoor.transform.position - transform.position;
                ActualizarGiroSprite(dirToDoor.x, estaEnAnimacionDeAtaque);
                AttackDoor();
            }
        }
        else
        {
            DettectDoor();

            if (actualDoor == null)
            {
                _animator.SetBool("Attacking", false);
                Vector3 direction = currentTargetPos - transform.position;
                ActualizarGiroSprite(direction.x, false);
                Move();
            }
            else
            {
                _animator.SetBool("Attacking", true);
            }
        }
    }

    void ActualizarGiroSprite(float direccionX, bool atacando)
    {
        if (Mathf.Abs(direccionX) < 0.1f) return;
        bool mirarIzquierda = direccionX < 0f;
        if (data.attackInverted && atacando) _spriteRenderer.flipX = !mirarIzquierda;
        else _spriteRenderer.flipX = mirarIzquierda;
    }

    void SetTargetForCurrentIndex()
    {
        if (path == null || indexPoint >= path.Length) return;
        Vector3 basePos = path[indexPoint].position;
        Vector2 rnd = Random.insideUnitCircle * wanderRadius;
        Vector3 offset = new Vector3(rnd.x, 0f, rnd.y);
        currentTargetPos = basePos + offset;
    }

    void Move()
    {
        if (path == null || indexPoint >= path.Length) return;

        Vector3 destine = currentTargetPos;
        Vector3 direction = destine - transform.position;

        if (Mathf.Abs(direction.normalized.x) > 0.10f)
            _spriteRenderer.flipX = direction.x < 0f;

        Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float moveSpeed = (data != null) ? data.speed : 0f;
        transform.position = Vector3.MoveTowards(transform.position, destine, moveSpeed * Time.deltaTime);

        swayTimer += Time.deltaTime * swayFrequency;
        Vector3 lateral = transform.right * Mathf.Sin(swayTimer) * swayAmplitude;
        if (data == null || !data.flying) lateral.y = 0f;

        transform.position += lateral * Time.deltaTime;

        if (Vector3.Distance(transform.position, destine) < arrivalThreshold)
        {
            indexPoint++;
            if (indexPoint < path.Length) SetTargetForCurrentIndex();
        }
    }

    void DettectDoor()
    {
        if (data == null) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, data.attackRange))
        {
            DoorObstacle obs = hit.collider.GetComponent<DoorObstacle>();
            if (obs != null && !obs.destroyed && !obs.estaAbierta)
            {
                actualDoor = obs;
            }
        }
    }

    void AttackDoor()
    {
        if (actualDoor == null || actualDoor.destroyed || actualDoor.estaAbierta)
        {
            actualDoor = null;
            _animator.SetBool("Attacking", false);
            return;
        }

        _animator.SetBool("Attacking", true);

        if (Time.time >= timeNextAttack)
        {
            _randoSoundEffecs.PlayRandomAttackClip();
            actualDoor.TakeDamage(data.damage);
            timeNextAttack = Time.time + data.attackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (data != null) Gizmos.DrawRay(transform.position, transform.forward * data.attackRange);

        if (path != null && indexPoint < path.Length)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(currentTargetPos, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(path[indexPoint].position, wanderRadius);
        }
    }
}