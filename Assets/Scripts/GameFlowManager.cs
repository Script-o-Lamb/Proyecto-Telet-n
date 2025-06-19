using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] private float puntosActuales = 0f;
    private int indexActual = 0;
    private const int maxPuntajes = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarPuntos(float puntos)
    {
        puntosActuales += puntos;
        if (puntosActuales < 0)
            puntosActuales = 0;
    }

    public float GetPuntosActuales()
    {
        return puntosActuales;
    }

    public void GuardarPuntajeFinal()
    {
        PlayerPrefs.SetFloat($"Puntaje_{indexActual}", puntosActuales);
        indexActual = (indexActual + 1) % maxPuntajes;
        PlayerPrefs.SetInt("UltimoIndex", indexActual);
        PlayerPrefs.Save();
        puntosActuales = 0f;
    }

    public float ObtenerPuntajeGuardado(int i)
    {
        return PlayerPrefs.GetFloat($"Puntaje_{i}", 0);
    }

    public int ObtenerUltimoIndex()
    {
        return PlayerPrefs.GetInt("UltimoIndex", 0);
    }
}