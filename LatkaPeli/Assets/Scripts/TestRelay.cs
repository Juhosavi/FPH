using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Instance { get; private set; }
    public GameObject buttonCanvas;
    public GameObject gameCanvas;
    public TMP_InputField inputCode;
    public TextMeshProUGUI hostCode;
    public string code;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        
        /*
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        */
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(11);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            hostCode.text = joinCode;

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }
    public async void JoinRelay(string joinCode)
    {

       
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);


            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
             

            

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }
    // void PlayerCharacterSettings()
    // {
    //     NetworkManager.Singleton.PrefabHandler.AddHandler()
    // }
    public void StarHostButton()
    {
       // CreateRelay();
        buttonCanvas.SetActive(false);
        gameCanvas.SetActive(true);

    }
    public void StarClientButton()
    {

        code = inputCode.text;
        JoinRelay(code);
        buttonCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }

}
