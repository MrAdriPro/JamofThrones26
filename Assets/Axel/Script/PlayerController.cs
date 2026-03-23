using Unity.VisualScripting;
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
    [SerializeField] Transform _disparo;

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

    [Header("Contacto")]
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    [SerializeField] float _cantidadReparacion;
    bool _reparacion = false;
    #endregion




    #region Funciones Unity
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Contacto();
        Movimiento();

        Rotacion();
    }

    void OnDrawGizmos()
    {
        //Gizmos para el Groundcheck
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _groundCheckSize);

        //Gizmos para la caja de contacto
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
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
        if (context.performed)
        {
            _reparacion = true;
        }
        else if (context.canceled)
        {
            _reparacion = false;
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Disparo();
        }
    }
    #endregion




    #region Funciones
    private void GroundCheck()
    {
        //Solo vamos a comprobar si es mayor que 0, asi que no necesitamos mas capacida de buffer
        Collider[] colliderBuffer = new Collider[1];
        //Comprobamos si hay contacto con el suelo
        Physics.OverlapBoxNonAlloc(transform.position, _groundCheckSize / 2f, colliderBuffer, transform.rotation, _groundLayer);
        //Actualitzamos el estado de _grounded
        _grounded = colliderBuffer[0] != null;
    }


    private void Movimiento()
    {
        //Calcula la direccion a la que se esta dirigiendo el player
        Vector3 direccion = new Vector3(_horizontal, 0, _vertical);
        //Aplicamos gravedad si no estamos tocando el suelo
        if (!_grounded)
        {
            vectorGravity = Vector3.down * 9.81f;
        }
        //Aplicamos la direccion multiplicada por la velocidad a la que no movemos en el tiempo
        _cC.Move((direccion + vectorGravity) * _velocidadMovimiento * Time.deltaTime);
    }
    private void Rotacion()
    {
        //Creamos un plano donde impacta el ray del raton
        Plane playerPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        //Desplegamos el Ray desde la camara a la posicion del raton
        Ray ray = _mainCamera.ScreenPointToRay(_posicionRaton);
        //Si chocamos con el plano ,sacammos la distancia a la que esta para pasarcela al point para saber la posicion
        if (playerPlane.Raycast(ray, out float hitDist))
        {
            _objetivoRaton = ray.GetPoint(hitDist);
        }
        //Calculamos la posicion del raton con respecto al player
        Vector3 direccionRaton = _objetivoRaton - transform.position;
        //Eliminar la y para que no gire hacia arriba
        direccionRaton.y = 0;
        //Calculamos la dirrecion a la que tiene que mirar el objeto
        Quaternion targetRot = Quaternion.LookRotation(direccionRaton);
        //Aplicamos la direcion al transform del player con un pequenio retraso para que quede mas bonito
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * _rotMultiplicador);
    }
    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);

        Collider[] colliders = new Collider[5];

        Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);

        foreach (var other in colliders)
        {
            if (other == null) return;
            if ((_interacteables & (1 << other.gameObject.layer)) != 0)
            {
                if (_reparacion)
                {
                    if (other.TryGetComponent(out DoorObstacle doorObstacle))
                    {
                        doorObstacle.RepairDoor(_cantidadReparacion);
                    }
                }

            }
        }
    }
    private void Disparo()
    {
        PoolManager.Instance.Pull("Bullet", _disparo.position, _disparo.rotation);
    }



    #endregion
}
