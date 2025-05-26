using Unity.Netcode;
using UnityEngine;

public class AimAtMouse : NetworkBehaviour
{
    public Transform playerCenter;

    void Update()
    {
        if (!IsOwner) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - playerCenter.position;
        transform.right = direction.normalized;
    }
}
