using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class OffSideSystem : MonoBehaviour
{
    public GameObject puck;
    public GameObject homeZone;
    public GameObject awayZone;


    void Update()
    {
        if (PuckManager.Instance.pucksInGame.Length > 0)
        {
        puck = PuckManager.Instance.pucksInGame[0];
            
        }
    }

}

