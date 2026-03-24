using UnityEngine;

public class EraProgresUI : MonoBehaviour
{
    public RectTransform cursor; 
    public RectTransform startPoint;
    public RectTransform endPoint;

    [Range(0, 1)]
    public float progress; 

    public void UpdateProgress(float value)
    {
        progress = Mathf.Clamp01(value);

        Vector3 pos = Vector3.Lerp(
            startPoint.position,
            endPoint.position,
            progress
        );

        cursor.position = pos;
    }
}
