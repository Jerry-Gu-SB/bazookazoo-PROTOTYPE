using UnityEngine;
using Unity.Netcode;
public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet(firePoint.position, firePoint.rotation);
        }
    }

    void ShootBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bullet.transform.right * 10f;
    }
}
