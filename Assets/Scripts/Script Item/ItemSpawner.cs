using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Configuración de los Items")]
    public GameObject itemPrefab; 
    public int numberOfItems = 5; 
    public float distanceBetweenItems = 2f;

    private void Start()
    {
        GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");

        foreach (GameObject rope in ropes)
        {
            SpawnItems(rope.transform);
        }
    }

    void SpawnItems(Transform ropeTransform)
    {
        if (ropeTransform == null)
        {
            Debug.LogError("¡No se ha asignado la cuerda!"); 
            return;
        }

        float ropeLength = ropeTransform.localScale.y; 
        Debug.Log("Longitud de la cuerda: " + ropeLength);

        for (int i = 0; i < numberOfItems; i++)
        {
            Vector3 spawnPosition = ropeTransform.position + ropeTransform.up * i * distanceBetweenItems;

            if (Vector3.Distance(spawnPosition, ropeTransform.position) > ropeLength)
            {
                Debug.Log("Posición excede la longitud de la cuerda, terminando generación.");
                break;
            }

            float heightOffset = 1.0f;
            Vector3 offset = -ropeTransform.forward * heightOffset; 
            Vector3 finalPosition = spawnPosition + offset;

            GameObject item = Instantiate(itemPrefab, finalPosition, Quaternion.identity);

            item.transform.up = ropeTransform.up;
            Debug.Log("Item generado en la posición: " + finalPosition);

            if (!item.GetComponent<ItemCollision>())
            {
                item.AddComponent<ItemCollision>();
            }
        }
    }

    public class ItemCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player")) 
            {
                Debug.Log("Item recogido por el jugador: " + gameObject.name); 
                Destroy(gameObject);
            }
        }
    }
}
