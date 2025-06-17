using UnityEngine;

public class MeasureTile : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 size = col.bounds.size;
            Gizmos.DrawWireCube(col.bounds.center, size);
            Debug.Log($"Tamaño del collider: {size}");
        }
    }
}
