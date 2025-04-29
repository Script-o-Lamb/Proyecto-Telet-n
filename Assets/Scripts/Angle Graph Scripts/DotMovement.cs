using UnityEngine;

public class DotMovement : MonoBehaviour
{
    private float scrollSpeed;

    private RectTransform rectTransform;
    private float canvasHalfWidth;
    private DotPool dotPool;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(float scrollSpeed, float canvasWidth, DotPool pool)
    {
        this.scrollSpeed = scrollSpeed;
        this.canvasHalfWidth = canvasWidth / 2;
        this.dotPool = pool;
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.x < -canvasHalfWidth - 100)
        {
            dotPool.ReturnDot(gameObject);
        }
    }
}
