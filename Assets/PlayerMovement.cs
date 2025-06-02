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

    public NetworkVariable<int> team = new NetworkVariable<int>();
    private static int nextTeam = 0;

    [SerializeField] private SpriteRenderer spriteRenderer;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            team.Value = nextTeam;
            nextTeam = (nextTeam + 1) % 2;
        }

        team.OnValueChanged += OnTeamChanged;

        // Set color immediately for local + remote players
        OnTeamChanged(0, team.Value);
    }

    private void OnTeamChanged(int previousValue, int newValue)
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = newValue == 0 ? Color.blue : Color.red;
        }
    }
    
    private void Update()
    {
        if (!IsOwner) return;

        horizontalInput = Input.GetAxis("Horizontal");
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
    }
}
