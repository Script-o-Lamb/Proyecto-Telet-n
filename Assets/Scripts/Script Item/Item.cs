using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos = 100f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.AgregarPuntos(cantidadPuntos);
            }

            // Usamos el manager para disparar las partículas
            if (ParticlePoolManager.Instance != null && ParticlePoolManager.Instance.pickupParticlePool != null)
            {
                ParticlePoolManager.Instance.pickupParticlePool.PlayParticles(transform.position);
            }

            gameObject.SetActive(false);
        }

        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
    }
}

