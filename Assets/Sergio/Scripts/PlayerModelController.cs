using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;

    [SerializeField] Sprite[] sprites;
    [SerializeField] Animator[] animatorList;


    public int currentSpriteIndex = 0;

    private void Update()
    {
        SwapModel(currentSpriteIndex);
    }

    void SwapModel(int currentIndex)
    {
        // Adquiere 
        SpriteRenderer spriteRenderer = playerModel.GetComponent<SpriteRenderer>();
        Animator animator = playerModel.GetComponent<Animator>();

        spriteRenderer.sprite = sprites[currentIndex];
        animator = animatorList[currentIndex];
    }
}
