using UnityEngine;
using TMPro;

public class AngleResultDisplay : MonoBehaviour
{
    public RecordedAngleData angleDataAsset; // Referencia al mismo ScriptableObject
    public TextMeshProUGUI resultadoText;

    void Start()
    {
        if (angleDataAsset == null || resultadoText == null) return;

        // Calcular promedio
        float promedio = 0f;
        var lista = angleDataAsset.recordedAngles;
        if (lista.Count > 0)
        {
            float suma = 0f;
            foreach (float angulo in lista)
                suma += angulo;
            promedio = suma / lista.Count;
        }

        // Mostrar mensaje
        resultadoText.text = $"Durante la sesión, el usuario mantuvo su inclinación alrededor de {promedio:F1}°";
    }
}
