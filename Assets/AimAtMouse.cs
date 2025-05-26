using Unity.Netcode;
using UnityEngine;

public class AimAtMouse : NetworkBehaviour
{
    public Transform playerCenter;

    private Camera mainCam;

    private void Start()
    {
        if (!IsOwner) return;

        // Cache the local camera once at the start
        mainCam = Camera.main;
    }

    void Update()
    {
        if (!IsOwner || mainCam == null) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - playerCenter.position;
        transform.right = direction.normalized;
    }
}
