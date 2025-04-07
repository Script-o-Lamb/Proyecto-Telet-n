using UnityEngine;

public class DestroyRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroy"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
