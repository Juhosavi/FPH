using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class FollowPuckCameraReplay : MonoBehaviour
{
    public TextMeshProUGUI playerHowScored;
    public GameObject replayHUD;
    public GameObject endOfPeriodHUD;
    //FMOD.Studio.EventInstance playerState;

    [SerializeField] float ObjectSpeed;
    [SerializeField] Transform[] positions;
    Transform NextPos;
    int NextPosIndex;

    public GameObject[] puck;
    public GameObject[] playersInGame;
    public GameObject cameraFollow;
    public GameObject PuckFollowCamera;
    private float puckZposition;
    //public float cameraZspeed;

    private int[] playersTouchedLast;
    //private int currentIndex;




    void Start()
    {
        playersTouchedLast = new int[3];
        //currentIndex = 0;

        replayHUD.SetActive(false);
        endOfPeriodHUD.SetActive(false);
        NextPos = positions[0];
    }

    void Update()
    {
        // if (!TimerController.instance.isTimer.Value)
        // {
        //     ShowPlayerHowScored();
        // }

        puck = GameObject.FindGameObjectsWithTag("ReplayPuck");
        playersInGame = GameObject.FindGameObjectsWithTag("Player");


        if (TimerController.instance.endOfPeriod.Value == false)
        {
            puckZposition = puck[0].transform.position.z;

            foreach (GameObject player in playersInGame)
            {
                if (player.GetComponent<PlayerMovement>().isPuckOnPlayerControl)
                {
                    cameraFollow = player;
                }
            }

            FollowPuck();
        }

        if (TimerController.instance.endOfPeriod.Value == true)
        {
            float distance = Vector3.Distance(gameObject.transform.position, NextPos.position);

            PuckFollowCamera.transform.LookAt(NextPos.transform.position);
            if (PuckFollowCamera.transform != NextPos)
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Game/EndOfPerioid", new Vector3(0, 0, 0));
                MoveGameObject();
            }
            else
            {
                //playerState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                PuckFollowCamera.transform.position = NextPos.transform.position;
                PuckFollowCamera.transform.rotation = NextPos.transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
                //Application.LoadLevel("MenuScene");
            }
            endOfPeriodHUD.SetActive(true);
        }
        if (TimerController.instance.playReplay == true)
        {
            replayHUD.SetActive(true);
        }
        else
        {
            replayHUD.SetActive(false);
        }
    }

    void MoveGameObject()
    {
        if (transform.position == NextPos.position)
        {
            NextPosIndex++;
            if (NextPosIndex >= positions.Length)
            {
                NextPosIndex = 0;
            }
            NextPos = positions[NextPosIndex];
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPos.position, ObjectSpeed * Time.deltaTime);
        }
    }

    void ShowPlayerHowScored()
    {
        foreach (GameObject player in playersInGame)
        {
            if (player.GetComponent<PlayerMovement>().isPuckOnPlayerControl)
            {
                //Debug.Log("Toimiiko tääpaska");
                //Debug.Log(player.GetComponent<PlayerSettings>().playerName);
                //playerHowScored.text = player.GetComponent<PlayerSettings>().playerName + " SCORED";
            }
            //else if(player.GetComponent<PlayerMovement>().isPuckOnPlayerControl)
            // Debug.Log(player.GetComponent<PlayerSettings>().playerName);
            // playerHowScored.text = player.GetComponent<PlayerSettings>().playerName +" SCORED";
            //playerHowScored.color = Color.blue;
        }
    }

    void FollowPuck()
    {
        if (puck.Length != 0)
        {
            cameraFollow = puck[0];
        }
        if (cameraFollow != null && TimerController.instance.endOfPeriod.Value == !true)
        {
            PuckFollowCamera.transform.LookAt(cameraFollow.transform.position);
        }
        if (puckZposition > gameObject.transform.position.z && TimerController.instance.endOfPeriod.Value == !true)
        {
            float cameraZspeed = puckZposition - gameObject.transform.position.z;
            transform.Translate(0, 0, 1 * cameraZspeed * Time.deltaTime, Space.World);
        }
        if (puckZposition < gameObject.transform.position.z && TimerController.instance.endOfPeriod.Value == !true)
        {
            float cameraZspeed = gameObject.transform.position.z - puckZposition;
            transform.Translate(0, 0, -1 * cameraZspeed * Time.deltaTime, Space.World);
        }
    }
}

