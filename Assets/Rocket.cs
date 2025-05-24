using Unity.Netcode;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 5f;
    public bool hasExploded = false;
    public int shooterTeam;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;
        if (hasExploded) return;

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player"))
        {
            hasExploded = true;
            Explode();
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
                if (pm != null && pm.TeamID.Value != shooterTeam)
                {
                    Rigidbody2D prb = hit.GetComponent<Rigidbody2D>();
                    Vector2 dir = (Vector2)(hit.transform.position - transform.position).normalized;
                    float dist = Vector2.Distance(transform.position, hit.transform.position);
                    float force = Mathf.Lerp(explosionForce, 0, dist / explosionRadius);
                    prb.AddForce(dir * force, ForceMode2D.Impulse);
                }
            }
        }
    }
}
