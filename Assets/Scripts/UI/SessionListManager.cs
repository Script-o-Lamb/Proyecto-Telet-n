using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionListManager : MonoBehaviour
{
    [Header("Botones de sesiones (1 = sesión 1, etc.)")]
    public Button[] sessionButtons;

    [Header("Referencia a la UI de detalle de sesión")]
    public SessionInfoDisplay sessionInfoDisplay;

    private void OnEnable()
    {
        // Espera un frame para asegurar que GameFlowManager cargó puntajes
        StartCoroutine(ActualizarBotonesAlInicio());
    }

    private System.Collections.IEnumerator ActualizarBotonesAlInicio()
    {
        yield return null; // Espera un frame
        ActualizarBotones();
    }

    public void ActualizarBotones()
    {
        string rutActual = GameFlowManager.Instance.GetRut();

        for (int i = 0; i < sessionButtons.Length; i++)
        {
            int index = i;

            // Verificar si hay sesión guardada
            bool hayDatos = PlayerPrefs.HasKey($"Puntaje_{rutActual}_{index}");

            // Activar/desactivar botón
            sessionButtons[i].gameObject.SetActive(hayDatos);
            sessionButtons[i].interactable = hayDatos;

            // Asignar texto
            TextMeshProUGUI textoBoton = sessionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
                textoBoton.text = $"Sesión {index + 1}";

            // Asignar callback
            sessionButtons[i].onClick.RemoveAllListeners();
            if (hayDatos)
            {
                sessionButtons[i].onClick.AddListener(() => MostrarSesion(index));
            }
        }
    }

    private void MostrarSesion(int index)
    {
        float puntaje = GameFlowManager.Instance.ObtenerPuntajeGuardado(index);
        float promedio = GameFlowManager.Instance.ObtenerPromedioInclinacionGuardado(index);

        if (sessionInfoDisplay != null)
            sessionInfoDisplay.MostrarDatosDeSesion(index + 1, puntaje, promedio);
    }
}