using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float horizontalInput;

    [SerializeField]
    private bool isGrounded;

    private void Update()
    {
        if (!IsOwner) return;
        
        horizontalInput = Input.GetAxis("Horizontal");
        Debug.Log("Input: " + horizontalInput);
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aim = (mouseWorld - transform.position);
        aim.Normalize();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        Vector2 velocityChange = targetVelocity - rb.velocity;
        rb.AddForce(new Vector2(velocityChange.x, 0), ForceMode2D.Force);
        Debug.Log("Velocity: " + rb.velocity);
    }
}
