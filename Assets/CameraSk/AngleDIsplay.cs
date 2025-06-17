using UnityEngine;
using TMPro; 

public class AngleDisplay : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Objeto PS")]
    public PipeServer pipeServer;
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