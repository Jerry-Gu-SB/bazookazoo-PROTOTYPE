using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;
    public Transform playerTransform;
    public float explosionRadius = 5f;
    public float explosionForce = 5f;
    public bool hasExploded = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRigidBody = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("Player not found by Rocket script.");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Exploded on " + collision.collider.name);
        if (!hasExploded && collision.collider.CompareTag("Ground"))
        {
            hasExploded = true;
            Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Vector2 explosionCenter = transform.position;
        Vector2 playerPosition = playerTransform.position;

        float distance = Vector2.Distance(explosionCenter, playerPosition);

        if (distance <= explosionRadius)
        {
            Vector2 direction = (playerPosition - explosionCenter).normalized;
            float forceMagnitude = Mathf.Lerp(explosionForce, 0, distance / explosionRadius);

            playerRigidBody.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
        }
        
    }

}
