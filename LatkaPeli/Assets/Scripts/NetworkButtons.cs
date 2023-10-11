using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class NetworkButtons : MonoBehaviour
{
    public bool startServer = false;


    void Start()
    {
        NetworkManager.Singleton.StartHost();
        if (startServer == true)
        {
            NetworkManager.Singleton.StartServer();

        }
    }
    private void OnGUI()
    {

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host"))
            {
                NetworkManager.Singleton.StartHost();
            }

            if (GUILayout.Button("Server"))
            {
                NetworkManager.Singleton.StartServer();

            }
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.StartClient();
            }

        }

        GUILayout.EndArea();


    }
    // [ServerRpc(RequireOwnership=false)] //server owns this object but client can request a spawn
    //     public void SpawnPlayerServerRpc(ulong clientId,int prefabId) {
    //         GameObject newPlayer;
    //         if (prefabId==0)
    //              newPlayer=(GameObject)Instantiate(playerPrefabA);
    //         else
    //             newPlayer=(GameObject)Instantiate(playerPrefabB);
    //         netObj=newPlayer.GetComponent<NetworkObject>();
    //         newPlayer.SetActive(true);
    //         netObj.SpawnAsPlayerObject(clientId,true);
    //     }

    // private void Awake() {
    //     GetComponent<UnityTransport>().SetDebugSimulatorParameters(
    //         packetDelay: 120,
    //         packetJitter: 5,
    //         dropRate: 3);
    // }
}