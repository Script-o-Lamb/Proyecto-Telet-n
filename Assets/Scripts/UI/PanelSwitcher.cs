using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Paneles")]
    public List<GameObject> paneles; // Lista genérica de paneles

    [Header("Escenas")]
    public string escena;

    public void MostrarPanel(GameObject panelAMostrar)
    {
        foreach (var panel in paneles)
        {
            panel.SetActive(panel == panelAMostrar);
        }
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene(escena);
    }
}
