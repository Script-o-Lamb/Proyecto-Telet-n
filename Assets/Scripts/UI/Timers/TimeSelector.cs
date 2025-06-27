using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeSelector : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    [SerializeField] string escenarioJuego;
    private int tiempoSeleccionado = 40;

    void Start()
    {
        ActualizarTexto();
    }

    public void AumentarTiempo()
    {
        if (tiempoSeleccionado < 40)
        {
            tiempoSeleccionado += 5;
            ActualizarTexto();
        }
    }

    public void DisminuirTiempo()
    {
        if (tiempoSeleccionado > 5)
        {
            tiempoSeleccionado -= 5;
            ActualizarTexto();
        }
    }

    void ActualizarTexto()
    {
        textoTiempo.text = tiempoSeleccionado.ToString() + " min";
    }

    public void IniciarJuego()
    {
        GameSettings.tiempoEnSegundos = tiempoSeleccionado * 60;
        SceneManager.LoadScene(escenarioJuego);
    }
}
