using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AngleRecorder : MonoBehaviour
{
    [Header("Referencias")]
    public PipeServer pipeServer;
    public RecordedAngleData angleDataAsset;
    public TextMeshProUGUI mensajeUI; // Opcional: muestra mensaje final

    [Header("Opciones de grabación")]
    public float recordInterval = 0.05f;     // Tiempo entre registros
    public float angleThreshold = 0.5f;      // Diferencia mínima para registrar nuevo ángulo
    public int maxRecordedAngles = 50000;    // Máximo de ángulos en memoria

    private float timer = 0f;
    private Queue<float> recordedAngles = new Queue<float>();
    private bool sessionEnded = false;
    private float lastRecordedAngle = float.NaN;

    void Awake()
    {
        if (angleDataAsset != null)
            angleDataAsset.ClearData();
    }

    void Update()
    {
        if (pipeServer == null || sessionEnded) return;

        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            float currentAngle = pipeServer.shoulderTiltAngle;

            // Registrar solo si hay cambio significativo
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
            recordedAngles.Dequeue();

        recordedAngles.Enqueue(angle);
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

        // 1. Volcar datos al ScriptableObject
        if (angleDataAsset != null)
        {
            angleDataAsset.ClearData();
            angleDataAsset.recordedAngles.AddRange(recordedAngles);
        }

        // 2. Calcular promedio
        float promedio = CalcularPromedio();

        // 3. Mostrar mensaje
        string mensaje = $"Durante la sesión, el usuario mantuvo su inclinación alrededor de {promedio:F1}°";
        Debug.Log(mensaje);

        if (mensajeUI != null)
            mensajeUI.text = mensaje;

        // 4. Guardar sesión completa en GameFlowManager
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.GuardarSesionFinal(promedio);
    }

    void OnDisable()
    {
        if (!sessionEnded)
            TerminarSesion();
    }

    public float[] GetRecordedAnglesArray() => recordedAngles.ToArray();
}