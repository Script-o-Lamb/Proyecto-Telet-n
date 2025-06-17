using System.Collections.Generic;
using UnityEngine;

public class TilePoolManager : MonoBehaviour
{
    public static TilePoolManager Instance;

    [Header("Tile Settings")]
    public GameObject normalTilePrefab;
    public GameObject ropeTilePrefab;
    public int poolSize = 10;
    public float tileLength = 193.06f;
    public float moveSpeed = 10f;

    [Header("Spawn Settings")]
    public Transform spawnOrigin;
    public int tilesAhead = 5;

    private Queue<GameObject> normalTilePool = new Queue<GameObject>();
    private Queue<GameObject> ropeTilePool = new Queue<GameObject>();
    private List<GameObject> activeTiles = new List<GameObject>();

    private bool spawnNormalNext = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // Inicializamos pools separados
        for (int i = 0; i < poolSize; i++)
        {
            GameObject normalTile = Instantiate(normalTilePrefab, transform);
            normalTile.SetActive(false);
            normalTilePool.Enqueue(normalTile);

            GameObject ropeTile = Instantiate(ropeTilePrefab, transform);
            ropeTile.SetActive(false);
            ropeTilePool.Enqueue(ropeTile);
        }

        // Llenamos inicialmente
        for (int i = 0; i < tilesAhead; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        MoveActiveTiles();
    }

    private void MoveActiveTiles()
    {
        foreach (var tile in activeTiles)
        {
            tile.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }
    }

    public void ReturnTileToPool(GameObject tile)
    {
        activeTiles.Remove(tile);
        tile.SetActive(false);

        // Devolver al pool correspondiente
        if (tile.CompareTag("NormalTile"))
            normalTilePool.Enqueue(tile);
        else if (tile.CompareTag("RopeTile"))
            ropeTilePool.Enqueue(tile);

        SpawnTile(); // Spawneamos uno nuevo inmediatamente
    }

    private void SpawnTile()
    {
        GameObject tileToSpawn = null;

        if (spawnNormalNext)
        {
            if (normalTilePool.Count > 0)
                tileToSpawn = normalTilePool.Dequeue();
        }
        else
        {
            if (ropeTilePool.Count > 0)
                tileToSpawn = ropeTilePool.Dequeue();
        }

        if (tileToSpawn == null) return;

        Vector3 spawnPos = (activeTiles.Count == 0)
            ? spawnOrigin.position
            : activeTiles[activeTiles.Count - 1].transform.position + Vector3.forward * tileLength;

        tileToSpawn.transform.position = spawnPos;
        tileToSpawn.SetActive(true);
        activeTiles.Add(tileToSpawn);

        // Alternamos para el siguiente
        spawnNormalNext = !spawnNormalNext;
    }
}
