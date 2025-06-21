using UnityEngine;
using TMPro;

public class MostrarPuntajeFinal : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;

    void Start()
    {
        // Verificamos que haya rut seteado
        if (string.IsNullOrEmpty(GameFlowManager.Instance.GetRut()))
        {
            textoPuntaje.text = "No hay perfil cargado";
            return;
        }

        int totalRegistros = 10; // igual al maxPuntajes en GameFlowManager
        int ultimoIndice = (GameFlowManager.Instance.ObtenerUltimoIndex() - 1 + totalRegistros) % totalRegistros;
        float ultimoPuntaje = GameFlowManager.Instance.ObtenerPuntajeGuardado(ultimoIndice);
        textoPuntaje.text = "Puntaje: " + Mathf.FloorToInt(ultimoPuntaje).ToString();
    }
}
