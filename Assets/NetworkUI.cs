using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host started");
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client started");
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        Debug.Log("Server started");
    }
}
