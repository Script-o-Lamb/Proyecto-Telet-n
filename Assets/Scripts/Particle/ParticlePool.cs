using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(particlePrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public void PlayParticles(Vector3 position)
    {
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(particlePrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        GameObject particleObj = pool.Dequeue();
        particleObj.transform.position = position;
        particleObj.SetActive(true);

        ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            StartCoroutine(ReturnToPoolAfterDelay(particleObj, ps.main.duration + ps.main.startLifetime.constantMax));
        }
        else
        {
            pool.Enqueue(particleObj);
        }
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject particleObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        particleObj.SetActive(false);
        pool.Enqueue(particleObj);
    }
}
