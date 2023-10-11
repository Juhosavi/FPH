using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Players : NetworkBehaviour
{
    //     public GameObject[] playersInGame;  
    //     public GameObject[] fakePucks;    
    //     public GameObject playerBody;

    //     void Update()
    //     {
    //        //FakePucksServerRpc();
    //     }
    //     [ServerRpc(RequireOwnership = false)]
    //     void FakePucksServerRpc()
    //     {        
    //             int index = 0;
    //             playersInGame = GameObject.FindGameObjectsWithTag("Player");
    //             fakePucks = GameObject.FindGameObjectsWithTag("fakepuck");
    //             foreach(GameObject player in playersInGame)
    //             {
    //             if(player.GetComponent<PlayerMovement>().isPuckOnPlayerControl == true)
    //             {
    //                 fakePucks[index].GetComponent<MeshRenderer>().enabled = true;
    //             }
    //             if(player.GetComponent<PlayerMovement>().isPuckOnPlayerControl == false)
    //             {
    //                 fakePucks[index].GetComponent<MeshRenderer>().enabled = false;
    //             }
    //             index++;
    //         } 
    //     }
}


// public Material[] playerMaterials;
//     [SerializeField]
//     private GameObject playerBody;

//     void Update()
//     {
//         if (IsLocalPlayer)
//         { 
//             if (Input.GetKeyDown(KeyCode.Alpha1))
//             {
//                 playerBody.GetComponent<MeshRenderer>().material = playerMaterials[1];
//             }
//             if (Input.GetKeyDown(KeyCode.Alpha2))
//             {
//                 playerBody.GetComponent<MeshRenderer>().material = playerMaterials[2];
//             }
//         }
//     }
//     public override void OnNetworkSpawn()
//     {
//         if(IsOwner)
//         {
//             playerBody.GetComponent<MeshRenderer>().material = playerMaterials[0];
//         }

//     }
