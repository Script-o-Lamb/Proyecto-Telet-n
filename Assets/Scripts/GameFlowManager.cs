using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] private string currentRut = "default"; // valor por defecto
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

    public void SetRut(string rut)
    {
        if (!string.IsNullOrEmpty(rut))
        {
            currentRut = rut;
            // Al cambiar el rut, podrías cargar puntos guardados
            puntosActuales = ObtenerPuntajeGuardado(0); // ejemplo cargar primer puntaje o 0
            indexActual = PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
        }
    }

    public string GetRut()
    {
        return currentRut;
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
        PlayerPrefs.SetFloat($"Puntaje_{currentRut}_{indexActual}", puntosActuales);
        indexActual = (indexActual + 1) % maxPuntajes;
        PlayerPrefs.SetInt("UltimoIndex_" + currentRut, indexActual);
        PlayerPrefs.Save();
        puntosActuales = 0f;
    }

    public float ObtenerPuntajeGuardado(int i)
    {
        return PlayerPrefs.GetFloat($"Puntaje_{currentRut}_{i}", 0);
    }

    public int ObtenerUltimoIndex()
    {
        return PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
    }
}