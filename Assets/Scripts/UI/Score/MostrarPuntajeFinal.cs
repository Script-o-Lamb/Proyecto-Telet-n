using UnityEngine;
using TMPro;

public class MostrarPuntajeFinal : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;

    void Start()
    {
        int totalRegistros = 10; // igual al maxPuntajes en GameFlowManager
        int ultimoIndice = (GameFlowManager.Instance.ObtenerUltimoIndex() - 1 + totalRegistros) % totalRegistros;
        float ultimoPuntaje = GameFlowManager.Instance.ObtenerPuntajeGuardado(ultimoIndice);
        textoPuntaje.text = "Puntaje: " + Mathf.FloorToInt(ultimoPuntaje).ToString();
    }
}
