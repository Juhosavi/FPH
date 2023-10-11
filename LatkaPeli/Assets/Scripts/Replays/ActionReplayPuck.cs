using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActionReplayPuck : NetworkBehaviour
{
    //public GameObject replayCamera;
    public bool isInReplayMode;
    //private float indexChangeRate;
    private int currentReplayIndex;
    //private Rigidbody rigidbody;
    private int maxLength;
    private bool isReplayStarted;

    public List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();

    public MeshRenderer replayPuckMesh;


    void Start()
    {
        isReplayStarted = false;
        maxLength = 420;
        //rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {//TimerController.instance.playReplay

        if (TimerController.instance.playReplay == true && !isInReplayMode)
        {
            isInReplayMode = !isInReplayMode;

            if (isInReplayMode)
            {
                SetTransform(currentReplayIndex);
                //rigidbody.isKinematic = true;
            }
            else
            {
                SetTransform(actionReplayRecords.Count - 1);
                //rigidbody.isKinematic = false;
            }
        }
        // if (Input.GetKeyDown(KeyCode.P) && IsHost)
        // {
        //     Debug.Log(actionReplayRecords.Count);
        //     actionReplayRecords.Clear();
        // }

    }
    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            isReplayStarted = false;
            actionReplayRecords.Add(new ActionReplayRecord { position = transform.position, rotation = transform.rotation });
            replayPuckMesh.enabled = false;
        }

        if (isInReplayMode)
        {
            replayPuckMesh.enabled = true;

            if (!isReplayStarted)
            {
                currentReplayIndex = actionReplayRecords.Count - maxLength;

                if (currentReplayIndex <= 0)
                {
                    currentReplayIndex = 0;
                }
                isReplayStarted = true;
                Debug.Log("Kiekon index count " + actionReplayRecords.Count);
                Debug.Log("Kiekon current index " + currentReplayIndex);
            }

            if (isReplayStarted)
            {
                if (currentReplayIndex < actionReplayRecords.Count - 1)
                {
                    SetTransform(currentReplayIndex);
                    currentReplayIndex++;
                }
                else
                {
                    //isReplayStarted = false;
                    isInReplayMode = false;
                    TimerController.instance.playReplay = false;
                    SetTransform(actionReplayRecords.Count - 1);
                }
            }
        }

    }
    private void SetTransform(int index)
    {
        //currentReplayIndex = index;
        //Debug.Log("Kiekon current index SetTransformissa " + index);
        if (index > 0 && index < actionReplayRecords.Count)
        {
            ActionReplayRecord actionReplayRecord = actionReplayRecords[index];

            transform.position = actionReplayRecord.position;
            transform.rotation = actionReplayRecord.rotation;
        }
    }
    private void HandleReplays()
    {

    }
}



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
//}


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

