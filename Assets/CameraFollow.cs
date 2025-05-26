using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        // Find the local player's transform when they spawn
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                NetworkObject netObj = player.GetComponent<NetworkObject>();
                if (netObj != null && netObj.IsOwner)
                {
                    target = player.transform;
                    break;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position;
            newPos.z = transform.position.z; // Keep camera's Z
            transform.position = newPos;
        }
    }
}
