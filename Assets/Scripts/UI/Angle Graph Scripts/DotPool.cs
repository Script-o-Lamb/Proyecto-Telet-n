using System.Collections.Generic;
using UnityEngine;

public class DotPool : MonoBehaviour
{
    public GameObject trailDotPrefab;
    public int initialPoolSize = 100;

    private Queue<GameObject> dotPool = new();

    void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject dot = Instantiate(trailDotPrefab, transform);
            dot.SetActive(false);
            dotPool.Enqueue(dot);
        }
    }

    public GameObject GetDot()
    {
        if (dotPool.Count > 0)
        {
            GameObject dot = dotPool.Dequeue();
            dot.SetActive(true);
            return dot;
        }
        else
        {
            GameObject dot = Instantiate(trailDotPrefab, transform);
            return dot;
        }
    }

    public void ReturnDot(GameObject dot)
    {
        dot.SetActive(false);
        dotPool.Enqueue(dot);
    }
}
