using System.Collections;
using UnityEngine;
using TMPro; 

public class EraProgresUI : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform[] eraPoints;
    public RectTransform finalPoint;

    [Header("Configuración de Textos")]
    public TextMeshProUGUI[] eraTexts; 
    public Color completedColor = new Color(0f, 0.5f, 0f); //little green
    public Color bossColor = Color.red;
    public Color normalColor = Color.white;

    private Coroutine flashCoroutine;
    private bool isShaking = false;
    private Vector3 originalPosBeforeShake;
    private Coroutine moveCoroutine;


    public void SetEraCompleted(int index)
    {
        if (index >= 0 && index < eraTexts.Length)
        {
            eraTexts[index].color = completedColor;
        }
    }

    public void StartBossFlash(int index)
    {
        StopFlash(); 
        flashCoroutine = StartCoroutine(FlashRoutine(index));
    }

    public void StopFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    IEnumerator FlashRoutine(int index)
    {
        if (index < 0 || index >= eraTexts.Length) yield break;

        while (true)
        {
            eraTexts[index].color = bossColor;
            yield return new WaitForSeconds(0.3f);
            eraTexts[index].color = normalColor;
            yield return new WaitForSeconds(0.3f);
        }
    }


    void Update()
    {
        if (isShaking)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 5f;
            randomOffset.z = 0;
            cursor.position = originalPosBeforeShake + randomOffset;
        }
    }

    public void SetShaking(bool shake)
    {
        if (shake) { originalPosBeforeShake = cursor.position; isShaking = true; }
        else { isShaking = false; if (cursor != null) cursor.position = originalPosBeforeShake; }
    }

    public void MoveCursorToPosition(Vector3 targetPos, float duration)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveSmooth(targetPos, duration));
    }

    IEnumerator MoveSmooth(Vector3 targetPos, float duration)
    {
        Vector3 startPos = cursor.position;
        float time = 0f;
        while (time < duration)
        {
            cursor.position = Vector3.Lerp(startPos, targetPos, time / duration);
            if (isShaking) originalPosBeforeShake = cursor.position;
            time += Time.deltaTime;
            yield return null;
        }
        cursor.position = targetPos;
        originalPosBeforeShake = targetPos;
    }

    public Vector3 GetPoint(int index) => (index < eraPoints.Length) ? eraPoints[index].position : finalPoint.position;
}