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
    Collider[] colliders = new Collider[1];

    private void Start()
    {
        currentHealth = maxHealth;
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
        doorPrefab.SetActive(false);
        GetComponent<Collider>().enabled = false;
    }

    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);
        int cantidad = Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);

        if (cantidad == 0)
        {
            currentEscudo = 0;
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            Collider other = colliders[i];
            if (other == null) continue;

            if (other.TryGetComponent(out PlayerController playerController))
            {
                float vidaPorMoneda = 1.5f;
                float monedasPorSegundo = 7f;
                float costoEsteFrame = monedasPorSegundo * Time.deltaTime;

                if (playerController._reparacionCantidad > 0 && currentHealth < maxHealth)
                {
                    if (ShopManager.shopInstance.TrySpendMoney(costoEsteFrame))
                    {
                        RepairDoor(costoEsteFrame * vidaPorMoneda);
                        playerController._animator.SetBool("Repairing", true);
                    }
                    else playerController._animator.SetBool("Repairing", false);
                }
                else playerController._animator.SetBool("Repairing", false);

                ActivarScudo(playerController);
                AbrirPuerta(playerController);
                CerrarPuerta(playerController);
            }
        }
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

    private void AbrirPuerta(PlayerController _playerController)
    {
        if (_animator.GetBool("Abrir")) return;
        if (_playerController.abrirPuerta && _temporizadorAbrir > 0f)
        {
            _canvasGroupTemporizadorPuerta.alpha = 1;
            _temporizadorAbrir -= Time.deltaTime;
            _temporizadorPuerta.fillAmount = 1 - (_temporizadorAbrir / 3f);
        }
        else if (_temporizadorAbrir <= 0f)
        {
            _canvasGroupTemporizadorPuerta.alpha = 0;
            _animator.SetBool("Abrir", true);
            _temporizadorAbrir = 3f;
        }
        else
        {
            _temporizadorAbrir = 3f;
            _canvasGroupTemporizadorPuerta.alpha = 0;
        }
    }

    private void CerrarPuerta(PlayerController _playerController)
    {
        if (!_animator.GetBool("Abrir")) return;
        if (_playerController.abrirPuerta && _temporizadorCerrar > 0f)
        {
            _canvasGroupTemporizadorPuerta.alpha = 1;
            _temporizadorCerrar -= Time.deltaTime;
            _temporizadorPuerta.fillAmount = 1 - _temporizadorCerrar;
        }
        else if (_temporizadorCerrar <= 0f)
        {
            _canvasGroupTemporizadorPuerta.alpha = 0;
            _animator.SetBool("Abrir", false);
            _temporizadorCerrar = 1f;
        }
        else
        {
            _temporizadorCerrar = 1f;
            _canvasGroupTemporizadorPuerta.alpha = 0;
        }
    }
}