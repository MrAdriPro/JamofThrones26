using UnityEngine;

public class PlayerModelController : MonoBehaviour
{

    [Header("Configuración de Eras")]
    [SerializeField] RuntimeAnimatorController[] controllers;

    public int currentSpriteIndex = 0;

    private Animator _animator;
    private PlayerController _playerController;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    public void SwapModel()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % controllers.Length;

        if (currentSpriteIndex < controllers.Length)
        {
            _animator.runtimeAnimatorController = controllers[currentSpriteIndex];

            if (_playerController != null)
            {
                _playerController.RefreshAnimator(_animator);
            }
        }

        Debug.Log("Era cambiada a: " + controllers[currentSpriteIndex].name);
    }
}