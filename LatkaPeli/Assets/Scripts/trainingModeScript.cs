using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class trainingModeScript : MonoBehaviour
{
    public GameObject multiplayerCanvas;
    public GameObject multiplayerManager;
    public GameObject lobbyManager;
    public GameObject testRelay;
    public GameObject trainingModeScripti;


    public GameObject trainingManager;

    public void TrainingButton()
    {
        testRelay.SetActive(false);
        lobbyManager.SetActive(false);
        multiplayerCanvas.SetActive(false);
        multiplayerManager.SetActive(false);
        trainingManager.SetActive(true);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        trainingModeScripti.SetActive(true);
    }

}
