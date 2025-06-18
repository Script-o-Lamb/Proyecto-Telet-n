using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualizedGraph : MonoBehaviour
{
    public RecordedTrailData trailData;
    public GameObject dotPrefab;
    public GameObject linePrefab;
    public RectTransform contentRect;
    public RectTransform scrollviewRect;
    public float pointSpacing = 10f;

    private float viewportWidth;
    private int bufferCount;
    private int totalPoints;

    private Dictionary<int, GameObject> activeDots = new();
    private Dictionary<int, GameObject> activeLines = new();
    private Queue<GameObject> dotPool = new();
    private Queue<GameObject> linePool = new();

    void Start()
    {
        totalPoints = trailData.recordedYPositions.Count;

        float totalWidth = pointSpacing * totalPoints;
        contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);

        viewportWidth = scrollviewRect.rect.width;
        bufferCount = Mathf.CeilToInt(viewportWidth / pointSpacing) + 10;

        UpdateVisibleDots();
    }

    void Update()
    {
        UpdateVisibleDots();
    }

    void UpdateVisibleDots()
    {
        float scrollX = contentRect.anchoredPosition.x;
        int firstVisibleIndex = Mathf.Max(0, Mathf.FloorToInt(-scrollX / pointSpacing));
        int lastVisibleIndex = Mathf.Min(totalPoints - 1, firstVisibleIndex + bufferCount);

        // Desactivar puntos fuera de pantalla
        List<int> dotsToRemove = new();
        foreach (var kvp in activeDots)
        {
            if (kvp.Key < firstVisibleIndex || kvp.Key > lastVisibleIndex)
            {
                kvp.Value.SetActive(false);
                dotPool.Enqueue(kvp.Value);
                dotsToRemove.Add(kvp.Key);
            }
        }
        foreach (int key in dotsToRemove)
            activeDots.Remove(key);

        // Desactivar líneas fuera de pantalla
        List<int> linesToRemove = new();
        foreach (var kvp in activeLines)
        {
            if (kvp.Key < firstVisibleIndex || kvp.Key > lastVisibleIndex - 1)
            {
                kvp.Value.SetActive(false);
                linePool.Enqueue(kvp.Value);
                linesToRemove.Add(kvp.Key);
            }
        }
        foreach (int key in linesToRemove)
            activeLines.Remove(key);

        // Activar puntos visibles
        for (int i = firstVisibleIndex; i <= lastVisibleIndex; i++)
        {
            if (!activeDots.ContainsKey(i))
            {
                GameObject dot = dotPool.Count > 0 ? dotPool.Dequeue() : Instantiate(dotPrefab, contentRect);
                dot.SetActive(true);

                RectTransform dotRect = dot.GetComponent<RectTransform>();
                float x = i * pointSpacing;
                float y = trailData.recordedYPositions[i];
                dotRect.anchoredPosition = new Vector2(x, y);

                activeDots[i] = dot;
            }

            // Dibujar línea entre este punto y el anterior
            if (i > 0 && !activeLines.ContainsKey(i - 1))
            {
                float x0 = (i - 1) * pointSpacing;
                float y0 = trailData.recordedYPositions[i - 1];
                float x1 = i * pointSpacing;
                float y1 = trailData.recordedYPositions[i];

                GameObject line = linePool.Count > 0 ? linePool.Dequeue() : Instantiate(linePrefab, contentRect);
                line.SetActive(true);

                RectTransform lineRect = line.GetComponent<RectTransform>();

                Vector2 p0 = new Vector2(x0, y0);
                Vector2 p1 = new Vector2(x1, y1);
                Vector2 dir = p1 - p0;
                float distance = dir.magnitude;
                Vector2 midPoint = (p0 + p1) / 2f;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                lineRect.anchoredPosition = midPoint;
                lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y); // mantiene grosor vertical
                lineRect.localRotation = Quaternion.Euler(0, 0, angle);

                activeLines[i - 1] = line;
            }
        }
    }
}
