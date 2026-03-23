using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] Transform _playerTransform;

    [Header("Movimiento")]
    float _inputX;
    float _inputY;
    Vector2 _mousePosition;
    #endregion




    #region Funciones Unity
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion




    #region Input System
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _inputX = input.x;
            _inputY = input.y;
        }
        else if (context.canceled)
        {
            _inputX = 0;
            _inputY = 0;
        }
    }
    public void OnMouse(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }
    #endregion
}
