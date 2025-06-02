using TMPro;
using UnityEngine;
using Unity.Netcode;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private PlayerHealth localPlayerHealth;

    void Start()
    {
        // Find the local player once they're spawned
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    void OnClientConnected(ulong clientId)
    {
        // Look for local player's health
        foreach (var player in FindObjectsOfType<PlayerHealth>())
        {
            if (player.IsOwner)
            {
                localPlayerHealth = player;
                break;
            }
        }
    }

    void Update()
    {
        if (localPlayerHealth != null)
        {
            healthText.text = "Health: " + localPlayerHealth.Health.Value.ToString();
        }
    }
}
