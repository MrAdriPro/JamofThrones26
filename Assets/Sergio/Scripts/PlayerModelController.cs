using UnityEngine;

public class PlayerModelController : MonoBehaviour
{

    [Header("Configuración de Eras")]
    [SerializeField] RuntimeAnimatorController[] controllers;

    public int currentSpriteIndex = 0;

    private Animator _animator;
    private PlayerController _playerController;

    public bool CanEvolve => currentSpriteIndex < controllers.Length - 1;


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
    }


    public void SwapModel()
    {
        if (CanEvolve)
        {
            currentSpriteIndex++;
            _animator.runtimeAnimatorController = controllers[currentSpriteIndex];

            if (_playerController != null)
            {
                _playerController.RefreshAnimator(_animator);
            }
        }
    }

    public bool IsMaxGrade()
    {
        return currentSpriteIndex >= controllers.Length - 1;
    }
}