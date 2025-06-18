using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento General")]
    public float walkSpeed = 5f;

    [Header("Modo Cuerda Floja")]
    public float rotationSpeed = 100f;
    public float maxTiltAngle = 20f;

    [Header("Transici�n a la Cuerda")]
    public float moveToRopeDuration = 1.5f; // Tiempo configurable desde el inspector

    public bool onRope = false;
    private Transform staticPivot;
    private float fixedZ;
    private float fixedY;

    private Rigidbody rb;
    private float movement;
    private Animator animator;

    private bool canMove = true; // Bloqueo de movimiento manual

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        fixedZ = transform.position.z;
        fixedY = transform.position.y;
    }

    void FixedUpdate()
    {
        if (!canMove) return;

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

                StartCoroutine(MoveToRope(staticPivot.position));
            }
        }
    }

    private IEnumerator MoveToRope(Vector3 targetPosition)
    {
        canMove = false;
        onRope = false;
        rb.useGravity = false;

        animator.Play("JumpTrick");

        float startX = transform.position.x;
        float targetX = targetPosition.x;
        float y = transform.position.y;
        float z = transform.position.z;

        float elapsed = 0f;

        while (elapsed < moveToRopeDuration)
        {
            float t = elapsed / moveToRopeDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            float newX = Mathf.Lerp(startX, targetX, t);
            transform.position = new Vector3(newX, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetX, y, z);
        fixedZ = z;
        fixedY = y;

        AlignWithRopeX(staticPivot);
        onRope = true;
        canMove = true;
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

            animator.Play("BalanceOff");
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
