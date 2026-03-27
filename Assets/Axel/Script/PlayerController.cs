using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] CharacterController _cC;
    [SerializeField] Camera _mainCamera;
    [SerializeField] RandoSoundEffecs _randomSoundEffect;

    [Header("Grounded")]
    [SerializeField] Vector3 _groundCheckSize;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] bool _grounded;

    [Header("Movimiento")]
    [SerializeField] float _velocidadMovimiento;
    float _horizontal;
    float _vertical;
    Vector3 vectorGravity;
    Vector2 _posicionRaton;

    [Header("Mecánicas")]
    public float stamina = 100;
    public bool _aguantandoLaPuerta;
    public bool abrirPuerta = false;
    public float _reparacionCantidad = 0;
    bool _onPause = false;

    public Animator _animator;

    void Start()
    {
        _mainCamera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Movimiento();
        RecuperacionEstamina();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _horizontal = input.x;
            _vertical = input.y;
        }
        else if (context.canceled)
        {
            _horizontal = 0;
            _vertical = 0;
        }
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        _posicionRaton = context.ReadValue<Vector2>();
    }

    public void OnRepair(InputAction.CallbackContext context)
    {
        if (_aguantandoLaPuerta) return;

        if (context.performed)
        {
            _reparacionCantidad = 2f;
            if (_randomSoundEffect != null) _randomSoundEffect.PlayRandomContructionClip();
        }
        else if (context.canceled)
        {
            _reparacionCantidad = 0f;
        }
    }

    public void OnHoldingDoor(InputAction.CallbackContext context)
    {
        if (context.performed) _aguantandoLaPuerta = true;
        else if (context.canceled) _aguantandoLaPuerta = false;
    }

    public void OnOpeninDoor(InputAction.CallbackContext context)
    {
        if (context.performed) abrirPuerta = true;
        else if (context.canceled) abrirPuerta = false;
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    _onPause = !_onPause;
        //    Time.timeScale = _onPause ? 0f : 1f;
        //}
    }

    private void GroundCheck()
    {
        Collider[] colliderBuffer = new Collider[1];
        Physics.OverlapBoxNonAlloc(transform.position, _groundCheckSize / 2f, colliderBuffer, transform.rotation, _groundLayer);
        _grounded = colliderBuffer[0] != null;
    }

    private void Movimiento()
    {
        bool isRepairing = _animator != null && _animator.GetBool("Repairing");
        bool isHolding = _animator != null && _animator.GetBool("Holding");

        if (isRepairing || isHolding || abrirPuerta)
        {
            _horizontal = 0;
            _vertical = 0;
            if (_animator != null) _animator.SetFloat("VerticalMove", 0);
        }

        Vector3 direccion = new Vector3(_horizontal, 0, _vertical);
        vectorGravity = _grounded ? Vector3.zero : Vector3.down * 9.81f;

        _cC.Move((direccion + vectorGravity) * _velocidadMovimiento * Time.deltaTime);

        if (_animator != null && !isRepairing && !isHolding && !abrirPuerta)
        {
            float verticalLimpio = Mathf.Abs(_vertical) < 0.1f ? 0f : _vertical;
            if (Mathf.Abs(_horizontal) > 0.1f && Mathf.Abs(_vertical) < 0.1f) 
                _animator.SetFloat("VerticalMove", -1f);
            else 
                _animator.SetFloat("VerticalMove", verticalLimpio);

            SpriteRenderer sR = _animator.GetComponent<SpriteRenderer>();
            if (sR != null)
            {
                if (_horizontal < -0.1f) sR.flipX = true;
                else if (_horizontal > 0.1f) sR.flipX = false;
            }
        }
    }

    private void RecuperacionEstamina()
    {
        if (stamina >= 100 || _aguantandoLaPuerta) return;
        stamina += Time.deltaTime * 5f;
    }

    public void RefreshAnimator(Animator newAnimator)
    {
        _animator = newAnimator;
        if (_animator != null) _animator.SetFloat("VerticalMove", 0);
    }
}