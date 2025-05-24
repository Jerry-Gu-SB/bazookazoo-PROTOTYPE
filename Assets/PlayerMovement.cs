using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public Vector2 movement;
    public Vector2 lastMovementDirection = Vector2.right;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float horizontalInput;

    [SerializeField]
    private bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        Vector2 velocityChange = targetVelocity - rb.velocity;

        // Only adjust horizontal velocity
        rb.AddForce(new Vector2(velocityChange.x, 0), ForceMode2D.Force);
    }

}
