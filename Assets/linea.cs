using System.Collections.Generic;
using UnityEngine;

public class linea : MonoBehaviour
{
    public GameObject trailDotPrefab;
    public RectTransform canvasRect;
    public float trailSpacing = 0.1f; // tiempo entre cada punto
    public float scrollSpeed = 100f;  // velocidad hacia la izquierda
    public float verticalSpeed = 200f;

    private float timer = 0f;
    private List<GameObject> trailDots = new List<GameObject>();
    private RectTransform pointRect;

    //public string inputString;

    void Start()
    {
        pointRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Movimiento vertical (opcional)
        //Vector2 input = new Vector2(0, Input.GetAxis(inputString));
        //pointRect.anchoredPosition += input * verticalSpeed * Time.deltaTime;

        // Crear nuevo punto cada X segundos
        timer += Time.deltaTime;
        if (timer >= trailSpacing)
        {
            CreateTrailDot();
            timer = 0f;
        }

        // Mover todos los puntos hacia la izquierda
        for (int i = trailDots.Count - 1; i >= 0; i--)
        {
            RectTransform dotRect = trailDots[i].GetComponent<RectTransform>();
            dotRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            // Opcional: borrar si sale del canvas
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
    }
}
