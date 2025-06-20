using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RutInput : MonoBehaviour
{
    public TMP_InputField rutInputField;
    public Button submitButton;  // El botón que se activa/desactiva

    void Start()
    {
        if (rutInputField != null)
        {
            rutInputField.onValueChanged.AddListener(FormatRutInput);
            rutInputField.onValueChanged.AddListener(ValidateRutInput);
            rutInputField.onEndEdit.AddListener(OnRutEntered);
        }

        if (submitButton != null)
        {
            submitButton.interactable = false; // Empieza desactivado
        }
    }

    // Da formato tipo "12.345.678-9" mientras el usuario escribe
    private void FormatRutInput(string input)
    {
        string cleaned = CleanRut(input);
        string formatted = FormatRut(cleaned);

        rutInputField.SetTextWithoutNotify(formatted);
        rutInputField.caretPosition = formatted.Length;
    }

    // Valida el RUT para activar o desactivar el botón
    private void ValidateRutInput(string input)
    {
        string cleanedRut = CleanRut(input);
        bool valid = IsValidRutLength(cleanedRut);

        if (submitButton != null)
        {
            submitButton.interactable = valid;
        }
    }

    // Cuando termina de editar o presiona Enter
    private void OnRutEntered(string input)
    {
        string cleanedRut = CleanRut(input);

        if (IsValidRutLength(cleanedRut))
        {
            GameFlowManager.Instance.SetRut(cleanedRut);
            Debug.Log("Perfil cargado: " + cleanedRut);
        }
        else
        {
            Debug.LogWarning("RUT inválido: " + cleanedRut);
        }
    }

    private bool IsValidRutLength(string rut)
    {
        return rut.Length == 8 || rut.Length == 9;
    }

    private string CleanRut(string rut)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (char c in rut)
        {
            if (char.IsDigit(c) || c == 'k' || c == 'K')
                sb.Append(char.ToUpper(c));
        }
        return sb.ToString();
    }

    private string FormatRut(string rut)
    {
        if (string.IsNullOrEmpty(rut))
            return "";

        string numberPart = rut.Length > 1 ? rut.Substring(0, rut.Length - 1) : "";
        string verifier = rut.Length > 0 ? rut.Substring(rut.Length - 1, 1) : "";

        string reversed = ReverseString(numberPart);
        System.Text.StringBuilder formattedNumber = new System.Text.StringBuilder();

        for (int i = 0; i < reversed.Length; i++)
        {
            if (i > 0 && i % 3 == 0)
                formattedNumber.Append('.');
            formattedNumber.Append(reversed[i]);
        }

        string formatted = ReverseString(formattedNumber.ToString());

        if (verifier != "")
            formatted += "-" + verifier;

        return formatted;
    }

    private string ReverseString(string s)
    {
        char[] arr = s.ToCharArray();
        System.Array.Reverse(arr);
        return new string(arr);
    }
}
