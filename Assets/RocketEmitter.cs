using UnityEngine;
using Unity.Netcode;

public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int playerTeam;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = firePoint.position;
            Quaternion spawnRot = firePoint.rotation;
            Vector2 launchVelocity = firePoint.right * 10f;

            // Play local firing effects (muzzle flash, sound) here if you want immediate feedback

            FireRocketServerRpc(spawnPos, spawnRot, launchVelocity);
        }
    }

    [ServerRpc]
    void FireRocketServerRpc(Vector3 position, Quaternion rotation, Vector2 velocity)
    {
        GameObject rocket = Instantiate(bulletPrefab, position, rotation);
        var netObj = rocket.GetComponent<NetworkObject>();
        netObj.Spawn();

        var rocketScript = rocket.GetComponent<Rocket>();

        rocketScript.shooterTeam = playerTeam;
        rocketScript.InitialVelocity.Value = velocity;

        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;

    }
}
