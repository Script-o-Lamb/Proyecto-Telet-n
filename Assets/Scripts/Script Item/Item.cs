using UnityEngine;

public class Item : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí podés sumar puntos, reproducir sonido, etc.
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Destroy"))
        {
            this.gameObject.SetActive(false);
        }
    }
}

