using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Netcode;


public class TimerController : NetworkBehaviour
{
    public static TimerController instance;
    public TextMeshPro timerText, timerText2;
    public float timer = 300.0f;
    //public bool isTimer = false;
    public NetworkVariable<bool> isTimer = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);

    //public NetworkVariable<bool> playReplay = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    public bool playReplay;
    public bool playReplayOnce;

    public GameObject playerCamera;
    public NetworkVariable<bool> endOfPeriod = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);

    void Start()
    {
        playReplayOnce = false;
        //StartTimer();
        instance = this;
        //isTimer.Value = false;
    }
    void Update()
    {
        HandleTimer();
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60.0f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText2.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void StartTimer()
    {
        playReplayOnce = false;
        playReplay = false;
        timerText.color = Color.green;
        timerText2.color = Color.green;
        if (IsServer)
        {
            isTimer.Value = true;
        }

    }
    public void StopTimer()
    {
        timerText.color = Color.red;
        timerText2.color = Color.red;
        if (IsServer)
        {
            isTimer.Value = false;
        }
        if (!playReplayOnce)
        {
            StartCoroutine(StartReplay());
            StartCoroutine(StopReplay());
            playReplayOnce = true;
        }

    }
    public void ResetTimer()
    {
        timer = 300.0f;
        DisplayTime();
        StartTimer();
    }
    public void EndOfPeriod()
    {
        if (IsServer)
        {
            endOfPeriod.Value = true;
        }
        timerText.ToString();
        timerText.color = Color.red;
        if (IsHost)
        {
            isTimer.Value = false;
        }
        StartCoroutine(ShowEndOfPeriod());

    }

    void HandleTimer()
    {
        if (isTimer.Value)
        {
            timer -= Time.deltaTime;
            DisplayTime();
        }

        if (timer <= 0f)
        {
            timerText.text = string.Format("00:00");
            timerText2.text = string.Format("00:00");
            EndOfPeriod();
        }
    }
    public IEnumerator StartReplay()
    {
        Debug.Log("StartAlko");
        yield return new WaitForSeconds(3);
        PuckManager.Instance.DestroyAllPucks();
        playReplay = true;
        Debug.Log("StartLoppu");
    }
    public IEnumerator StopReplay()
    {
        Debug.Log("StopAlko");
        yield return new WaitForSeconds(7);
        playReplay = false;
        playReplayOnce = false;


        Debug.Log("StopLoppu");
    }
    public IEnumerator ShowEndOfPeriod()
    {
        if (IsServer)
        {
            endOfPeriod.Value = true;
        }

        yield return new WaitForSeconds(3);
        playerCamera.gameObject.SetActive(false);
    }
}
