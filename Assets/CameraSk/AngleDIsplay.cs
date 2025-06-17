using UnityEngine;
using TMPro; 

public class AngleDisplay : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto de la escena que tiene el script PipeServer.")]
    public PipeServer pipeServer;

    [Tooltip("El componente de texto donde se mostrará el ángulo.")]
    public TextMeshProUGUI angleTextElement;

    void Update()
    {
        if (pipeServer != null && angleTextElement != null)
        {
            float angle = pipeServer.shoulderTiltAngle;
            angleTextElement.text = "Inclinación: " + angle.ToString("F1") + "°";
        }
    }
}