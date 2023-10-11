using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ProjectSettings : NetworkBehaviour
{
    private PlayerInput playerInput;
    public GameObject puck;
    public GameObject orginalPuck;
    public Transform puckNettiKokeilu;
    public Transform puckDropCentre;
    public PuckDetector puckDetector;
    public GameManager gameManager;
    public TimerController timerController;
    public GameObject escMenu, optionsMenu;
    public CountDownController countDownController;
    public GameObject[] playersInGame;
    public GameObject replayPuck;
    public GameObject pauseFirstButton, settingsFirstButton, settingsClosedButton;


    private bool escMenuOn = false;

    void Start()
    {
        replayPuck = GameObject.FindGameObjectWithTag("ReplayPuck");
        countDownController = GameObject.Find("GameManager").GetComponent<CountDownController>();
        Application.targetFrameRate = 60;
        //Instantiate(orginalPuck, puckDropCentre);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timerController = GameObject.Find("GameManager").GetComponent<TimerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playersInGame = GameObject.FindGameObjectsWithTag("Player");


        if (Input.GetKeyDown(KeyCode.P) && IsHost)
        {
            Debug.Log("Kyllä se kiekko sieltä tippuu. Malttia!");
            FaceOff();

        }

        //if (playerInput.actions["EscMenu"].WasPressedThisFrame())
        if (Input.GetKeyDown(KeyCode.F1)) // || Input.GetKeyDown(Gamepad.jotain)
        {
            //EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);

            Resume();

            //Application.Quit();
        }
        //if (Input.GetKeyDown(KeyCode.Return) && escMenuOn)
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        //    //Application.Quit();
        //}

    }
    public IEnumerator PuckTimer()
    {
        timerController.playReplayOnce = false;

        yield return new WaitForSeconds(10);


        replayPuck.GetComponent<ActionReplayPuck>().actionReplayRecords.Clear();
        replayPuck.GetComponent<ReplayPuck>().ResetPucks();

        StartCoroutine(DropPuckTimer(Random.Range(5.00f, 6.00f)));
        StartCounterClientRpc();

        //Debug.Log("---------Puctimer loppu!!!!!!!!!!!!!");
    }
    public void FaceOff()
    {
        foreach (GameObject player in playersInGame)
        {
            player.GetComponent<PlayerMovement>().isStickOnPlayerControl = false;

        }

        StartCoroutine(PuckTimer());
    }
    [ClientRpc]
    public void ReleasePlayersClientRpc()
    {
        foreach (GameObject player in playersInGame)
        {
            // if (player.GetComponent<PlayerMovement>().playerIsOnPlayerControl == false)
            // {
            player.GetComponent<PlayerMovement>().playerIsOnPlayerControl = true;
            player.GetComponent<PlayerMovement>().isStickOnPlayerControl = true;
            player.GetComponent<PlayerMovement>().rb.isKinematic = false;
            StartCoroutine(ReleaseFaceOff(5));

            //player.GetComponent<ActionReplay>().rb.isKinematic = false;
            //}
        }
        foreach (GameObject player in playersInGame)
        {
            player.GetComponent<ActionReplay>().actionReplayRecords.Clear();
        }
    }
    [ClientRpc]
    void StartCounterClientRpc()
    {
        StartCoroutine(countDownController.CountdownToStart());
    }
    private IEnumerator DropPuckTimer(float sec)
    {
        foreach (GameObject player in playersInGame)
        {
            player.GetComponent<PlayerMovement>().faceOff = true;
            player.GetComponent<PlayerMovement>().animator.SetBool("faceoff", true);
            player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 1);
        }

        yield return new WaitForSeconds(sec);
        //Destroy(GameObject.FindGameObjectWithTag("puck"));
        Transform spawnedObjectTransform = Instantiate(puckNettiKokeilu, puckDropCentre);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        spawnedObjectTransform.GetComponent<Rigidbody>().velocity = -spawnedObjectTransform.transform.up * Random.Range(3.00f, 8.00f);
        spawnedObjectTransform.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(Random.Range(0.00f, 180.00f), 0, Random.Range(0.00f, 180.00f));

    }
    private IEnumerator ReleaseFaceOff(float sec)
    {
        yield return new WaitForSeconds(sec);

        foreach (GameObject player in playersInGame)
        {
            player.GetComponent<PlayerMovement>().faceOff = false;
            player.GetComponent<PlayerMovement>().animator.SetBool("faceoff", false);
            player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
        }
    }

    public void Resume()
    {
        if (escMenuOn)
        {
            //Jatketaan hommia tähän
            escMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = false;
            escMenuOn = false;
            EventSystem.current.SetSelectedGameObject(null);

        }
        else if (!escMenuOn)
        {

            escMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            escMenuOn = true;

            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        }

    }

    public void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        //SceneManager.UnloadSceneAsync("HockeyKit");
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        optionsMenu.SetActive(true);
        escMenu.SetActive(false);
        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void CloseSettings()
    {
        optionsMenu.SetActive(false);
        escMenu.SetActive(true);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(settingsClosedButton);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting Game");
    }


}
