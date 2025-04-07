using UnityEngine;

public class RoadMove : MonoBehaviour
{

    [SerializeField] int speed;
    void Update()
    {
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
    }
}
