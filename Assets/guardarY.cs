using System.Collections.Generic;
using UnityEngine;

public class guardarY : MonoBehaviour
{
    public GameObject trailDotPrefab;
    public RectTransform canvasRect;
    public float trailSpacing = 0.1f;
    public float scrollSpeed = 100f;

    private float timer = 0f;
    private List<GameObject> trailDots = new();
    [SerializeField] private List<float> recordedYPositions = new(); // AQUÍ guardamos las Y

    private RectTransform pointRect;

    void Start()
    {
        pointRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Crear nuevo punto cada cierto tiempo
        timer += Time.deltaTime;
        if (timer >= trailSpacing)
        {
            CreateTrailDot();
            timer = 0f;
        }

        // Mover los puntos existentes hacia la izquierda y destruir los que salen
        for (int i = trailDots.Count - 1; i >= 0; i--)
        {
            RectTransform dotRect = trailDots[i].GetComponent<RectTransform>();
            dotRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            if (dotRect.anchoredPosition.x < -canvasRect.rect.width / 2 - 100)
            {
                Destroy(trailDots[i]);
                trailDots.RemoveAt(i);
            }
        }
    }

    void CreateTrailDot()
    {
        GameObject dot = Instantiate(trailDotPrefab, canvasRect);
        dot.GetComponent<RectTransform>().anchoredPosition = pointRect.anchoredPosition;
        trailDots.Add(dot);

        // Guardar la posición Y
        recordedYPositions.Add(pointRect.anchoredPosition.y);
    }

    // Puedes llamar esto al final para exportar o usar los datos
    public List<float> GetRecordedYPositions()
    {
        return recordedYPositions;
    }
}
