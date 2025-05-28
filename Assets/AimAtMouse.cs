using Unity.Netcode;
using UnityEngine;

public class AimAtMouse : NetworkBehaviour
{
    public Transform playerCenter;
    public Camera playerCamera; // Drag the player’s camera here in prefab

    private void Update()
    {
        if (!IsOwner || playerCamera == null) return;

        Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - playerCenter.position;
        transform.right = direction.normalized;
    }
}
