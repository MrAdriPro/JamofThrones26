using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    [Header("Referencias Visuales")]
    [SerializeField] GameObject playerModel; 

    [Header("Assets")]
    [SerializeField] Sprite[] sprites;
    [SerializeField] RuntimeAnimatorController[] controllers; 

    public int currentSpriteIndex = 0;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = playerModel.GetComponent<SpriteRenderer>();
        _animator = playerModel.GetComponent<Animator>();

    }

    public void SwapModel()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;

        _spriteRenderer.sprite = sprites[currentSpriteIndex];

        if (currentSpriteIndex < controllers.Length)
        {
            _animator.runtimeAnimatorController = controllers[currentSpriteIndex];
        }

        print("Modelo cambiado a: " + currentSpriteIndex);
    }
}

