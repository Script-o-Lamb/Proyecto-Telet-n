using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Movement")]
    public float walkSpeed = 5f;
    //public float ropeSpeed = 2f;

    //[Header("Rope Behavior")]
    

    private Rigidbody rb;
    //private bool isOnRope = false;
    //private float ropeTimer = 0f;
    //private float wobbleDirection = 1f;
    //private float balanceOffset = 0f;
    //private Vector3 ropeDirection;

    public float movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if (!isOnRope)
        //{
            movement = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector3(movement, 0f, 0f).normalized * walkSpeed;
        //}
    }
}
