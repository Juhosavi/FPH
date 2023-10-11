using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    //public bool PuckOutOfPlay;
    public TimerController timerController;
    public NetworkVariable<bool> PuckOutOfPlay = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    //public bool homeScored;
    public NetworkVariable<bool> homeScored = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    //public bool awayScored;
    public NetworkVariable<bool> awayScored = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    public int homescore;
    public NetworkVariable<int> homescoreNetWork = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public int awayscore;
    public NetworkVariable<int> awayscoreNetWork = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public GameObject homeGoalLightAndAudio;
    public GameObject awayGoalLightAndAudio;
    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    [SerializeField]
    private TextMeshProUGUI playersCountText;
    [SerializeField]
    private TextMeshProUGUI homeScoreText;
    [SerializeField]
    private TextMeshProUGUI homeScoreText2;
    [SerializeField]
    private TextMeshProUGUI awaySccoreText;
    [SerializeField]
    private TextMeshProUGUI awaySccoreText2;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHomeScore(0);
        UpdateAwayScore(0);
        timerController = GameObject.Find("GameManager").GetComponent<TimerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (homeScored.Value)
        {
            TimerController.instance.StopTimer();
            homeGoalLightAndAudio.SetActive(true);
            StartCoroutine(FalseScoreValues());
            StartCoroutine(PlayHomeLightAndHorn());

        }
        if (awayScored.Value)
        {
            TimerController.instance.StopTimer();
            awayGoalLightAndAudio.SetActive(true);
            StartCoroutine(FalseScoreValues());
            StartCoroutine(PlayAwayLightAndHorn());

        }
        if (PuckOutOfPlay.Value)
        {
            TimerController.instance.StopTimer();
            StartCoroutine(OutOfPlayReset());

        }

        HandleDebugUI();
    }
    public void UpdateHomeScore(int scoreToAdd)
    {
        homescoreNetWork.Value += scoreToAdd;
    }
    public void UpdateAwayScore(int scoreToAdd)
    {
        awayscoreNetWork.Value += scoreToAdd;
    }
    public IEnumerator PlayHomeLightAndHorn()
    {
        yield return new WaitForSeconds(13);
        homeGoalLightAndAudio.SetActive(false);
        //homeScored.Value = false;
    }
    public IEnumerator PlayAwayLightAndHorn()
    {
        yield return new WaitForSeconds(13);
        awayGoalLightAndAudio.SetActive(false);
        //awayScored.Value = false;
    }
    public IEnumerator FalseScoreValues()
    {
        yield return new WaitForSeconds(5);
        if (IsHost)
        {
            homeScored.Value = false;
            awayScored.Value = false;
        }

    }
    public IEnumerator OutOfPlayReset()
    {
        yield return new WaitForSeconds(5);
        if (IsHost)
        {
            PuckOutOfPlay.Value = false;
        }

    }
    private void HandleDebugUI()
    {
        homeScoreText.text = "" + homescoreNetWork.Value.ToString();
        homeScoreText2.text = "" + homescoreNetWork.Value.ToString();
        awaySccoreText.text = "" + awayscoreNetWork.Value.ToString();
        awaySccoreText2.text = "" + awayscoreNetWork.Value.ToString();
        playersCountText.text = "Players: " + playersNum.Value.ToString();
        if (!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
