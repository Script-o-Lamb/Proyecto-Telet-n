using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] private float tiempoRestante;
    private bool contadorActivo = true;
    [SerializeField] private string finalScene;

    public TextMeshProUGUI textoTiempo;
    [SerializeField] private AngleRecorder angleRecorder;

    void Start()
    {
        Time.timeScale = 1f;
        tiempoRestante = GameSettings.tiempoEnSegundos;
    }

    void Update()
    {
        if (!contadorActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            MostrarTiempo(tiempoRestante);
        }
        else
        {
            tiempoRestante = 0;
            MostrarTiempo(tiempoRestante);
            contadorActivo = false;
            LoadNextScene();
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
        // Terminar la sesión desde AngleRecorder
        if (angleRecorder != null)
            angleRecorder.TerminarSesion();

        SceneManager.LoadScene(finalScene);
    }
}
