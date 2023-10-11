using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuckManager : NetworkBehaviour
{
    public static PuckManager Instance { get; private set; }
    public GameObject[] pucksInGame;
    public GameObject[] playersInGame;
    public GameObject[] fakePuckssInGame;
    public GameObject[] activeFakePucksInGame;
    public GameObject[] nonPlayablePucks;
    //public List<GameObject> activeFakePuckssInGame = new List<GameObject>();
    //public NetworkVariable<bool> isNewPuckSpawnable = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone);
    private void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        //isNewPuckSpawnable.Value = false;
    }
    private void Update()
    {
        pucksInGame = GameObject.FindGameObjectsWithTag("puck");
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        fakePuckssInGame = GameObject.FindGameObjectsWithTag("fakepuck");
        nonPlayablePucks = GameObject.FindGameObjectsWithTag("elakoskekiekoon");

        if (pucksInGame.Length > 0)
        {
            //isNewPuckSpawnable.Value = false;
        }
        else if (pucksInGame.Length > 1)
        {
            for (int i = 1; i < pucksInGame.Length; i++)
            {

                Destroy(pucksInGame[i]);
                Debug.Log("Ylimääräinen kiekko numero: " + i + " tuhottiin");
            }

        }
        else
        {
            //isNewPuckSpawnable.Value = true;
        }

    }

    public void DestroyAllPucks()
    {
        int kiekko = 0;
        int eiKiekko = 0;
        if (pucksInGame.Length > 0)
        {
            for (int i = 0; i < pucksInGame.Length; i++)
            {
                Destroy(pucksInGame[i]);

                kiekko++;
            }
        }
        if (nonPlayablePucks.Length > 0)
        {
            for (int i = 0; i < nonPlayablePucks.Length; i++)
            {
                Destroy(nonPlayablePucks[i]);

                eiKiekko++;
            }
        }
        Debug.Log("Kiekkoja tuhottiin: " + kiekko + " kappaletta");
        Debug.Log("Ei pelattavia kiekkoja tuhottiin: " + eiKiekko + " kappaletta");
    }

}
