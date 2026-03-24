using System.Collections;
using UnityEngine;

public class EraProgresUI : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform[] eraPoints;
    public RectTransform finalPoint;

    private Coroutine moveCoroutine;

    public void MoveCursorToPosition(Vector3 targetPos, float duration)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveSmooth(targetPos, duration));
    }

    IEnumerator MoveSmooth(Vector3 targetPos, float duration)
    {
        Vector3 startPos = cursor.position;
        float time = 0f;

        while (time < duration)
        {
            cursor.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cursor.position = targetPos;
    }

    public Vector3 GetPoint(int index)
    {
        if (index < eraPoints.Length)
            return eraPoints[index].position;
        else
            return finalPoint.position;
    }
}