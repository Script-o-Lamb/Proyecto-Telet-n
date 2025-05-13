using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemPooler pooler;
    public BoxCollider generatorArea;
    public float spawnInterval = 1.5f;
    public int coinsPerSpawn = 3;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCoins), 1f, spawnInterval);
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinsPerSpawn; i++)
        {
            Vector3 spawnPos = GetRandomPositionInBox();
            GameObject coin = pooler.GetFromPool();
            if (coin != null)
            {
                coin.transform.position = spawnPos;
                coin.transform.rotation = Quaternion.identity;
            }
        }
    }

    Vector3 GetRandomPositionInBox()
    {
        Vector3 center = generatorArea.center + generatorArea.transform.position;
        Vector3 size = generatorArea.size;

        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        return new Vector3(x, y, z);
    }
}
