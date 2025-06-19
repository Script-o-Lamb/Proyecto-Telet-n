using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos = 100f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.AgregarPuntos(cantidadPuntos);
            }

            gameObject.SetActive(false);
        }

        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
    }
}

