using UnityEngine;
using Unity.Netcode;

public class RocketEmitter : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int localPlayerTeam;
    public int launchSpeed = 20;

    private PlayerMovement localPlayerMovement;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            StartCoroutine(WaitForLocalPlayer());
        }
    }

    private System.Collections.IEnumerator WaitForLocalPlayer()
    {
        while (localPlayerMovement == null)
        {
            foreach (var player in FindObjectsOfType<PlayerMovement>())
            {
                if (player.IsOwner)
                {
                    localPlayerMovement = player;
                    localPlayerTeam = localPlayerMovement.team.Value;
                    break;
                }
            }
            yield return null;
        }
    }

    void Update()
    {
        if (!IsOwner || localPlayerMovement == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = firePoint.position;
            Quaternion spawnRot = firePoint.rotation;
            Vector2 launchVelocity = firePoint.right * launchSpeed;

            FireRocketServerRpc(spawnPos, spawnRot, launchVelocity, localPlayerTeam);
        }
    }

    [ServerRpc]
    void FireRocketServerRpc(Vector3 position, Quaternion rotation, Vector2 velocity, int team)
    {
        GameObject rocket = Instantiate(bulletPrefab, position, rotation);
        var netObj = rocket.GetComponent<NetworkObject>();
        var rocketScript = rocket.GetComponent<Rocket>();

        // Set server-side logic
        rocketScript.shooterTeam = team;

        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;

        netObj.Spawn(true); // spawn for all clients
    }
}
