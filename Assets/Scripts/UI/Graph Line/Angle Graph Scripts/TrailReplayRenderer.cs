using System.Collections.Generic;
using UnityEngine;

public class TrailReplayRenderer : MonoBehaviour
{
    public GameObject trailDotPrefab;
    public RectTransform canvasRect;
    public float spacing; 
    [SerializeField] private List<float> recordedYPositions = new();

    public void RenderTrail()
    {
        float startX = -canvasRect.rect.width / 2f;

        for (int i = 0; i < recordedYPositions.Count; i++)
        {
            float xPos = startX + i * spacing;
            float yPos = recordedYPositions[i];

            GameObject dot = Instantiate(trailDotPrefab, canvasRect);
            RectTransform dotRect = dot.GetComponent<RectTransform>();
            dotRect.anchoredPosition = new Vector2(xPos, yPos);
        }
    }

    // Transferencia de scripts
    public void SetRecordedPositions(List<float> positions)
    {
        recordedYPositions = new List<float>(positions);
    }
}
