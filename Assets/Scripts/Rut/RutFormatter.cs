using TMPro;
using UnityEngine;

public class RutFormatter : MonoBehaviour
{
    public TMP_InputField rutInputField;

    private bool isFormatting = false;

    void Start()
    {
        rutInputField.characterLimit = 12; 
        rutInputField.onValueChanged.AddListener(FormatRut);
    }

    private void FormatRut(string input)
    {
        if (isFormatting) return;
        isFormatting = true;

        string clean = "";
        foreach (char c in input)
        {
            if (char.IsDigit(c) || c == 'k' || c == 'K')
            {
                clean += c;
            }
        }

        clean = clean.ToUpper();

        if (clean.Length > 9)
        {
            clean = clean.Substring(0, 9);
        }

        if (clean.Length < 2)
        {
            rutInputField.text = clean;
            rutInputField.caretPosition = rutInputField.text.Length;
            isFormatting = false;
            return;
        }

        string dv = clean.Substring(clean.Length - 1, 1);
        string numeros = clean.Substring(0, clean.Length - 1);

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
