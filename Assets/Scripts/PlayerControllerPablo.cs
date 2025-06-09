using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPablo : MonoBehaviour 
{
    [Header("General Movement")]
    public float forwardSpeed = 5f;
    public float lateralSpeed = 5f;

    [Header("Rope Tilt")]
    public float balanceSpeed = 50f;
    public float maxTiltAngle = 10f;
    public bool onRope = false;
    public float ropeSpeed = 1.0f;

    [Header("Disk Movement")]


    private Rigidbody rb;
    private float currentTilt = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GyroController.Instance.LoadCalibration();
        GyroController.Instance.SetActiveAxes(true, false, false);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {

        if (onRope)
        {
            // Movimiento en la cuerda
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, ropeSpeed);

            // Oscilación automática
            currentTilt += Mathf.Sin(Time.time * 2f) * Time.deltaTime * 10f;

            // Corregir oscilación con giroscopio
            float deviceTilt = GyroController.Instance != null ? GyroController.Instance.Tilt : 0f;
            currentTilt -= deviceTilt * balanceSpeed * Time.deltaTime;

            currentTilt = Mathf.Clamp(currentTilt, -maxTiltAngle, maxTiltAngle);
            transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);

        }
        else
        {
            // Movimiento lateral con giroscopio
            float deviceTilt = GyroController.Instance != null ? GyroController.Instance.Tilt : 0f;
            float moveX = Mathf.Clamp(deviceTilt, -1f, 1f) * lateralSpeed;

            rb.linearVelocity = new Vector3(moveX, rb.linearVelocity.y, forwardSpeed);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            GyroController.Instance.SetActiveAxes(true, false, true);
            AlignWithRopeCenter(other.transform);
            AttachToRope();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            GyroController.Instance.SetActiveAxes(true, false, false);
            DetachFromRope();
        }
    }

    private void AlignWithRopeCenter(Transform ropeTransform)
    {
        Collider ropeCollider = ropeTransform.GetComponent<Collider>();
        if (ropeCollider != null)
        {
            Bounds bounds = ropeCollider.bounds;
            float yOffset = 0.5f;

            Vector3 newPosition = new Vector3(bounds.center.x, bounds.max.y + yOffset, transform.position.z);
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void AttachToRope()
    {
        onRope = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        rb.useGravity = false;
        currentTilt = 0f;
    }

    private void DetachFromRope()
    {
        onRope = false;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = true;
    }
}
