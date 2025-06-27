using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    public static ParticlePoolManager Instance { get; private set; }

    [Header("Pools disponibles")]
    public ParticlePool pickupParticlePool;
    public ParticlePool splashParticlePool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
