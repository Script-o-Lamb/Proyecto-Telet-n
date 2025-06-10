using UnityEngine;

public class MeasureTile : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Gizmos.color = Color.green;
            Vector3 size = rend.bounds.size;
            Gizmos.DrawWireCube(rend.bounds.center, size);
            Debug.Log($"Tamaño del suelo: {size}");
        }
    }
}
