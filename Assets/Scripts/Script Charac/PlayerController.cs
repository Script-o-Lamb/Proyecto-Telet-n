using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Referencia Tracker Camara")] 
    public PipeServer bodyTracker;
    public float sensTilt = 1.5f;

    private float smoothedMovement; // Guardar� el valor suavizado
    [Tooltip("Controla el suavizado. M�s alto = m�s r�pido y nervioso. M�s bajo = m�s suave y con m�s inercia.")]
    public float smoothingFactor = 5f;

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

    [Header("P�rdida de puntos por inclinaci�n")]
    public float tiltThreshold = 10f; // Grados a partir de los cuales empieza a perder
    public float pointsLossPerTick = 10f;  // Puntos que se pierden por tick
    public float lossInterval = 1f; // Cada cu�ntos segundos pierde puntos

    private float lossTimer = 0f;
    private bool isTiltingTooMuch = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        fixedZ = transform.position.z;
        fixedY = transform.position.y;
    }

    void FixedUpdate()
    {
        //movement = Input.GetAxis("Horizontal");
        float rawMovement = 0f;
        if (bodyTracker != null)
        {
            // Leemos el �ngulo de inclinaci�n desde el tracker
            float tiltAngle = bodyTracker.shoulderTiltAngle;

            // Convertimos el �ngulo a un valor entre -1 y 1 para que el resto del c�digo funcione igual.
            // El movimiento ser� m�ximo cuando el �ngulo llegue al 'maxTiltAngle'.
            rawMovement = Mathf.Clamp(tiltAngle / maxTiltAngle, -1f, 1f);
        }
        movement = Mathf.Lerp(movement, rawMovement, smoothingFactor * Time.fixedDeltaTime);

        

        if (onRope && staticPivot != null)
        {
            RotateAroundStaticPivot();
        }
        else
        {
            MoveNormally();
        }

        //float leanInput = Input.GetAxis("Horizontal");
        animator.SetFloat("LeanDirection", movement);
    }

    private void MoveNormally()
    {
        Vector3 newVelocity = new Vector3(movement * walkSpeed * sensTilt, rb.linearVelocity.y, 0f);
        rb.linearVelocity = newVelocity;
    }

    private void RotateAroundStaticPivot()
    {
        if (staticPivot == null) return;

        float desiredRotation = movement * rotationSpeed * Time.fixedDeltaTime;

        Vector3 toPlayer = transform.position - staticPivot.position;
        Vector3 rotationAxis = staticPivot.right;
        Vector3 projected = Vector3.ProjectOnPlane(toPlayer, rotationAxis).normalized;
        Vector3 reference = Vector3.ProjectOnPlane(staticPivot.up, rotationAxis).normalized;

        float angle = Vector3.SignedAngle(reference, projected, rotationAxis);

        if ((movement > 0 && angle < maxTiltAngle) || (movement < 0 && angle > -maxTiltAngle))
        {
            transform.RotateAround(staticPivot.position, rotationAxis, desiredRotation);
        }

        Vector3 pos = transform.position;
        pos.z = fixedZ;
        pos.y = fixedY;
        transform.position = pos;

        rb.linearVelocity = Vector3.zero;

        // L�gica de p�rdida de puntos por inclinaci�n
        if (Mathf.Abs(angle) > tiltThreshold)
        {
            isTiltingTooMuch = true;
        }
        else
        {
            isTiltingTooMuch = false;
            lossTimer = 0f;
        }

        if (isTiltingTooMuch)
        {
            lossTimer += Time.fixedDeltaTime;
            if (lossTimer >= lossInterval)
            {
                if (Points.Instance != null)
                {
                    Points.Instance.SumarPuntos(-pointsLossPerTick);
                }
                lossTimer = 0f;
            }
        }
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
