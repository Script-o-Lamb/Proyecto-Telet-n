using System.Collections.Generic;
using UnityEngine;

public class TilePoolManager : MonoBehaviour
{
    public static TilePoolManager Instance;

    public GameObject tilePrefabNormal;
    public GameObject tilePrefabRope;
    public Transform spawnOrigin;
    public int poolSize = 10;
    public float tileLength = 193.06f;

    private Queue<GameObject> tilePool = new Queue<GameObject>();
    private List<GameObject> activeTiles = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // Llenamos el pool inicial con la mezcla de prefabs
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefabToUse = (Random.value > 0.5f) ? tilePrefabNormal : tilePrefabRope;
            GameObject tile = Instantiate(prefabToUse, transform);
            tile.SetActive(false);
            tilePool.Enqueue(tile);
        }

        // Inicialmente spawnemos algunos tiles
        for (int i = 0; i < poolSize / 2; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        if (tilePool.Count == 0) return;

        GameObject tile = tilePool.Dequeue();

        Vector3 spawnPos;
        if (activeTiles.Count == 0)
        {
            spawnPos = spawnOrigin.position;
        }
        else
        {
            GameObject lastTile = activeTiles[activeTiles.Count - 1];
            spawnPos = lastTile.transform.position + Vector3.forward * tileLength;
        }

        tile.transform.position = spawnPos;
        tile.SetActive(true);
        activeTiles.Add(tile);
    }

    public void ReturnTileToPool(GameObject tile)
    {
        tile.SetActive(false);
        activeTiles.Remove(tile);
        tilePool.Enqueue(tile);
    }
}
