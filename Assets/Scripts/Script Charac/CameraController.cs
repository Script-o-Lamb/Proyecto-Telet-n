using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;               
    public Vector3 offset = new Vector3(0, 5, -10); 
    public float followSpeed = 5f;       
    public float adaptSpeedFactor = 2f;   

    private Vector3 currentVelocity;
    private Rigidbody targetRb;

    void Start()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody>();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        float speedFactor = 1f;
        if (targetRb != null)
        {
            float targetSpeed = targetRb.linearVelocity.magnitude;
            speedFactor = 1 + targetSpeed / adaptSpeedFactor;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / (followSpeed * speedFactor));

        transform.LookAt(target);
    }
}

