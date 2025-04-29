using System.Collections.Generic;
using UnityEngine;

public class trailmanagerprueba : MonoBehaviour
{
    public DotPool dotPool;
    public RectTransform canvasRect;
    public float trailSpacing = 0.1f;
    public float scrollSpeed = 100f;

    private float timer = 0f;
    [SerializeField] private List<float> recordedYPositions = new();

    private RectTransform pointRect;

    void Awake()
    {
        pointRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= trailSpacing)
        {
            CreateTrailDot();
            timer -= trailSpacing;
        }
    }

    void CreateTrailDot()
    {
        GameObject dot = dotPool.GetDot();
        dot.transform.SetParent(canvasRect, false);

        RectTransform dotRect = dot.GetComponent<RectTransform>();
        dotRect.anchoredPosition = pointRect.anchoredPosition;

        float halfWidth = canvasRect.rect.width / 2f;
        Vector2 spawnPos = new Vector2(halfWidth, pointRect.anchoredPosition.y);
        dotRect.anchoredPosition = spawnPos;

        DotMovementPrueba movement = dot.GetComponent<DotMovementPrueba>();
        if (movement != null)
        {
            movement.Initialize(scrollSpeed, canvasRect.rect.width, dotPool);
        }

        recordedYPositions.Add(pointRect.anchoredPosition.y);
    }

    public List<float> GetRecordedYPositions()
    {
        return recordedYPositions;
    }
}
