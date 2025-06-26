using UnityEngine;

public class Item : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Destroy"))
        {
            this.gameObject.SetActive(false);
        }
    }
}

