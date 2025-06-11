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
                    localPlayerTeam = player.team.Value;
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

            FireRocketServerRpc(spawnPos, spawnRot, launchVelocity, localPlayerTeam, NetworkObject.OwnerClientId);
        }
    }

    [ServerRpc]
    void FireRocketServerRpc(Vector3 position, Quaternion rotation, Vector2 velocity, int team, ulong shooterId)
    {
        GameObject rocket = Instantiate(bulletPrefab, position, rotation);
        var netObj = rocket.GetComponent<NetworkObject>();

        var rocketScript = rocket.GetComponent<Rocket>();

        rocketScript.shooterTeam.Value = team;
        rocketScript.shooterId.Value = shooterId;
        rocketScript.InitialVelocity.Value = velocity;

        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;

        // Pass shooter collider for collision ignore
        var shooterPlayer = NetworkManager.Singleton.ConnectedClients[shooterId].PlayerObject;
        if (shooterPlayer != null)
        {
            var shooterCollider = shooterPlayer.GetComponent<Collider2D>();
            if (shooterCollider != null)
            {
                rocketScript.SetShooterCollider(shooterCollider);
            }
        }

        netObj.Spawn(true);
    }

}