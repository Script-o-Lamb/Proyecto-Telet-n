using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AngleInput : MonoBehaviour
{
    public PlayerController playerController;
    public float moveSpeed = 10f;
    public string inputAxis;

    public RectTransform movementContainer; 

    private RectTransform pointRect;
    private Vector2 initialPosition;
    private float moveRange;

    void Start()
    {
        pointRect = GetComponent<RectTransform>();
        initialPosition = pointRect.anchoredPosition;

        if (movementContainer != null)
        {
            moveRange = movementContainer.rect.height / 2f;
        }
        else
        {
            Debug.LogWarning("No se asignó un contenedor de movimiento. El movimiento no tendrá límites.");
            moveRange = 200f; 
        }
    }

    void Update()
    {
        //float input = Input.GetAxisRaw(inputAxis);
        float input = 0f;
        if (playerController != null)
        {
            input = playerController.movement;
        }
        float targetY = input * moveRange;

        float newY = Mathf.Lerp(pointRect.anchoredPosition.y, initialPosition.y + targetY, Time.deltaTime * moveSpeed);

        float minY = movementContainer.rect.yMin;
        float maxY = movementContainer.rect.yMax;

        newY = Mathf.Clamp(newY, minY, maxY);

        pointRect.anchoredPosition = new Vector2(pointRect.anchoredPosition.x, newY);
    }
}
