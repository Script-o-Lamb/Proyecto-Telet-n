using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualizedGraph : MonoBehaviour
{
    public RecordedTrailData trailData;
    public GameObject dotPrefab;
    public RectTransform contentRect;
    public RectTransform scrollviewRect;
    public float pointSpacing = 10f;

    private float viewportWidth;
    private int bufferCount;
    private int totalPoints;

    private Dictionary<int, GameObject> activeDots = new();
    private Queue<GameObject> dotPool = new();

    void Start()
    {
        totalPoints = trailData.recordedYPositions.Count;

        // Ajustamos tamaño del contenido
        float totalWidth = pointSpacing * trailData.recordedYPositions.Count;
        contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);

        viewportWidth = scrollviewRect.rect.width;

        // Buffer dinámico: puntos visibles + margen a ambos lados
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

        // Desactivar los puntos que ya no están en pantalla
        List<int> keysToRemove = new();
        foreach (var kvp in activeDots)
        {
            if (kvp.Key < firstVisibleIndex || kvp.Key > lastVisibleIndex)
            {
                kvp.Value.SetActive(false);
                dotPool.Enqueue(kvp.Value);
                keysToRemove.Add(kvp.Key);
            }
        }
        foreach (int key in keysToRemove)
        {
            activeDots.Remove(key);
        }

        // Activar o instanciar los nuevos puntos visibles
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
        }
    }
}
