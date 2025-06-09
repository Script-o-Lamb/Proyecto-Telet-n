using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Movement")]
    public float walkSpeed = 5f;

    [Header("Rope Mode")]
    public float rotationSpeed = 100f;
    public bool onRope = false;
    public float maxRopeAngle = 45f; // grados máximos de giro permitidos
    private float currentAngle = 0f;

    private Transform ropePivot;
    private Rigidbody rb;
    private float movement;
    private Vector3 initialOffset; // offset inicial respecto al centro del cilindro

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        movement = Input.GetAxis("Horizontal");

        if (onRope && ropePivot != null)
        {
            RotateAroundRope();
        }
        else
        {
            MoveNormally();
        }
    }

    private void MoveNormally()
    {
        Vector3 newVelocity = new Vector3(movement * walkSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        rb.linearVelocity = newVelocity;
    }

    private void RotateAroundRope()
    {
        float deltaAngle = movement * rotationSpeed * Time.fixedDeltaTime;
        currentAngle = Mathf.Clamp(currentAngle + deltaAngle, -maxRopeAngle, maxRopeAngle);

        Vector3 ropeAxis = ropePivot.forward;

        // Para que rote sobre el eje z del ropePivot, hacemos:
        // rotar el personaje alrededor del eje ropeAxis en ropePivot.position, pero con el ángulo acumulado "currentAngle"

        // Primero, poner al personaje en la posición correcta (offset desde ropePivot)
        Vector3 offset = transform.position - ropePivot.position;

        // Rotamos el offset desde su posición inicial
        Quaternion rotation = Quaternion.AngleAxis(currentAngle, ropeAxis);
        Vector3 rotatedOffset = rotation * initialOffset;

        transform.position = ropePivot.position + rotatedOffset;

        // Finalmente, rotamos al personaje para que esté alineado con el ángulo actual
        transform.rotation = rotation;

        // No movemos rb, o seteamos rb.linearVelocity a cero
        rb.linearVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            ropePivot = other.transform.Find("Cilindro");
            if (ropePivot != null)
            {
                AlignWithRopeX(ropePivot);
                onRope = true;
                rb.useGravity = false;

                // Guardar offset inicial respecto al cilindro
                initialOffset = transform.position - ropePivot.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            onRope = false;
            ropePivot = null;
            rb.useGravity = true;
        }
    }

    private void AlignWithRopeX(Transform pivot)
    {
        Vector3 pos = transform.position;
        pos.x = pivot.position.x;
        transform.position = pos;
    }
}
