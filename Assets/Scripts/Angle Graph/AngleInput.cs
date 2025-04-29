using System.Collections.Generic;
using UnityEngine;

public class AngleInput : MonoBehaviour
{
    public float moveSpeed = 10f;
    public string inputAxis;

    public RectTransform movementContainer; // El fondo donde se mueve el punto

    private RectTransform pointRect;
    private Vector2 initialPosition;
    private float moveRange;

    void Start()
    {
        pointRect = GetComponent<RectTransform>();
        initialPosition = pointRect.anchoredPosition;

        if (movementContainer != null)
        {
            // Calcular el rango a partir del alto del contenedor (mitad hacia arriba y mitad hacia abajo)
            moveRange = movementContainer.rect.height / 2f;
        }
        else
        {
            Debug.LogWarning("No se asignó un contenedor de movimiento. El movimiento no tendrá límites.");
            moveRange = 200f; // fallback
        }
    }

    void Update()
    {
        float input = Input.GetAxisRaw(inputAxis); // valor entre -1 y 1
        float targetY = input * moveRange;

        float newY = Mathf.Lerp(pointRect.anchoredPosition.y, initialPosition.y + targetY, Time.deltaTime * moveSpeed);

        // Limitar dentro del contenedor
        float minY = movementContainer.rect.yMin;
        float maxY = movementContainer.rect.yMax;

        newY = Mathf.Clamp(newY, minY, maxY);

        pointRect.anchoredPosition = new Vector2(pointRect.anchoredPosition.x, newY);
    }
}
