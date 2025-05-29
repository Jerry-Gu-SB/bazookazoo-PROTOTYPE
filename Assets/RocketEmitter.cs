using UnityEngine;
using Unity.Netcode;

public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int playerTeam = 0;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            FireRocketServerRpc(firePoint.position, firePoint.rotation);
        }
    }

    [ServerRpc]
    void FireRocketServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject rocket = Instantiate(bulletPrefab, position, rotation);
        var netObj = rocket.GetComponent<NetworkObject>();
        var rocketScript = rocket.GetComponent<Rocket>();

        rocketScript.shooterTeam = playerTeam;

        Vector2 launchVelocity = rocket.transform.right * 10f;

        // ✅ Set velocity both directly and via network variable
        rocket.GetComponent<Rigidbody2D>().velocity = launchVelocity;
        rocketScript.InitialVelocity.Value = launchVelocity;

        netObj.Spawn();
    }

}
