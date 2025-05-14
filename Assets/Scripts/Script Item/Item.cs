using UnityEngine;

public class Item : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqu� pod�s sumar puntos, reproducir sonido, etc.
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Destroy"))
        {
            this.gameObject.SetActive(false);
        }
    }
}

