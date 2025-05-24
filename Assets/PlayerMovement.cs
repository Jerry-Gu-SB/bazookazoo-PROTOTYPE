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

    public NetworkVariable<int> TeamID = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<Vector2> AimDirection = new NetworkVariable<Vector2>(
        Vector2.right,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            TeamID.Value = Random.Range(0, 2);
        }

        GetComponent<SpriteRenderer>().color = TeamID.Value == 0 ? Color.red : Color.blue;
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

        AimDirection.Value = aim;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        Vector2 velocityChange = targetVelocity - rb.velocity;
        rb.AddForce(new Vector2(velocityChange.x, 0), ForceMode2D.Force);
    }
}
