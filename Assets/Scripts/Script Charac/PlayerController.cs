using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento General")]
    public float walkSpeed = 5f;

    [Header("Modo Cuerda Floja")]
    public float rotationSpeed = 100f;
    public float maxTiltAngle = 20f;

    public bool onRope = false;
    private Transform staticPivot;
    private float fixedZ;
    private float fixedY;

    private Rigidbody rb;
    private float movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        fixedZ = transform.position.z;
        fixedY = transform.position.y;
    }

    void FixedUpdate()
    {
        movement = Input.GetAxis("Horizontal");


        if (onRope && staticPivot != null)
        {
            RotateAroundStaticPivot();
        }
        else
        {
            MoveNormally();
        }

        float leanInput = Input.GetAxis("Horizontal");
        animator.SetFloat("LeanDirection", leanInput);

    }

    private void MoveNormally()
    {
        Vector3 newVelocity = new Vector3(movement * walkSpeed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = newVelocity;
        
    }

    private void RotateAroundStaticPivot()
    {
        if (staticPivot == null) return;

        float desiredRotation = movement * rotationSpeed * Time.fixedDeltaTime;

        // Calcular vector desde el centro del cilindro al personaje
        Vector3 toPlayer = transform.position - staticPivot.position;

        // El eje de rotación es el local right del cilindro
        Vector3 rotationAxis = staticPivot.right;

        // Proyectamos el vector al personaje en el plano perpendicular al eje de rotación
        Vector3 projected = Vector3.ProjectOnPlane(toPlayer, rotationAxis).normalized;

        // Elegimos un vector de referencia en el mismo plano (usamos up del cilindro)
        Vector3 reference = Vector3.ProjectOnPlane(staticPivot.up, rotationAxis).normalized;

        // Obtenemos el ángulo firmado entre los vectores
        float angle = Vector3.SignedAngle(reference, projected, rotationAxis);

        // Limitamos la rotación solo si no excede los ángulos
        if ((movement > 0 && angle < maxTiltAngle) || (movement < 0 && angle > -maxTiltAngle))
        {
            transform.RotateAround(staticPivot.position, rotationAxis, desiredRotation);
        }

        // Mantener al personaje fijo en el eje Z
        Vector3 pos = transform.position;
        pos.z = fixedZ;
        pos.y = fixedY;
        transform.position = pos;

        rb.linearVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            Transform rope = other.transform.Find("Cilindro");
            if (rope != null)
            {
                GameObject pivotGO = new GameObject("StaticPivot");
                pivotGO.transform.position = rope.position;
                pivotGO.transform.rotation = rope.rotation;
                staticPivot = pivotGO.transform;

                AlignWithRopeX(staticPivot);
                onRope = true;
                rb.useGravity = false;

                fixedZ = transform.position.z;
                fixedY = transform.position.y;
                animator.Play("Balance");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            onRope = false;
            rb.useGravity = true;

            if (staticPivot != null)
                Destroy(staticPivot.gameObject);

            staticPivot = null;

            // Restaurar rotación Z
            Vector3 euler = transform.eulerAngles;
            euler.z = 0f;
            transform.eulerAngles = euler;

            animator.Play("LeanBlend");
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private void AlignWithRopeX(Transform pivot)
    {
        Vector3 pos = transform.position;
        pos.x = pivot.position.x;
        transform.position = pos;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
