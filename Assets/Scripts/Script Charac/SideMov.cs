using UnityEngine;

public class SideMov : MonoBehaviour

{
    [Header("Referencia al Tracker")]
    [Tooltip("Arrastra aquí el objeto de la escena que tiene el script PipeServer.")]
    public PipeServer bodyTracker;

    [Header("Configuración de Movimiento por Niveles")]
    [Tooltip("Ángulo mínimo (en grados) para empezar a moverse. Evita movimientos no deseados.")]
    public float anguloZonaMuerta = 3.0f;

    [Tooltip("Velocidad lateral para la primera inclinación.")]
    public float velocidadNivel1 = 2.0f;

    [Tooltip("A partir de este ángulo se activa la velocidad del Nivel 2.")]
    public float anguloParaNivel2 = 8.0f;
    [Tooltip("Velocidad lateral para la inclinación media.")]
    public float velocidadNivel2 = 3.5f;

    [Tooltip("A partir de este ángulo se activa la velocidad del Nivel 3.")]
    public float anguloParaNivel3 = 13.0f;
    [Tooltip("Velocidad máxima para la inclinación pronunciada.")]
    public float velocidadNivel3 = 5.0f;

    private float velocidadDeMovimientoActual;

    void Update()
    {
        if (bodyTracker == null)
        {
            Debug.LogError("Error: No se ha asignado el Body Tracker al PlayerController.");
            return; 
        }

        float anguloInclinacion = bodyTracker.shoulderTiltAngle;

        float anguloAbsoluto = Mathf.Abs(anguloInclinacion);

        if (anguloAbsoluto > anguloParaNivel3)
        {
            velocidadDeMovimientoActual = velocidadNivel3;
        }
        else if (anguloAbsoluto > anguloParaNivel2)
        {
            velocidadDeMovimientoActual = velocidadNivel2;
        }
        else if (anguloAbsoluto > anguloZonaMuerta)
        {
            velocidadDeMovimientoActual = velocidadNivel1;
        }
        else 
        {
            velocidadDeMovimientoActual = 0;
        }

        float direccion = Mathf.Sign(anguloInclinacion);

        Vector3 movimiento = Vector3.right * direccion * velocidadDeMovimientoActual * Time.deltaTime;

        transform.Translate(movimiento);
    }
}

