using System.Collections.Generic;
using UnityEngine;

public class ScrollableTrailDisplay : MonoBehaviour
{
    public RecordedTrailData trailData;
    public GameObject dotPrefab;
    public GameObject linePrefab; // Esto será una simple imagen UI con color y tamaño ajustable
    public RectTransform contentRect;
    public float pointSpacing = 10f;
    public float verticalScale = 1f;

    void Start()
    {
        GenerateTrail();
    }

    void GenerateTrail()
    {
        List<float> yPositions = trailData.recordedYPositions;

        float totalWidth = pointSpacing * yPositions.Count;
        contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);

        List<RectTransform> dotRects = new();

        for (int i = 0; i < yPositions.Count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, contentRect);
            RectTransform dotRect = dot.GetComponent<RectTransform>();

            float x = i * pointSpacing;
            float y = yPositions[i] * verticalScale;

            float yOffset = contentRect.rect.height / 2f;
            dotRect.anchoredPosition = new Vector2(x, y + yOffset);

            dotRects.Add(dotRect);

            // Dibujar línea entre puntos (excepto el primero)
            if (i > 0)
            {
                CreateLineBetween(dotRects[i - 1], dotRect);
            }
        }
    }

    void CreateLineBetween(RectTransform from, RectTransform to)
    {
        GameObject line = Instantiate(linePrefab, contentRect);
        RectTransform lineRect = line.GetComponent<RectTransform>();

        Vector2 startPos = from.anchoredPosition;
        Vector2 endPos = to.anchoredPosition;

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        lineRect.sizeDelta = new Vector2(distance, 2f); // 2f es el grosor de la línea
        lineRect.anchoredPosition = startPos + direction / 2;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.rotation = Quaternion.Euler(0, 0, angle);
    }
}
