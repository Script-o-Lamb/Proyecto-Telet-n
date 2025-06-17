using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos = 100;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verifica que exista el sistema de puntos
            if (Points.Instance != null)
            {
                Points.Instance.SumarPuntos(cantidadPuntos);
            }

            gameObject.SetActive(false); // o Destroy(gameObject);
        }

        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
    }
}

