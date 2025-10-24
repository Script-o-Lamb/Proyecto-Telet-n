using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AngleRecorderOptimized : MonoBehaviour
{
    [Header("Referencias")]
    public PipeServer pipeServer;
    public RecordedAngleData angleDataAsset;
    public TextMeshProUGUI mensajeUI;

    [Header("Opciones de grabación")]
    public float recordInterval = 0.05f;     // Tiempo entre registros
    public float angleThreshold = 0.5f;      // Diferencia mínima para registrar nuevo ángulo
    public int maxRecordedAngles = 1000;     // Máximo de ángulos a mantener en memoria

    private float timer = 0f;
    private List<float> recordedAngles = new List<float>();
    private bool sessionEnded = false;
    private float lastRecordedAngle = float.NaN; // Para comparar cambios significativos

    void Awake()
    {
        if (angleDataAsset != null)
            angleDataAsset.ClearData();
    }

    void Update()
    {
        if (pipeServer == null) return;

        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            float currentAngle = pipeServer.shoulderTiltAngle;

            // Registrar solo si cambio significativo
            if (float.IsNaN(lastRecordedAngle) || Mathf.Abs(currentAngle - lastRecordedAngle) >= angleThreshold)
            {
                RecordAngle(currentAngle);
                lastRecordedAngle = currentAngle;
            }

            timer -= recordInterval;
        }
    }

    private void RecordAngle(float angle)
    {
        if (recordedAngles.Count >= maxRecordedAngles)
            recordedAngles.RemoveAt(0); // Mantener tamaño máximo (FIFO)

        recordedAngles.Add(angle);
    }

    public float CalcularPromedio()
    {
        if (recordedAngles.Count == 0) return 0f;

        float suma = 0f;
        foreach (float angulo in recordedAngles)
            suma += angulo;

        return suma / recordedAngles.Count;
    }

    public void TerminarSesion()
    {
        if (sessionEnded) return;
        sessionEnded = true;

        // Volcar datos al ScriptableObject
        if (angleDataAsset != null)
        {
            angleDataAsset.ClearData();
            angleDataAsset.recordedAngles.AddRange(recordedAngles);
        }

        // Calcular promedio
        float promedio = CalcularPromedio();

        // Mostrar mensaje
        string mensaje = $"Durante la sesión, el usuario mantuvo su inclinación alrededor de {promedio:F1}°";
        Debug.Log(mensaje);

        if (mensajeUI != null)
            mensajeUI.text = mensaje;

        // Guardar puntaje final
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.GuardarPuntajeFinal();
    }

    void OnDisable()
    {
        if (!sessionEnded)
            TerminarSesion();
    }

    public List<float> GetRecordedAngles() => recordedAngles;
}