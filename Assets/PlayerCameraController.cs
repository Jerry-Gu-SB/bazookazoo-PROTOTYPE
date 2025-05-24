using UnityEngine;
using Unity.Netcode;

public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;

    void Start()
    {
        if (!IsOwner)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
    }
}
