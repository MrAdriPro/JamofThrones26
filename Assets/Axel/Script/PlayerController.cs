using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Camera _mainCamera;
    [SerializeField] CharacterController _cC;

    [Header("Grounded")]
    [SerializeField] Vector3 _groundCheckSize;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] bool _grounded;

    [Header("Movimiento")]
    [SerializeField] float _velocidadMovimiento;
    float _horizontal;
    float _vertical;
    Vector3 vectorGravity;

    [Header("Rotacion")]
    [SerializeField] float _rotMultiplicador;
    Vector2 _posicionRaton;
    Vector3 _objetivoRaton;
    #endregion




    #region Funciones Unity
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();

        Rotacion();
    }
    void OnDrawGizmos()
    {
        //Cambiamos el color del Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _groundCheckSize);
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
    #endregion




    #region Funciones
    private void GroundCheck()
    {
        //Solo vamos a comprobar si es mayor que 0, asi que no necesitamos mas capacida de buffer
        Collider[] colliderBuffer = new Collider[1];
        //Comprobamos si hay contacto con el suelo
        Physics.OverlapBoxNonAlloc(transform.position,
                                    _groundCheckSize / 2f,
                                    colliderBuffer,
                                    transform.rotation,
                                    _groundLayer);
        //Actualitzamos el estado de _grounded
        _grounded = colliderBuffer[0] != null;
    }
    private void Movimiento()
    {
        //Calcula la direccion a la que se esta dirigiendo el player
        Vector3 direccion = new Vector3(_horizontal, 0, _vertical);
        if (!_grounded)
        {
            vectorGravity = Vector3.down * 9.81f;
        }
        //Aplicamos la direccion multiplicada por la velocidad a la que no movemos en el tiempo
        _cC.Move((direccion + vectorGravity) * _velocidadMovimiento * Time.deltaTime);
    }
    private void Rotacion()
    {
        Plane playerPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        Ray ray = _mainCamera.ScreenPointToRay(_posicionRaton);

        if (playerPlane.Raycast(ray, out float hitDist))
        {
            _objetivoRaton = ray.GetPoint(hitDist);
        }

        Vector3 direccionRaton = _objetivoRaton - transform.position;
        direccionRaton.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(direccionRaton);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * _rotMultiplicador);
    }
    #endregion
}
