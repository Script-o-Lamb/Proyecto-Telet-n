using TMPro;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI puntosTexto;
    public Color normalColor = Color.white;
    public Color positiveColor = Color.blue;
    public Color negativeColor = Color.red;
    public float flashDuration = 1f;

    private Coroutine flashCoroutine;

    public void ActualizarPuntos(int puntos)
    {
        puntosTexto.text = puntos.ToString();
    }

    public void FlashColor(int delta)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine(delta));
    }

    private IEnumerator FlashRoutine(int delta)
    {
        if (delta > 0)
            puntosTexto.color = positiveColor;
        else if (delta < 0)
            puntosTexto.color = negativeColor;

        yield return new WaitForSeconds(flashDuration);

        puntosTexto.color = normalColor;
    }
}
