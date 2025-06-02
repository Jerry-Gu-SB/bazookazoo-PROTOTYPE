using UnityEngine;
using Unity.Netcode;

public class Rocket : NetworkBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 5f;
    public bool hasExploded = false;
    public int shooterTeam;
    public int maxDamageToEnemy = 100;
    public int maxDamageToSelf = 40;

    private Rigidbody2D rb;

    public NetworkVariable<Vector2> InitialVelocity = new NetworkVariable<Vector2>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            // Apply velocity immediately on all clients when spawned
            rb.velocity = InitialVelocity.Value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;

        if (collision.collider.CompareTag("Ground"))
        {
            ExplodeAndDestroy();
        }
        else if (collision.collider.CompareTag("Player"))
        {
            var pm = collision.collider.GetComponent<PlayerMovement>();
            if (pm != null && pm.team.Value != shooterTeam)
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
            NetworkObject.Despawn();
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
            var netObj = hit.GetComponent<NetworkObject>();

            if (pm != null && health != null && netObj != null)
            {
                // Explosion force
                Rigidbody2D prb = hit.GetComponent<Rigidbody2D>();
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                float force = Mathf.Lerp(explosionForce, 0, dist / explosionRadius);
                prb.AddForce(dir * force, ForceMode2D.Impulse);

                // Damage falloff
                float distanceFactor = dist / explosionRadius;

                // Compare hit player's team to shooter
                bool isSelf = pm.team.Value == shooterTeam;

                int maxDamage = isSelf ? maxDamageToSelf : maxDamageToEnemy;
                int damage = Mathf.RoundToInt(Mathf.Lerp(maxDamage, 0, distanceFactor));

                health.TakeDamage(damage);
            }
        }
    }
}

}
