using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MostrarListaPuntajesPerfil : MonoBehaviour
{
    public GameObject puntajeItemPrefab; // Prefab con TextMeshProUGUI
    public Transform contentPanel;        // El Content del ScrollView

    public void MostrarPuntajes()
    {
        // Limpiar contenido anterior
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Obtener los puntajes directamente de la lista ya cargada
        List<float> puntajes = GameFlowManager.Instance.ObtenerTodosLosPuntajes();

        for (int i = 0; i < GameFlowManager.Instance.MaxPuntajes; i++)
        {
            float puntaje = GameFlowManager.Instance.ObtenerPuntajeGuardado(i);

            // Crear nuevo item y setear texto
            GameObject nuevoItem = Instantiate(puntajeItemPrefab, contentPanel);
            TextMeshProUGUI texto = nuevoItem.GetComponentInChildren<TextMeshProUGUI>();

            if (texto != null)
                texto.text = $"Puntaje {i + 1}: {puntaje.ToString("0")}";
        }
    }
}
