using UnityEngine;

public class ShoulderController : MonoBehaviour
{
    // Arrastra aqu� los objetos que representan los hombros desde la jerarqu�a.
    // Estos son los puntos que ya se mueven con tu cuerpo.
    public Transform puntoHombroIzquierdo;
    public Transform puntoHombroDerecho;

    void Update()
    {
        // Nos aseguramos de que los puntos de los hombros existan antes de hacer nada.
        if (puntoHombroIzquierdo == null || puntoHombroDerecho == null)
        {
            return; // Si no hay hombros, no hagas nada.
        }

        // --- 1. Calcular la Posici�n Central ---
        // El inclin�metro se posicionar� justo en el medio de ambos hombros.
        Vector3 posicionCentral = (puntoHombroIzquierdo.position + puntoHombroDerecho.position) / 2f;
        transform.position = posicionCentral;


        // --- 2. Calcular la Rotaci�n (la inclinaci�n) ---
        // Creamos un vector que apunta desde el hombro izquierdo al derecho.
        Vector3 direccionHombros = puntoHombroDerecho.position - puntoHombroIzquierdo.position;

        // Usamos Quaternion.LookRotation para que el objeto "mire" en esa direcci�n.
        // Esto alinea autom�ticamente el objeto con la l�nea de tus hombros.
        // El segundo argumento (Vector3.up) ayuda a estabilizar la rotaci�n.
        if (direccionHombros != Vector3.zero)
        {
            Quaternion rotacionDeInclinacion = Quaternion.LookRotation(Vector3.forward, direccionHombros);
            transform.rotation = rotacionDeInclinacion;
        }
    }
}
