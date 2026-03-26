using UnityEngine;
using UnityEngine.UI;

public class DoorObstacle : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float currentEscudo;
    float _damageEscudo;
    public bool destroyed = false;
    public GameObject doorPrefab;
    [SerializeField] Animator _animator;

    [Header("Configuración Escudo")]
    [SerializeField] float _consumoPasivoEstamina = 5f;
    [SerializeField] float _multiplicadorDanoImpacto = 0.3f;

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
    private Collider _doorCollider;

    private void Start()
    {
        currentHealth = maxHealth;
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 0;
        _doorCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        Contacto();
    }

    public void TakeDamage(float damage)
    {
        _damageEscudo = damage;
        if (destroyed || currentEscudo > 0) return;
        currentHealth -= damage;
        if (currentHealth <= 0) DestroyDoor();
    }

    public void RepairDoor(float repairAmount)
    {
        if (destroyed) return;
        currentHealth += repairAmount;
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
    }

    void DestroyDoor()
    {
        destroyed = true;
        currentHealth = 0;
        if (doorPrefab != null) doorPrefab.SetActive(false);
        if (_doorCollider != null) _doorCollider.enabled = false;
    }

    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);
        int cantidad = Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2f, colliders, transform.rotation, _interacteables);

        bool playerEncontrado = false;

        for (int i = 0; i < cantidad; i++)
        {
            if (colliders[i] != null && colliders[i].TryGetComponent(out PlayerController playerController))
            {
                playerEncontrado = true;
                ManejarAcciones(playerController);
                break;
            }
        }

        if (!playerEncontrado)
        {
            ResetBarra();
        }
    }

    private void ManejarAcciones(PlayerController playerController)
    {
        ManejarReparacion(playerController);
        ActivarScudo(playerController);

        if (playerController.abrirPuerta)
        {
            bool estaAbierta = _animator.GetBool("Abrir");
            if (!estaAbierta) AbrirPuertaLogica();
            else CerrarPuertaLogica();
        }
        else
        {
            ResetBarra();
        }
    }

    private void ManejarReparacion(PlayerController playerController)
    {
        if (playerController._reparacionCantidad > 0 && currentHealth < maxHealth)
        {
            float vidaPorMoneda = 1.5f;
            float monedasPorSegundo = 7f;
            float costoEsteFrame = monedasPorSegundo * Time.deltaTime;

            if (ShopManager.shopInstance.TrySpendMoney(costoEsteFrame))
            {
                RepairDoor(costoEsteFrame * vidaPorMoneda);
                playerController._animator.SetBool("Repairing", true);
            }
            else playerController._animator.SetBool("Repairing", false);
        }
        else playerController._animator.SetBool("Repairing", false);
    }

    private void ActivarScudo(PlayerController _playerController)
    {
        if (!_playerController._aguantandoLaPuerta || _playerController.stamina <= 0)
        {
            currentEscudo = 0;
            if (_playerController._animator != null) _playerController._animator.SetBool("Holding", false);
            return;
        }

        if (_playerController._animator != null) _playerController._animator.SetBool("Holding", true);

        float consumoEsteFrame = _consumoPasivoEstamina * Time.deltaTime;
        float danoPorImpacto = _damageEscudo * _multiplicadorDanoImpacto;

        _playerController.stamina -= (consumoEsteFrame + danoPorImpacto);
        if (_playerController.stamina < 0) _playerController.stamina = 0;

        currentEscudo = _playerController.stamina;
        _damageEscudo = 0;
    }

    private void AbrirPuertaLogica()
    {
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 1;

        _temporizadorAbrir -= Time.deltaTime;
        float progreso = 1f - (_temporizadorAbrir / 3f);
        if (_temporizadorPuerta != null) _temporizadorPuerta.fillAmount = Mathf.Clamp01(progreso);

        if (_temporizadorAbrir <= 0f)
        {
            _animator.SetBool("Abrir", true);
            if (_doorCollider != null) _doorCollider.enabled = false;
            ResetBarra();
        }
    }

    private void CerrarPuertaLogica()
    {
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 1;

        _temporizadorCerrar -= Time.deltaTime;
        float progreso = 1f - _temporizadorCerrar;
        if (_temporizadorPuerta != null) _temporizadorPuerta.fillAmount = Mathf.Clamp01(progreso);

        if (_temporizadorCerrar <= 0f)
        {
            _animator.SetBool("Abrir", false);
            if (_doorCollider != null) _doorCollider.enabled = true;
            ResetBarra();
        }
    }

    private void ResetBarra()
    {
        _temporizadorAbrir = 3f;
        _temporizadorCerrar = 1f;
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 0;
        if (_temporizadorPuerta != null) _temporizadorPuerta.fillAmount = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, _tamanioCaja);
    }
}