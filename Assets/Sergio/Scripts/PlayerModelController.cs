using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;

    [SerializeField] Sprite[] sprites;
    [SerializeField] Animator[] animatorList;


    public int currentSpriteIndex = 0;

    private void Update()
    {
        //SwapModel(currentSpriteIndex);
    }

    public void SwapModel()
    {
        if(currentSpriteIndex < sprites.Length - 1)
        {
            currentSpriteIndex += 1;

            // Adquiere 
            SpriteRenderer spriteRenderer = playerModel.GetComponent<SpriteRenderer>();
            //Animator animator = playerModel.GetComponent<Animator>();

            spriteRenderer.sprite = sprites[currentSpriteIndex];
            //animator = animatorList[currentSpriteIndex];
        }
    }
}
