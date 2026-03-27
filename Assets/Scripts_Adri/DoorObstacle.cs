using UnityEngine;
using UnityEngine.UI;

public class DoorObstacle : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float _currentEscudo;
    float _damageEscudo;
    public bool destroyed = false;

    [Header("Referencias Modelos")]
    public GameObject[] doorPrefab;
    [SerializeField] Animator _animatorActual;
    [SerializeField] RandoSoundEffecs _randomSoundEffect;
    [SerializeField] ModelSwapper _modelSwapper;

    [Header("Abrir/Cerrar")]
    [SerializeField] Image _temporizadorPuerta;
    [SerializeField] CanvasGroup _canvasGroupTemporizadorPuerta;
    float _temporizadorAbrir = 3f;
    float _temporizadorCerrar = 1f;

    [Header("Contacto")]
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    Collider[] colliders = new Collider[5];
    private Collider _mainCollider;

    private void Start()
    {
        currentHealth = maxHealth;
        _mainCollider = GetComponent<Collider>();
        ActualizarAnimator();
    }

    private void Update()
    {
        Contacto();
    }

    public void TakeDamage(float damage)
    {
        if (destroyed) return;
        _damageEscudo = damage;
        if (_randomSoundEffect != null) _randomSoundEffect.PlayRandomAttackClip();
    }

    public void RepairDoor(float repairAmount)
    {
        if (destroyed) return;
        currentHealth += repairAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    void DestroyDoor()
    {
        destroyed = true;
        currentHealth = 0;
        if (_mainCollider != null) _mainCollider.enabled = false;
        if (_animatorActual != null) _animatorActual.gameObject.SetActive(false);
    }

    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);
        int cantidad = Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);

        bool playerEncontrado = false;

        for (int i = 0; i < cantidad; i++)
        {
            if (colliders[i] != null && colliders[i].TryGetComponent(out PlayerController player))
            {
                playerEncontrado = true;
                ManejarAccionesPlayer(player);
                break;
            }
        }

        if (!playerEncontrado) ResetBarra();
    }

    private void ManejarAccionesPlayer(PlayerController player)
    {
        if (player._reparacionCantidad > 0 && currentHealth < maxHealth)
        {
            float costo = 7f * Time.deltaTime;
            if (ShopManager.shopInstance.TrySpendMoney(costo))
            {
                RepairDoor(costo * 1.5f);
                player._animator.SetBool("Repairing", true);
            }
            else player._animator.SetBool("Repairing", false);
        }
        else player._animator.SetBool("Repairing", false);

        if (player._aguantandoLaPuerta && player.stamina > 0)
        {
            float desgastePasivo = 5f * Time.deltaTime;
            player.stamina -= (desgastePasivo + _damageEscudo);
            player.stamina = Mathf.Max(player.stamina, 0);
            _currentEscudo = player.stamina;
            if (player._animator != null) player._animator.SetBool("Holding", true);
            _damageEscudo = 0;
        }
        else
        {
            if (_damageEscudo > 0)
            {
                currentHealth -= _damageEscudo;
                if (currentHealth <= 0) DestroyDoor();
                _damageEscudo = 0;
            }
            _currentEscudo = 0;
            if (player._animator != null) player._animator.SetBool("Holding", false);
        }

        if (player.abrirPuerta)
        {
            bool estaAbierta = _animatorActual.GetBool("Abrir");
            if (!estaAbierta) AbrirLogica();
            else CerrarLogica();
        }
        else
        {
            ResetBarra();
        }
    }

    private void AbrirLogica()
    {
        _canvasGroupTemporizadorPuerta.alpha = 1;
        _temporizadorAbrir -= Time.deltaTime;
        _temporizadorPuerta.fillAmount = 1 - (_temporizadorAbrir / 3f);

        if (_temporizadorAbrir <= 0f)
        {
            _animatorActual.SetBool("Abrir", true);
            if (_mainCollider != null) _mainCollider.enabled = false;
            if (_randomSoundEffect != null) _randomSoundEffect.PlayRandomContructionClip();
            ResetBarra();
        }
    }

    private void CerrarLogica()
    {
        _canvasGroupTemporizadorPuerta.alpha = 1;
        _temporizadorCerrar -= Time.deltaTime;
        _temporizadorPuerta.fillAmount = 1 - _temporizadorCerrar;

        if (_temporizadorCerrar <= 0f)
        {
            _animatorActual.SetBool("Abrir", false);
            if (_mainCollider != null) _mainCollider.enabled = true;
            if (_randomSoundEffect != null) _randomSoundEffect.PlayRandomDieClip();
            ResetBarra();
        }
    }

    private void ResetBarra()
    {
        _temporizadorAbrir = 3f;
        _temporizadorCerrar = 1f;
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 0;
    }

    private void ActualizarAnimator()
    {
        _animatorActual = GetComponentInChildren<Animator>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, _tamanioCaja);
    }
}