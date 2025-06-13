using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public static Points Instance { get; private set; }

    private float puntos;
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        // Esto asegura que solo haya una instancia accesible globalmente
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // por si hay más de uno
        }
    }

    void Start()
    {
        // Busca el componente de texto (TextMeshProUGUI) en este mismo objeto
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Actualiza el texto con el valor actual de puntos
        textMesh.text = puntos.ToString("0");
    }

    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
    }
}
