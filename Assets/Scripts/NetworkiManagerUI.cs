using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkiManagerUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        canvas.enabled = false;
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        canvas.enabled = false;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        canvas.enabled = false;
    }
}
