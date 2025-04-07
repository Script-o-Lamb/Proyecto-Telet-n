using UnityEngine;

public class SectionTrigger : MonoBehaviour
{

    [SerializeField] GameObject roadSeaction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(roadSeaction,new Vector3(0, 0, -19), Quaternion.identity);
        }
    }
}
