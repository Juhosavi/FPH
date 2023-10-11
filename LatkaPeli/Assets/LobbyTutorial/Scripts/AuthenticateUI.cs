using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AuthenticateUI : MonoBehaviour
{


    [SerializeField] private Button authenticateButton;
    public GameObject netManager;


    private void Awake()
    {
        authenticateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();

            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public void NetWorkManagerON()
    {
        netManager.SetActive(true);
    }

}