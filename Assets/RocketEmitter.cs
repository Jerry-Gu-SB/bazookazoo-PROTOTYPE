using Unity.Netcode;
using UnityEngine;

public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerMovement player;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var rocket = bullet.GetComponent<Rocket>();
            rocket.shooterTeam = player.TeamID.Value;

            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = firePoint.right * 10f;

            bullet.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}
