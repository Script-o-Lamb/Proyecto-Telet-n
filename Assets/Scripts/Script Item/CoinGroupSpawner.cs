using System.Collections.Generic;
using UnityEngine;

public class CoinGroupSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CoinGroupOptions
    {
        public Transform[] largePositions;
        public Transform[] mediumPositions;
        public Transform[] smallPositions;
    }

    public CoinGroupOptions spawnOptions;
    public GameObject largePrefab1;
    public GameObject largePrefab2;
    public GameObject mediumPrefab;
    public GameObject smallPrefab1;
    public GameObject smallPrefab2;

    [Header("Altura fija para los grupos")]
    public float coinGroupHeight = 0f;  

    void Start()
    {
        SpawnGroups();
    }

    void SpawnGroups()
    {
        List<int> occupiedPositions = new List<int>();

        int largeSpawnChance = Random.Range(0, 3); 
        if (largeSpawnChance != 2)
        {
            Transform place = spawnOptions.largePositions[largeSpawnChance];
            GameObject prefab = (Random.value < 0.5f) ? largePrefab1 : largePrefab2;
            Vector3 spawnPos = place.position;
            spawnPos.y = coinGroupHeight;
            Instantiate(prefab, spawnPos, Quaternion.identity, transform);

            if (largeSpawnChance == 0)
            {
                occupiedPositions.Add(0);
                occupiedPositions.Add(1);
            }
            else if (largeSpawnChance == 1)
            {
                occupiedPositions.Add(2);
                occupiedPositions.Add(3);
            }
        }

        for (int i = 0; i < spawnOptions.mediumPositions.Length; i++)
        {
            if (occupiedPositions.Contains(i)) continue;
            if (Random.value < 0.5f)
            {
                Transform place = spawnOptions.mediumPositions[i];
                Vector3 spawnPos = place.position;
                spawnPos.y = coinGroupHeight;
                Instantiate(mediumPrefab, spawnPos, Quaternion.identity, transform);
            }
        }

        for (int i = 0; i < spawnOptions.smallPositions.Length; i++)
        {
            if (occupiedPositions.Contains(i)) continue;
            if (Random.value < 0.5f)
            {
                Transform place = spawnOptions.smallPositions[i];
                GameObject prefab = (Random.value < 0.5f) ? smallPrefab1 : smallPrefab2;
                Vector3 spawnPos = place.position;
                spawnPos.y = coinGroupHeight;
                Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}