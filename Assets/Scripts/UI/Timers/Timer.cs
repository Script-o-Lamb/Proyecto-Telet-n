using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] private float tiempoRestante;
    private bool contadorActivo = true;
    [SerializeField] private string finalScene;

    public TextMeshProUGUI textoTiempo; // Asigna en el Inspector

    void Start()
    {
        Time.timeScale = 1f; // Reinicia el tiempo si venías de una pausa
        tiempoRestante = GameSettings.tiempoEnSegundos;
    }

    void Update()
    {
        if (contadorActivo)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                MostrarTiempo(tiempoRestante);
            }
            else
            {
                contadorActivo = false;
                tiempoRestante = 0;
                MostrarTiempo(tiempoRestante);
                LoadNextScene();
            }
        }
    }

    void MostrarTiempo(float tiempoAMostrar)
    {
        tiempoAMostrar = Mathf.Max(0, tiempoAMostrar);
        int minutos = Mathf.FloorToInt(tiempoAMostrar / 60);
        int segundos = Mathf.FloorToInt(tiempoAMostrar % 60);
        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void LoadNextScene()
    {
        // Guardamos el puntaje final y pasamos a la escena final
        GameFlowManager.Instance.GuardarPuntajeFinal();
        SceneManager.LoadScene(finalScene);
    }
}
