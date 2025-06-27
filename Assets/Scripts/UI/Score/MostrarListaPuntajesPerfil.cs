using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MostrarListaPuntajesPerfil : MonoBehaviour
{
    public GameObject puntajeItemPrefab; 
    public Transform contentPanel;       

    public void MostrarPuntajes()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        List<float> puntajes = GameFlowManager.Instance.ObtenerTodosLosPuntajes();

        for (int i = 0; i < GameFlowManager.Instance.MaxPuntajes; i++)
        {
            float puntaje = GameFlowManager.Instance.ObtenerPuntajeGuardado(i);

            GameObject nuevoItem = Instantiate(puntajeItemPrefab, contentPanel);
            TextMeshProUGUI texto = nuevoItem.GetComponentInChildren<TextMeshProUGUI>();

            if (texto != null)
                texto.text = $"Puntaje {i + 1}: {puntaje.ToString("0")}";
        }
    }
}
