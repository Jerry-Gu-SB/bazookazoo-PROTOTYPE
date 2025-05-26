using UnityEngine;

public class AimAtMouse : MonoBehaviour
{
    public Transform playerCenter;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - playerCenter.position;
        transform.right = direction.normalized;
    }
}
