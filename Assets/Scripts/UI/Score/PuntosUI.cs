using TMPro;
using UnityEngine;
using System.Collections;

public class PuntosUI : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public Color normalColor = Color.white;
    public Color colorGanado = Color.blue;
    public Color colorPerdido = Color.red;

    public float duracionCambioColor = 1f;
    public float escalaMaxima = 1.3f;
    public float duracionPulso = 0.3f;

    private float puntosPrevios = -1f;
    private Coroutine colorCoroutine;

    private Vector3 escalaOriginal;

    void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.color = normalColor;
        escalaOriginal = textMesh.transform.localScale;

        ActualizarTexto();
    }

    void Update()
    {
        float puntosActuales = GameFlowManager.Instance.GetPuntosActuales();

        if (puntosActuales != puntosPrevios)
        {
            ActualizarTexto();

            if (colorCoroutine != null)
                StopCoroutine(colorCoroutine);

            if (puntosActuales > puntosPrevios)
                colorCoroutine = StartCoroutine(CambiarColorYPulso(colorGanado));
            else
                colorCoroutine = StartCoroutine(CambiarColorYPulso(colorPerdido));

            puntosPrevios = puntosActuales;
        }
    }

    void ActualizarTexto()
    {
        textMesh.text = GameFlowManager.Instance.GetPuntosActuales().ToString("0");
    }

    IEnumerator CambiarColorYPulso(Color colorCambio)
    {
        textMesh.color = colorCambio;

        // Pulso escala hacia arriba
        float tiempo = 0f;
        while (tiempo < duracionPulso)
        {
            float t = tiempo / duracionPulso;
            float escalaActual = Mathf.Lerp(escalaOriginal.x, escalaMaxima, Mathf.Sin(t * Mathf.PI));
            textMesh.transform.localScale = escalaOriginal * escalaActual;
            tiempo += Time.deltaTime;
            yield return null;
        }

        textMesh.transform.localScale = escalaOriginal;

        // Esperar el resto del tiempo del cambio de color
        yield return new WaitForSeconds(duracionCambioColor - duracionPulso);

        textMesh.color = normalColor;
    }
}
