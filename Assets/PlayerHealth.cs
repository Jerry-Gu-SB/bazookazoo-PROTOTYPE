using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>(100);

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;  // Only server changes health

        Health.Value = Mathf.Max(Health.Value - damage, 0);

        if (Health.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death logic here
        Debug.Log($"Player {OwnerClientId} has been killed.");
        // You might want to respawn the player or trigger a game over state
    }
}
