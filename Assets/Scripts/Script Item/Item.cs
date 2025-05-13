using UnityEngine;

public class Item : MonoBehaviour
{
    //public float rotationSpeed = 50f;
    //public float floatAmplitude = 0.25f;
    //public float floatFrequency = 1f;
    public float speed = 5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        //transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        //float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        //transform.position = startPos + new Vector3(0, newY, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí podrías sumar puntaje, efectos, etc.
            FindFirstObjectByType<ItemPooler>().ReturnToPool(gameObject);
        }
    }
}

