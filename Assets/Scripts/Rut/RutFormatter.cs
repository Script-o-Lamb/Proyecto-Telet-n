using TMPro;
using UnityEngine;

public class RutFormatter : MonoBehaviour
{
    public TMP_InputField rutInputField;

    private bool isFormatting = false;

    void Start()
    {
        rutInputField.characterLimit = 12; // para incluir puntos y guion (máx 12 caracteres en formato)
        rutInputField.onValueChanged.AddListener(FormatRut);
    }

    private void FormatRut(string input)
    {
        if (isFormatting) return;
        isFormatting = true;

        // Quitamos todo lo que no sea dígito o k/K
        string clean = "";
        foreach (char c in input)
        {
            if (char.IsDigit(c) || c == 'k' || c == 'K')
            {
                clean += c;
            }
        }

        clean = clean.ToUpper();

        // Limitar a máximo 9 caracteres (8 números + 1 DV)
        if (clean.Length > 9)
        {
            clean = clean.Substring(0, 9);
        }

        if (clean.Length < 2)
        {
            rutInputField.text = clean; // Sin formatear aún, no hay DV suficiente
            rutInputField.caretPosition = rutInputField.text.Length;
            isFormatting = false;
            return;
        }

        // Separar números y dígito verificador (último char)
        string dv = clean.Substring(clean.Length - 1, 1);
        string numeros = clean.Substring(0, clean.Length - 1);

        // Insertar puntos cada 3 dígitos desde el final
        string numerosConPuntos = "";
        int count = 0;
        for (int i = numeros.Length - 1; i >= 0; i--)
        {
            numerosConPuntos = numeros[i] + numerosConPuntos;
            count++;
            if (count == 3 && i != 0)
            {
                numerosConPuntos = "." + numerosConPuntos;
                count = 0;
            }
        }

        rutInputField.text = numerosConPuntos + "-" + dv;
        rutInputField.caretPosition = rutInputField.text.Length;

        isFormatting = false;
    }
}
