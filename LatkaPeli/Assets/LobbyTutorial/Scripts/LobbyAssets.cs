using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAssets : MonoBehaviour
{



    public static LobbyAssets Instance { get; private set; }


    [SerializeField] private Sprite marineSprite;
    [SerializeField] private Sprite ninjaSprite;
    [SerializeField] private Sprite zombieSprite;

    [SerializeField] private Sprite CenterSprite;
    [SerializeField] private Sprite RigtWingSprite;
    [SerializeField] private Sprite LeftWingSprite;
    [SerializeField] private Sprite RightDefenceSprite;
    [SerializeField] private Sprite LeftDefenceSprite;
    [SerializeField] private Sprite GoalieSprite;


    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSpriteTeam(LobbyManager.PlayerCharacter playerCharacter)
    {
        switch (playerCharacter)
        {
            default:
            case LobbyManager.PlayerCharacter.Marine: return marineSprite;
            case LobbyManager.PlayerCharacter.Ninja: return ninjaSprite;
            case LobbyManager.PlayerCharacter.Zombie: return zombieSprite;
        }
    }
    public Sprite GetSpritePosition(LobbyManager.PlayerCharacter playerCharacter)
    {
        switch (playerCharacter)
        {
            default:
            case LobbyManager.PlayerCharacter.Marine: return marineSprite;
            case LobbyManager.PlayerCharacter.Ninja: return ninjaSprite;
            case LobbyManager.PlayerCharacter.Zombie: return zombieSprite;
        }
    }

}