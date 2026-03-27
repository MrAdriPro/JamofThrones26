using UnityEngine;
using UnityEngine.UI;

public class DoorObstacle : MonoBehaviour
{
    #region Variables
    public float maxHealth = 100f;
    public float currentHealth;
    public float _currentEscudo;
    float _damageEscudo;
    public bool destroyed = false;
    public bool estaAbierta = false;

    [Header("Referencias Modelos")]
    public GameObject[] doorPrefab;
    [SerializeField] Animator _animatorActual;
    [SerializeField] RandoSoundEffecs _randomSoundEffect;
    [SerializeField] ModelSwapper _modelSwapper;
    int _ultimoIndiceModelo = -1;

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
    private AudioSource _audioSourceReparar;
    #endregion

    private void Start()
    {
        currentHealth = maxHealth;
        _mainCollider = GetComponent<Collider>();
        if (_randomSoundEffect != null)
            _audioSourceReparar = _randomSoundEffect.GetComponent<AudioSource>();
        ActualizarPuertaVisual();
    }

    private void Update()
    {
        if (_modelSwapper != null && _modelSwapper.currentTek != _ultimoIndiceModelo)
        {
            ActualizarPuertaVisual();
        }
        Contacto();
    }

    private void ActualizarPuertaVisual()
    {
        _ultimoIndiceModelo = _modelSwapper.currentTek;
        for (int i = 0; i < doorPrefab.Length; i++)
        {
            bool esActivo = (i == _ultimoIndiceModelo);
            doorPrefab[i].SetActive(esActivo);
            if (esActivo) _animatorActual = doorPrefab[i].GetComponent<Animator>();
        }
    }

    public void TakeDamage(float damage)
    {
        if (destroyed || estaAbierta) return;
        _damageEscudo = damage;
        if (_randomSoundEffect != null) _randomSoundEffect.PlayRandomAttackClip();

        if (_currentEscudo <= 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) DestroyDoor();
        }
    }

    public void RepairDoor(float repairAmount)
    {
        if (destroyed || estaAbierta) return;
        currentHealth += repairAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    void DestroyDoor()
    {
        destroyed = true;
        estaAbierta = true;
        currentHealth = 0;
        DetenerSonidoReparacion();
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

        if (!playerEncontrado)
        {
            ResetBarra();
            DetenerSonidoReparacion();
        }
    }

    private void ManejarAccionesPlayer(PlayerController player)
    {
        if (player._reparacionCantidad > 0 && currentHealth < maxHealth && !estaAbierta)
        {
            float costo = 7f * Time.deltaTime;
            if (ShopManager.shopInstance.TrySpendMoney(costo))
            {
                RepairDoor(costo * 1.5f);
                player._animator.SetBool("Repairing", true);
                if (_audioSourceReparar != null && !_audioSourceReparar.isPlaying)
                    _randomSoundEffect.PlayRandomContructionClip();
            }
            else DetenerReparacionAnim(player);
        }
        else DetenerReparacionAnim(player);

        if (player._aguantandoLaPuerta && player.stamina > 0 && !estaAbierta)
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
            _currentEscudo = 0;
            if (player._animator != null) player._animator.SetBool("Holding", false);
        }

        if (player.abrirPuerta)
        {
            if (_animatorActual != null)
            {
                if (!_animatorActual.GetBool("Abrir")) AbrirPuertaLogica();
                else CerrarPuertaLogica();
            }
        }
        else ResetBarra();
    }

    private void DetenerReparacionAnim(PlayerController player)
    {
        if (player._animator != null) player._animator.SetBool("Repairing", false);
        DetenerSonidoReparacion();
    }

    private void DetenerSonidoReparacion()
    {
        if (_audioSourceReparar != null && _audioSourceReparar.isPlaying)
            _audioSourceReparar.Stop();
    }

    private void AbrirPuertaLogica()
    {
        _canvasGroupTemporizadorPuerta.alpha = 1;
        _temporizadorAbrir -= Time.deltaTime;
        _temporizadorPuerta.fillAmount = 1 - (_temporizadorAbrir / 3f);
        if (_temporizadorAbrir <= 0f)
        {
            estaAbierta = true;
            _animatorActual.SetBool("Abrir", true);
            if (_mainCollider != null) _mainCollider.enabled = false;
            _randomSoundEffect.PlayRandomContructionClip();
            ResetBarra();
        }
    }

    private void CerrarPuertaLogica()
    {
        _canvasGroupTemporizadorPuerta.alpha = 1;
        _temporizadorCerrar -= Time.deltaTime;
        _temporizadorPuerta.fillAmount = 1 - _temporizadorCerrar;
        if (_temporizadorCerrar <= 0f)
        {
            estaAbierta = false;
            _animatorActual.SetBool("Abrir", false);
            if (_mainCollider != null) _mainCollider.enabled = true;
            _randomSoundEffect.PlayRandomDieClip();
            ResetBarra();
        }
    }

    private void ResetBarra()
    {
        _temporizadorAbrir = 3f;
        _temporizadorCerrar = 1f;
        if (_canvasGroupTemporizadorPuerta != null) _canvasGroupTemporizadorPuerta.alpha = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}