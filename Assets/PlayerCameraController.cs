using UnityEngine;
using Unity.Netcode;

public class PlayerCameraController : NetworkBehaviour
{
    private Camera playerCam;

    private void Awake()
    {
        playerCam = GetComponentInChildren<Camera>(true); // Find even if disabled
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Enable camera only for the local player
            playerCam.gameObject.SetActive(true);
            playerCam.tag = "MainCamera";
        }
        else
        {
            // Disable other players' cameras
            if (playerCam != null)
                playerCam.gameObject.SetActive(false);
        }
    }
}
