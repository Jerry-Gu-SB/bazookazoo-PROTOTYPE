using UnityEngine;
using Unity.Netcode;

public class Rocket : NetworkBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 5f;
    public bool hasExploded = false;

    public NetworkVariable<Vector2> InitialVelocity = new NetworkVariable<Vector2>();

    public NetworkVariable<int> shooterTeam = new NetworkVariable<int>();
    public NetworkVariable<ulong> shooterId = new NetworkVariable<ulong>();

    private Rigidbody2D rb;
    private Collider2D shooterCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            rb.velocity = InitialVelocity.Value;
        }
    }

    public void SetShooterCollider(Collider2D collider)
    {
        shooterCollider = collider;
        // Optionally ignore collision between rocket collider and shooter collider here
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooterCollider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;

        if (collision.collider == shooterCollider) return; // ignore shooter collider

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Rocket"))
        {
            ExplodeAndDestroy();
        }
        else if (collision.collider.CompareTag("Player"))
        {
            var pm = collision.collider.GetComponent<PlayerMovement>();
            if (pm != null && pm.team.Value != shooterTeam.Value)
            {
                ExplodeAndDestroy();
            }
        }
    }

    void ExplodeAndDestroy()
    {
        hasExploded = true;
        Explode();

        if (IsServer)
        {
            NetworkObject.Despawn(true);
        }
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var pm = hit.GetComponent<PlayerMovement>();
                var health = hit.GetComponent<PlayerHealth>();
                if (pm != null && health != null)
                {
                    Rigidbody2D prb = hit.GetComponent<Rigidbody2D>();
                    Vector2 dir = (hit.transform.position - transform.position).normalized;
                    float dist = Vector2.Distance(transform.position, hit.transform.position);
                    float force = Mathf.Lerp(explosionForce, 0, dist / explosionRadius);
                    prb.AddForce(dir * force, ForceMode2D.Impulse);

                    float distanceFactor = dist / explosionRadius;
                    bool isSelf = pm.team.Value == shooterTeam.Value;
                    int maxDamage = isSelf ? 40 : 100;
                    int damage = Mathf.RoundToInt(Mathf.Lerp(maxDamage, 0, distanceFactor));

                    health.TakeDamage(damage);
                }
            }
        }
    }
}
