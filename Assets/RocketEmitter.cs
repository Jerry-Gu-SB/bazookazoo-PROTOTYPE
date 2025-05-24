using Unity.Netcode;
using UnityEngine;

public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            ShootServerRpc(firePoint.position, firePoint.rotation);
        }
    }

    [ServerRpc]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bullet.transform.right * 10f;
    }
}
