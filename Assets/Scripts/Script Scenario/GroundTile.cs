using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private int speed = 10;

    private void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            TilePoolManager.Instance.ReturnTileToPool(this.gameObject);
            TilePoolManager.Instance.SpawnTile();
        }
    }
}
