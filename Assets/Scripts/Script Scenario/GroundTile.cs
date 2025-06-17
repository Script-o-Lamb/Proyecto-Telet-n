using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            TilePoolManager.Instance.ReturnTileToPool(gameObject);
        }
    }
}
