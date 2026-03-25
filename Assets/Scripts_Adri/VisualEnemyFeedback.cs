using System.Collections;
using UnityEngine;

public class VisualEnemyFeedback : MonoBehaviour
{
    public SpriteRenderer sprite;
    private Color originalColor;
    [SerializeField] private float duration = 0.2f;

    private void Start()
    {
        originalColor = sprite.color;
    }

    public void PlayDamageFeedBack()
    {
        if (sprite == null) return;

        StopAllCoroutines();
        StartCoroutine(DamageDuration());
    }

    private IEnumerator DamageDuration()
    {
        
        sprite.color = Color.red;
        yield return new WaitForSeconds(duration);
        sprite.color = originalColor;
        
    }
}
