using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private TileManager poolManager;

    public void SetPoolManager(TileManager manager)
    {
        poolManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            poolManager.ReturnTileToPool(this.gameObject);
        }
    }
}
