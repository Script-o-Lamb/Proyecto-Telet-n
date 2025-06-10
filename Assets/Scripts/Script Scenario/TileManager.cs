using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject groundTilePrefabNormal;
    public GameObject groundTilePrefabRope;
    public Transform playerTransform;
    public int poolSize = 10;
    public float tileLength = 20f;
    public int tilesAhead = 5;

    private Queue<GameObject> tilePool = new Queue<GameObject>();
    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextZ = 0f;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = (i % 2 == 0) ? groundTilePrefabNormal : groundTilePrefabRope;
            GameObject tile = Instantiate(prefab);
            tile.SetActive(false);

            // Asignamos el pool manager al tile
            GroundTile tileScript = tile.GetComponent<GroundTile>();
            if (tileScript != null)
            {
                tileScript.SetPoolManager(this);
            }

            tilePool.Enqueue(tile);
        }

        for (int i = 0; i < tilesAhead; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (playerTransform.position.z + tilesAhead * tileLength > nextZ)
        {
            SpawnTile();
        }
    }

    void SpawnTile()
    {
        if (tilePool.Count == 0) return;

        GameObject tile = tilePool.Dequeue();
        tile.transform.position = new Vector3(0f, 0f, nextZ);
        tile.SetActive(true);
        activeTiles.Add(tile);
        nextZ += tileLength;
    }

    public void ReturnTileToPool(GameObject tile)
    {
        tile.SetActive(false);
        activeTiles.Remove(tile);
        tilePool.Enqueue(tile);
    }
}
