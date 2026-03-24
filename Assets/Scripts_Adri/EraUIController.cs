using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EraUIController : MonoBehaviour
{
    [Header("References")]
    public RectTransform trackArea;
    public RectTransform cursor; 
    public GameObject eraMarkerPrefab; 

    [Header("Settings")]
    public Color inactiveColor = Color.white;
    public Color activeColor = Color.yellow;
    public float moveDuration = 0.3f;
    [Range(0f, 0.2f)]
    public float markerPaddingPercent = 0.05f; 

    private List<GameObject> markers = new List<GameObject>();
    private List<string> eraNames = new List<string>();
    private int totalWaves = 1;
    private Coroutine moveCoroutine;

    public void SetupEras(List<string> eras, int wavesCount)
    {
        ClearMarkers();

        if (eras == null || eras.Count == 0 || trackArea == null || eraMarkerPrefab == null) return;

        eraNames = new List<string>(eras);
        totalWaves = Mathf.Max(1, wavesCount);

        Canvas.ForceUpdateCanvases();

        if (cursor != null && cursor.parent != trackArea)
        {
            cursor.SetParent(trackArea, false);
        }

        float padding = Mathf.Clamp01(markerPaddingPercent) * trackArea.rect.width;

        for (int i = 0; i < eraNames.Count; i++)
        {
            GameObject marker = Instantiate(eraMarkerPrefab, trackArea);
            marker.name = "EraMarker_" + i + "_" + eraNames[i];
            var rectTransform = marker.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float t = (eraNames.Count == 1) ? 0f : (float)i / (eraNames.Count - 1);
                // Use centered anchors so positions are relative to track center
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                float halfWidth = trackArea.rect.width * 0.5f;
                float x = Mathf.Lerp(-halfWidth + padding, halfWidth - padding, t);
                // Ensure x is within bounds
                x = Mathf.Clamp(x, -halfWidth + padding, halfWidth - padding);
                rectTransform.anchoredPosition = new Vector2(x, 0f);
            }

            var text = marker.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = eraNames[i];
                text.color = inactiveColor;
            }

            markers.Add(marker);
        }

        if (cursor != null && trackArea != null)
        {
            // Parent cursor to track so positions use same local space
            if (cursor.parent != trackArea) cursor.SetParent(trackArea, false);
            cursor.anchoredPosition = (markers.Count > 0) ? (markers[0].GetComponent<RectTransform>().anchoredPosition) : Vector2.zero;
        }
    }

    public void UpdateForRound(int roundIndex)
    {
        if (markers.Count == 0) return;

        float normalized = (totalWaves <= 1) ? 0f : (float)roundIndex / (float)(totalWaves - 1);
        float eraPosF = normalized * (markers.Count - 1);
        int eraIndex = Mathf.Clamp(Mathf.RoundToInt(eraPosF), 0, markers.Count - 1);

        Vector2 targetPos = markers[eraIndex].GetComponent<RectTransform>().anchoredPosition;
        if (cursor != null)
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCursor(cursor.anchoredPosition, targetPos, moveDuration));
        }

        for (int i = 0; i < markers.Count; i++)
        {
            var txt = markers[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                txt.color = (i == eraIndex) ? activeColor : inactiveColor;
            }
        }
    }

    private IEnumerator MoveCursor(Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cursor.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        cursor.anchoredPosition = to;
        moveCoroutine = null;
    }

    private void ClearMarkers()
    {
        foreach (var marker in markers)
        {
            if (marker != null) Destroy(marker);
        }
        markers.Clear();
    }

    private void OnDestroy()
    {
        ClearMarkers();
    }
}
