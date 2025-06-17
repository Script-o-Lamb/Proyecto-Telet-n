using System.Collections.Generic;
using UnityEngine;

public class CoinGroupSpawner : MonoBehaviour
{
    [Header("Posiciones de los grupos grandes")]
    public Transform grandePos1;
    public Transform grandePos2;

    [Header("Posiciones de los medianos")]
    public Transform[] medianoPositions; // 4 posiciones

    [Header("Posiciones de los pequeños")]
    public Transform[] pequenoPositions; // 4 posiciones

    [Header("Prefabs disponibles por grupo")]
    public string[] grandesTags = { "GrandeA", "GrandeB" };
    public string[] medianosTags = { "Mediano" };
    public string[] pequenosTags = { "PequenoA", "PequenoB" };

    private void OnEnable()
    {
        ClearChildren();
        SpawnGroups();
    }

    private void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("CoinGroup"))
                Destroy(child.gameObject);
        }
    }

    private void SpawnGroups()
    {
        int grandePosition = 0;

        int grandeChoice = Random.Range(0, 4); // 0 = no spawn, 1 = pos1, 2 = pos2, 3 = pos1 y luego 2

        if (grandeChoice == 1 || grandeChoice == 3)
        {
            string tag = grandesTags[Random.Range(0, grandesTags.Length)];
            SpawnGroup(tag, grandePos1);
            grandePosition = 1;
        }

        if (grandeChoice == 2 || grandeChoice == 3)
        {
            string tag = grandesTags[Random.Range(0, grandesTags.Length)];
            SpawnGroup(tag, grandePos2);
            grandePosition = 2;
        }

        List<int> posicionesLibres = new List<int> { 1, 2, 3, 4 };

        if (grandePosition == 1 || grandePosition == 2)
        {
            posicionesLibres.Remove(1);
            posicionesLibres.Remove(2);
        }

        foreach (int pos in posicionesLibres)
        {
            if (Random.value < 0.5f)
            {
                string tag = medianosTags[0];
                SpawnGroup(tag, medianoPositions[pos - 1]);
            }
        }

        foreach (int pos in posicionesLibres)
        {
            if (Random.value < 0.5f)
            {
                string tag = pequenosTags[Random.Range(0, pequenosTags.Length)];
                SpawnGroup(tag, pequenoPositions[pos - 1]);
            }
        }
    }

    private void SpawnGroup(string tag, Transform spawnPoint)
    {
        GameObject group = CoinGroupPoolManager.Instance.SpawnFromPool(tag, spawnPoint.position, spawnPoint.rotation);
        group.transform.SetParent(transform);
    }
}