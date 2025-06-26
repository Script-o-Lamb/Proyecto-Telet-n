using UnityEngine;

public class ItemGroupGenerator : MonoBehaviour
{
    public string[] groupTags;

    public float spawnRate = 2f;
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCoinGroup();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnCoinGroup()
    {
        string randomTag = groupTags[Random.Range(0, groupTags.Length)];
        ItemGroupPool.Instance.SpawnFromPool(randomTag, transform.position, Quaternion.identity);
    }
}
