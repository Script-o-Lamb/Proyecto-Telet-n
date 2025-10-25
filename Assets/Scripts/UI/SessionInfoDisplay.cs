using UnityEngine;
using TMPro;

public class SessionInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI tituloSesionText;
    public TextMeshProUGUI puntajeText;
    public TextMeshProUGUI inclinacionText;

    public void MostrarDatosDeSesion(int numeroSesion, float puntaje, float promedioInclinacion)
    {
        tituloSesionText.text = $"Sesión {numeroSesion}";
        puntajeText.text = $"Puntaje: {puntaje:0}";
        inclinacionText.text = $"Promedio de inclinación: {promedioInclinacion:F1}°";
    }
}