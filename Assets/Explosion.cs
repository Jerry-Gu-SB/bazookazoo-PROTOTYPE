using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Explosion : MonoBehaviour

{
    public Rigidbody2D playerRigidBody;
    public Transform playerTransform;
    public Transform rocketTransform;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdfasdfsaf");
        if (other.CompareTag("Player")) {
            Vector2Field explosionEpicenter = new Vector2Field("Explosion");
            explosionEpicenter.value = new Vector2(rocketTransform.position.x, rocketTransform.position.y);

            Vector2Field playerLocation = new Vector2Field("Player");
            playerLocation.value = new Vector2(playerTransform.position.x, playerTransform.position.y);
            playerRigidBody.AddForce(playerLocation.value - explosionEpicenter.value);
        }
    }
}
