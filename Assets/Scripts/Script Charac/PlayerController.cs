using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Movement")]
    public float walkSpeed = 5f;
    public float ropeSpeed = 2f;

    [Header("Rope Behavior")]
    

    private Rigidbody rb;
    private bool isOnRope = false;
    private float ropeTimer = 0f;
    private float wobbleDirection = 1f;
    private float balanceOffset = 0f;
    private Vector3 ropeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    void Update()
    {
        if (!isOnRope)
        {
            float h = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * h * walkSpeed * Time.deltaTime);
        }
    }        
}
