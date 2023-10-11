using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyPlayerSingleUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image PositionImage;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private Button playerPositionButtonC;
    [SerializeField] private Button playerPositionButtonRW;
    [SerializeField] private Button playerPositionButtonLW;
    [SerializeField] private Button playerPositionButtonRD;
    [SerializeField] private Button playerPositionButtonLD;
    [SerializeField] private Button playerPositionButtonG;
    //public static int playerPosition;



    private Player player;


    private void Awake()
    {


        kickPlayerButton.onClick.AddListener(KickPlayer);

        playerPositionButtonC.onClick.AddListener(PositionC);
        playerPositionButtonRW.onClick.AddListener(PositionRW);
        playerPositionButtonLW.onClick.AddListener(PositionLW);
        playerPositionButtonRD.onClick.AddListener(PositionRD);
        playerPositionButtonLD.onClick.AddListener(PositionLD);
        playerPositionButtonG.onClick.AddListener(PositionG);
    }

    public void SetKickPlayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        LobbyManager.PlayerCharacter playerCharacter =
            System.Enum.Parse<LobbyManager.PlayerCharacter>(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);
        characterImage.sprite = LobbyAssets.Instance.GetSpriteTeam(playerCharacter);
        //PositionImage.sprite = LobbyAssets.Instance.GetSpritePosition(playerCharacter);
    }

    private void KickPlayer()
    {
        if (player != null)
        {
            LobbyManager.Instance.KickPlayer(player.Id);
        }
    }
    private void PositionC()
    {
        LobbyUI.positionNumber = 0;

    }
    private void PositionRW()
    {
        LobbyUI.positionNumber = 2;

    }
    private void PositionLW()
    {
        LobbyUI.positionNumber = 4;

    }
    private void PositionRD()
    {
        LobbyUI.positionNumber = 6;

    }
    private void PositionLD()
    {
        LobbyUI.positionNumber = 8;

    }
    private void PositionG()
    {
        LobbyUI.positionNumber = 10;

    }



}