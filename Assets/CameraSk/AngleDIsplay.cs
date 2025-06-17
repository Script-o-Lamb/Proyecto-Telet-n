using UnityEngine;
using TMPro; 

public class AngleDisplay : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aqu� el objeto de la escena que tiene el script PipeServer.")]
    public PipeServer pipeServer;

    [Tooltip("El componente de texto donde se mostrar� el �ngulo.")]
    public TextMeshProUGUI angleTextElement;

    void Update()
    {
        if (pipeServer != null && angleTextElement != null)
        {
            float angle = pipeServer.shoulderTiltAngle;
            angleTextElement.text = "Inclinaci�n: " + angle.ToString("F1") + "�";
        }
    }
}