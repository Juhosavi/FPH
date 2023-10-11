using UnityEngine;
using Unity.Netcode;

public class ReplayPuck : NetworkBehaviour
{
    public GameObject[] puckArray;
    public GameObject[] playersInGame;
    public GameObject[] elaKoskeKiekkoon;
    public Transform fakePuck;
    public GameObject cameraFollow;
    public GameObject PuckFollowCamera;
    public GameObject puckToFollow;
    public Vector3 puckOffSet;
    //public GameObject replayPuckTrails;




    void Update()
    {
        puckArray = GameObject.FindGameObjectsWithTag("puck");
        //fakePucks = GameObject.FindGameObjectsWithTag("fakepuck");
        playersInGame = GameObject.FindGameObjectsWithTag("fakepuck");
        elaKoskeKiekkoon = GameObject.FindGameObjectsWithTag("elakoskekiekoon");

        // if (Input.GetKeyDown(KeyCode.P) && IsHost)
        // {

        // }

        if (!GetComponent<ActionReplayPuck>().isInReplayMode)
        {
            if (puckArray.Length == 0 && elaKoskeKiekkoon.Length == 0)
            {
                foreach (GameObject player in playersInGame)
                {

                    if (player.GetComponentInParent<PlayerMovement>().isPuckOnPlayerControl)
                    {
                        gameObject.transform.position = player.transform.position;
                        gameObject.transform.rotation = player.transform.rotation;
                    }
                }


            }
            else if (elaKoskeKiekkoon.Length != 0)
            {
                gameObject.transform.position = elaKoskeKiekkoon[0].transform.position;
                gameObject.transform.rotation = elaKoskeKiekkoon[0].transform.rotation;
            }
            else
            {
                gameObject.transform.position = puckArray[0].transform.position;
                gameObject.transform.rotation = puckArray[0].transform.rotation;
            }
        }

        //if (GetComponent<ActionReplayPuck>().isInReplayMode)
        //{
        //    replayPuckTrails.SetActive(true);

        //    if (puckArray.Length != 0)
        //    {

        //        puckArray[0].SetActive(false);
        //    }
        //    if (elaKoskeKiekkoon.Length != 0)
        //    {
        //        replayPuckTrails.SetActive(false);
        //        elaKoskeKiekkoon[0].SetActive(false);
        //    }
        //}
        //else
        //{
        //    replayPuckTrails.SetActive(false);
        //}


    }
    public void ResetPucks()
    {
        foreach (GameObject puck in puckArray)
        {
            Destroy(puck);
        }
        foreach (GameObject puck in elaKoskeKiekkoon)
        {
            Destroy(puck);
        }
        foreach (GameObject player in playersInGame)
        {
            if (player.GetComponentInParent<PlayerMovement>().isPuckOnPlayerControl)
            {
                player.GetComponentInParent<PlayerMovement>().isPuckOnPlayerControl = false;
                player.GetComponentInParent<PlayerMovement>().animator.SetBool("puckInPlayerControl", false);
                player.GetComponentInParent<PlayerMovement>().animator.SetBool("loadShotRight", false);
                player.GetComponentInParent<PlayerMovement>().animator.SetBool("shootPuckRight", false);
            }
        }
    }
}
