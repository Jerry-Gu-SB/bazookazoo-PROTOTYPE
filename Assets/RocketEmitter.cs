using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEmitter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * 10f;
        }
    }
}
