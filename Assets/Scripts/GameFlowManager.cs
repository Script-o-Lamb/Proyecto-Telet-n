using UnityEngine.SceneManagement;
using UnityEngine;
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
            puntosActuales = 0f; // SIEMPRE reiniciar a 0 al cambiar de perfil
            indexActual = PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
            CargarPuntajesParaInspector();  // ← Cargamos los puntajes al cambiar de perfil
        }
    }

    public void AgregarPuntos(float puntos)
    {
        puntosActuales += puntos;
        if (puntosActuales < 0)
            puntosActuales = 0;
    }

    public float GetPuntosActuales() => puntosActuales;

    public void GuardarPuntajeFinal()
    {
        PlayerPrefs.SetFloat($"Puntaje_{currentRut}_{indexActual}", puntosActuales);
        indexActual = (indexActual + 1) % maxPuntajes;
        PlayerPrefs.SetInt("UltimoIndex_" + currentRut, indexActual);
        PlayerPrefs.Save();
        puntosActuales = 0f;
        CargarPuntajesParaInspector();  // ← refrescamos también al guardar
    }

    public float ObtenerPuntajeGuardado(int i)
    {
        return PlayerPrefs.GetFloat($"Puntaje_{currentRut}_{i}", 0);
    }

    public int ObtenerUltimoIndex()
    {
        return PlayerPrefs.GetInt("UltimoIndex_" + currentRut, 0);
    }

    public string GetRut()
    {
        return currentRut;
    }

    public void CargarPuntajesParaInspector()
    {
        puntajesGuardados.Clear();
        for (int i = 0; i < maxPuntajes; i++)
        {
            puntajesGuardados.Add(ObtenerPuntajeGuardado(i));
        }
    }

    // Método público para que cualquier script pueda obtener el listado completo de puntajes
    public List<float> ObtenerTodosLosPuntajes()
    {
        return new List<float>(puntajesGuardados);
    }
}