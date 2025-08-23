using UnityEngine;
using TMPro;
using System.Collections;

public class BalanceFeedback : MonoBehaviour
{
    [Header("Configuración del equilibrio")]
    public float tolerance = 5f;   // Margen de error en grados
    public float holdTime = 1.5f;  // Tiempo en equilibrio antes de mostrar feedback

    [Header("Feedback visual")]
    public GameObject feedbackTextPrefab;
    [TextArea]
    public string[] motivationalPhrases = {
        "¡Bien hecho!",
        "¡Sigue así!",
        "¡Perfecto!",
        "¡Mantente firme!",
        "¡Excelente control!"
    };

    [Header("Posición del feedback")]
    [Tooltip("Altura sobre el jugador donde aparece el texto.")]
    public float verticalOffset = 2f;
    [Tooltip("Distancia hacia la cámara (para evitar que atraviese el modelo).")]
    public float forwardOffset = 0.5f;

    private float balanceTimer;
    private bool isInBalance;
    private Coroutine feedbackCoroutine;

    private void Update()
    {
        // ---- USAR EJE X ----
        float xRotation = transform.eulerAngles.x;
        if (xRotation > 180) xRotation -= 360; // normalizar ángulo a -180/180

        if (Mathf.Abs(xRotation) <= tolerance)
        {
            balanceTimer += Time.deltaTime;

            if (!isInBalance && balanceTimer >= holdTime)
            {
                isInBalance = true;
                feedbackCoroutine = StartCoroutine(ShowFeedback());
            }
        }
        else
        {
            balanceTimer = 0;
            isInBalance = false;

            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
                feedbackCoroutine = null;
            }
        }
    }

    private IEnumerator ShowFeedback()
    {
        while (isInBalance)
        {
            string phrase = motivationalPhrases[Random.Range(0, motivationalPhrases.Length)];
            SpawnFeedbackText(phrase);

            yield return new WaitForSeconds(2f); // cada 2 segundos lanza un nuevo mensaje
        }
    }

    private void SpawnFeedbackText(string text)
    {
        if (feedbackTextPrefab != null && Camera.main != null)
        {
            // Offset dinámico: arriba + hacia la cámara
            Vector3 spawnPos = transform.position
                               + Vector3.up * verticalOffset
                               + Camera.main.transform.forward * forwardOffset;

            GameObject instance = Instantiate(feedbackTextPrefab, spawnPos, Quaternion.identity);

            TMP_Text tmp = instance.GetComponentInChildren<TMP_Text>();
            if (tmp != null)
                tmp.text = text;

            instance.AddComponent<LookAtCamera>();
            instance.AddComponent<FloatingTextAnimation>();
        }
    }
}

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}

public class FloatingTextAnimation : MonoBehaviour
{
    private TMP_Text text;
    private Vector3 startPos;

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        startPos = transform.position;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 initialScale = Vector3.one * 0.3f;
        Vector3 finalScale = Vector3.one;
        Vector3 targetPos = startPos + Vector3.up * 0.5f;

        Color initialColor = text.color;
        Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Escala (pop desde pequeño hasta normal)
            transform.localScale = Vector3.Lerp(initialScale, finalScale, Mathf.SmoothStep(0, 1, t));

            // Movimiento hacia arriba
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // Desvanecer
            text.color = Color.Lerp(initialColor, finalColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}