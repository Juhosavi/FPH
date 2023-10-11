using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActionReplay : NetworkBehaviour
{
    public GameObject replayCamera;
    public bool isInReplayMode;
    private float indexChangeRate;
    private int currentReplayIndex;
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;
    private int maxLength;
    private bool isReplayStarted;
    public List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();

    //public GameObject replayPuck;


    //uutta paskaa

    void Start()
    {
        isReplayStarted = false;
        maxLength = 420;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (TimerController.instance.playReplay == true && !isInReplayMode)
        {
            isInReplayMode = !isInReplayMode;

            if (isInReplayMode)
            {
                //GetComponent<PlayerMovement>().animator.SetBool("skateForward", true);
                SetTransform(currentReplayIndex);
                rb.isKinematic = true;

            }

            // else
            // {
            //     SetTransform(actionReplayRecords.Count - 1);
            //     rb.isKinematic = false;
            // }
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
            //replayPuck.SetActive(false);
        }

        if (isInReplayMode)
        {
            if (!isReplayStarted)
            {
                currentReplayIndex = actionReplayRecords.Count - maxLength;

                if (currentReplayIndex <= 0)
                {
                    currentReplayIndex = 0;
                }

                //Debug.Log("ReplayIndex = " + currentReplayIndex);
                isReplayStarted = true;

                Debug.Log("Pelaajan index count " + actionReplayRecords.Count);
                Debug.Log("Pelaajan current index " + currentReplayIndex);
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

                    TimerController.instance.playReplay = false;
                    isInReplayMode = false;
                    if (IsLocalPlayer)
                    {
                        GetComponent<PlayerSettings>().SpawnToFaceOff();

                    }
                    //SetTransform(actionReplayRecords.Count - 1);
                    if (!IsLocalPlayer)
                    {
                        rb.isKinematic = false;
                    }
                }

            }
        }
    }
    private void SetTransform(int index)
    {
        //currentReplayIndex = index;
        if (index > 0 && index < actionReplayRecords.Count)
        {
            ActionReplayRecord actionReplayRecord = actionReplayRecords[index];
            transform.position = actionReplayRecord.position;
            transform.rotation = actionReplayRecord.rotation;
        }
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

