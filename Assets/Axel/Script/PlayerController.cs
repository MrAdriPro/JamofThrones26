using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] CharacterController _cC;
    [SerializeField] Camera _mainCamera;
    [SerializeField] Transform _playerTransform;
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

    [Header("Mecanicas")]
    public float stamina = 100;
    public bool _aguantandoLaPuerta;
    public bool abrirPuerta = false;
    public float _reparacionCantidad = 0;
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    bool _onPause = false;
    [SerializeField] private float _costeDineroPerSecond;

    public Animator _animator;
    #endregion

    #region Funciones Unity
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
    #endregion

    #region Input System
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
        if (context.performed)
        {
            abrirPuerta = true;
        }
        else if (context.canceled)
        {
            abrirPuerta = false;
        }
    }
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onPause = !_onPause;
            if (_onPause)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    #endregion

    #region Funciones de Logica
    private void GroundCheck()
    {
        Collider[] colliderBuffer = new Collider[1];
        Physics.OverlapBoxNonAlloc(transform.position, _groundCheckSize / 2f, colliderBuffer, transform.rotation, _groundLayer);
        _grounded = colliderBuffer[0] != null;
    }

    private void Movimiento()
    {
        Vector3 direccion = new Vector3(_horizontal, 0, _vertical);

        if (!_grounded) vectorGravity = Vector3.down * 9.81f;
        else vectorGravity = Vector3.zero;

        _cC.Move((direccion + vectorGravity) * _velocidadMovimiento * Time.deltaTime);

        if (_animator != null)
        {
            bool isRepairing = _animator.GetBool("Repairing");

            if (isRepairing)
            {
                _animator.SetFloat("VerticalMove", 0);
            }
            else
            {
                float verticalLimpio = Mathf.Abs(_vertical) < 0.1f ? 0f : _vertical;

                if (Mathf.Abs(_horizontal) > 0.1f && Mathf.Abs(_vertical) < 0.1f)
                {
                    _animator.SetFloat("VerticalMove", -1f);
                }
                else
                {
                    _animator.SetFloat("VerticalMove", verticalLimpio);
                }
            }


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
        stamina += Time.deltaTime;
    }

    public void RefreshAnimator(Animator newAnimator)
    {
        _animator = newAnimator;
        if (_animator != null) _animator.SetFloat("VerticalMove", 0);
    }
    #endregion
}