using Unity.Netcode;
using UnityEngine;

public class AimAtMouse : NetworkBehaviour
{
    public Transform playerCenter;
    public Camera playerCamera; // Only for the local player

    // Sync rotation to other clients
    private NetworkVariable<Quaternion> syncedRotation = new NetworkVariable<Quaternion>(
        writePerm: NetworkVariableWritePermission.Owner);

    private void Update()
    {
        if (IsOwner)
        {
            if (playerCamera == null || playerCenter == null) return;

            Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            Vector3 direction = mousePos - playerCenter.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.right = direction.normalized;
                syncedRotation.Value = transform.rotation;
            }
        }
        else
        {
            // Apply the synced rotation
            transform.rotation = syncedRotation.Value;
        }
    }
}
