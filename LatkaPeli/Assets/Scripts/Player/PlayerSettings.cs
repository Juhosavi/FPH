using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using Unity.Services.Lobbies.Models;

//[System.Serializable]

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField]
    private MeshRenderer armsMesh;
    [SerializeField]
    private SkinnedMeshRenderer BodyMesh;
    [SerializeField]
    private TextMeshProUGUI playerNumber;
    [SerializeField]
    private TextMeshProUGUI playerJerseyName;
    [SerializeField]
    private NetworkVariable<FixedString128Bytes> networkPlayerNumber = new NetworkVariable<FixedString128Bytes>
    ("0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new NetworkVariable<FixedString128Bytes>
    ("0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public List<Color> colors = new List<Color>();

    public List<Material> materials = new List<Material>();

    [SerializeField]
    private List<GameObject> spawns = new List<GameObject>();
    //public GameObject[] spawns;
    [SerializeField]
    public int index;
    public int team; // 1 = home 2 = away
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        index = 0;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < 12; i++)
        {
            spawns.Insert(i, GameObject.Find("/-----Areena----------/SpawnPoints/Spawn (" + i + ")"));
        }
    }
    public override void OnNetworkSpawn()
    {
        rb.isKinematic = true;

        team = LobbyUI.teamNumber;
        index = LobbyUI.positionNumber;

        if (IsLocalPlayer)
        {
            if (index % 2 == 0 && team == 2)
            {
                index++;
            }
        }
        networkPlayerNumber.Value = (OwnerClientId + 1).ToString();
        playerNumber.text = networkPlayerNumber.Value.ToString();

        if (IsLocalPlayer)
        {
            Debug.Log("Joukkue: " + team + " Position Number " + index);
        }

    }

    private void LateUpdate()
    {
        if (GetComponent<PlayerMovement>().playerIsOnPlayerControl == false && IsOwner)
        {
            rb.isKinematic = true;
            UpdatePosition();
            UpdatePositionServerRpc();
            PlayerTeamSelect();
            // PlayerTeamSelect();
            // PlayerTeamSelectClientRpc();
        }
        else
        {
            //rb.isKinematic = false;
            //Debug.Log(playerName);
        }

    }
    void GetPlayerNames()
    {
        //Debug.Log(NetworkManager.Singleton.ConnectedClients);

        // foreach(GameObject player in NetworkManager.Singleton.ConnectedClients)
        // {

        // }
        //NetworkManager.Singleton.ConnectedClients.Count
    }

    public void SpawnToFaceOff()
    {
        rb.isKinematic = true;
        this.transform.position = spawns[index].transform.position;
        this.transform.rotation = spawns[index].transform.rotation;
    }
    private void UpdatePosition()
    {
        this.transform.position = spawns[index].transform.position;
        this.transform.rotation = spawns[index].transform.rotation;
    }
    [ServerRpc(RequireOwnership = false)]
    private void UpdatePositionServerRpc()
    {
        this.transform.position = spawns[index].transform.position;
        this.transform.rotation = spawns[index].transform.rotation;
    }
    [ClientRpc]
    private void PlayerTeamSelectClientRpc()
    {
        //playerJerseyName.text = PlayerInfo.playerName.ToString();
        PlayerTeamSelect();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerNameServerRpc()
    {
        networkPlayerName.Value = LobbyManager.Instance.playerName.ToString();
        playerJerseyName.text = networkPlayerName.Value.ToString();

        //networkPlayerNumber.Value = (OwnerClientId + 1).ToString();
        //playerNumber.text = networkPlayerNumber.Value.ToString();
    }

    private void PlayerTeamSelect()
    {
        if (team == 1)
        {
            animator.SetBool("home", true);
        }

        if (team == 2)
        {
            animator.SetBool("away", true);
        }

    }
    [ClientRpc]
    public void SetSpawnClientRpc(Vector3 position, ClientRpcParams clientRpcParams = default)
    {
        //m_CharacterController.enabled = false;
        transform.position = position;
        //m_CharacterController.enabled = true;
        gameObject.SetActive(true);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HomeOffsideTrigger")
        {
            Debug.Log("Ollaa Home puolella");
        }
        else if (other.gameObject.tag == "AwayOffsideTrigger")
        {
            Debug.Log("Ollaa Away puolella");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HomeOffsideTrigger")
        {
            Debug.Log("Ollaa Keskella");
        }
        else if (other.gameObject.tag == "AwayOffsideTrigger")
        {
            Debug.Log("Ollaa Keskella");
        }
    }

    //https://www.youtube.com/watch?v=HWPKlpeZUjM

}

