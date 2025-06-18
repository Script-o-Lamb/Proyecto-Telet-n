using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Paneles Principales")]
    public GameObject panelPrincipal;
    public GameObject panelSeleccionarUsuario;
    public GameObject panelOpciones;
    public GameObject panelRevisarPuntuacion;
    public GameObject panelComoJugar;
    public GameObject panelPrepararNivel;

    [Header("Escenas")]
    public string escena;

    public void MostrarPanel(GameObject panelAMostrar)
    {
        OcultarTodosLosPaneles();
        panelAMostrar.SetActive(true);
    }

    private void OcultarTodosLosPaneles()
    {
        panelPrincipal.SetActive(false);
        panelSeleccionarUsuario.SetActive(false);
        panelOpciones.SetActive(false);
        panelRevisarPuntuacion.SetActive(false);
        panelComoJugar.SetActive(false);
        panelPrepararNivel.SetActive(false);
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene(escena);
    }
}
