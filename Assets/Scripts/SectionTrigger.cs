using UnityEngine;

public class SectionTrigger : MonoBehaviour
{

    [SerializeField] GameObject roadSeaction;
    [SerializeField] Vector3 spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(roadSeaction, spawnPoint, Quaternion.identity);
        }
    }
}
