using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] private string currentRut = "default";
    [SerializeField] private float puntosActuales = 0f;
    private int indexActual = 0;
    private const int maxPuntajes = 10;
    public int MaxPuntajes => maxPuntajes;

    [Header("Debug - Puntajes actuales")]
    [SerializeField] private List<float> puntajesGuardados = new List<float>();

    [Header("Debug - Promedios de inclinación")]
    [SerializeField] private List<float> promediosInclinacionGuardados = new List<float>();

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
            puntosActuales = 0f; // reiniciar puntaje al cambiar de perfil
            indexActual = PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
            CargarPuntajesParaInspector();
            CargarPromediosParaInspector();
        }
    }

    public void AgregarPuntos(float puntos)
    {
        puntosActuales += puntos;
        if (puntosActuales < 0)
            puntosActuales = 0;
    }

    public float GetPuntosActuales() => puntosActuales;

    public void GuardarSesionFinal(float promedioInclinacion)
    {
        // Guardar puntaje y promedio
        PlayerPrefs.SetFloat($"Puntaje_{currentRut}_{indexActual}", puntosActuales);
        PlayerPrefs.SetFloat($"PromedioInclinacion_{currentRut}_{indexActual}", promedioInclinacion);

        // Actualizar listas de debug
        CargarPuntajesParaInspector();
        CargarPromediosParaInspector();

        // Avanzar índice
        indexActual = (indexActual + 1) % maxPuntajes;
        PlayerPrefs.SetInt("UltimoIndex_" + currentRut, indexActual);
        PlayerPrefs.Save();

        puntosActuales = 0f;
    }

    public float ObtenerPuntajeGuardado(int i)
    {
        return PlayerPrefs.GetFloat($"Puntaje_{currentRut}_{i}", 0);
    }

    public float ObtenerPromedioInclinacionGuardado(int i)
    {
        return PlayerPrefs.GetFloat($"PromedioInclinacion_{currentRut}_{i}", 0);
    }

    public void CargarPuntajesParaInspector()
    {
        puntajesGuardados.Clear();
        for (int i = 0; i < maxPuntajes; i++)
        {
            puntajesGuardados.Add(ObtenerPuntajeGuardado(i));
        }
    }

    public void CargarPromediosParaInspector()
    {
        promediosInclinacionGuardados.Clear();
        for (int i = 0; i < maxPuntajes; i++)
        {
            promediosInclinacionGuardados.Add(ObtenerPromedioInclinacionGuardado(i));
        }
    }

    public List<float> ObtenerTodosLosPuntajes() => new List<float>(puntajesGuardados);

    public List<float> ObtenerTodosLosPromedios() => new List<float>(promediosInclinacionGuardados);

    public string GetRut() => currentRut;

    public int ObtenerUltimoIndex() => PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
}