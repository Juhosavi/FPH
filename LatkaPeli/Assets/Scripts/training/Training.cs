using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Training : NetworkBehaviour
{
    private GameObject player;
    private GameObject rightHand;
    [SerializeField]
    private GameObject[] testiObjektiLista;
    [SerializeField]
    private GameObject[] offilleLaitettavatGameObjektit;
    [SerializeField]
    List<GameObject> m_SpawnPoints;
    [SerializeField]
    private GameObject puck;
    public Vector3 offSet;
    void Start()
    {

        Debug.Log("Morjensta!!! Nää on treenit");
        int eiKamaaPelaajanAlotusPisteeseen = 1;
        for (int i = 0; i < 11; i++)
        {

            Instantiate(testiObjektiLista[0], m_SpawnPoints[eiKamaaPelaajanAlotusPisteeseen].transform.position, Quaternion.identity);
            eiKamaaPelaajanAlotusPisteeseen++;

        }
        foreach (GameObject go in offilleLaitettavatGameObjektit)
        {
            go.SetActive(false);
        }


    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            rightHand = GameObject.FindGameObjectWithTag("RightHandThrowPoint");
            player.GetComponent<PlayerMovement>().playerIsOnPlayerControl = true;
            player.GetComponent<PlayerMovement>().isStickOnPlayerControl = true;
            player.GetComponent<PlayerMovement>().rb.isKinematic = false;
            player.GetComponent<PlayerMovement>().faceOff = false;
            player.GetComponent<PlayerMovement>().animator.SetBool("faceoff", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("puckInPlayerControl", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("loadShotRight", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("shootPuckRight", false);
            player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PuckManager.Instance.DestroyAllPucks();
            player.GetComponent<PlayerMovement>().isPuckOnPlayerControl = false;
            player.GetComponent<PlayerMovement>().loadingShot = false;
            player.GetComponent<PlayerMovement>().animator.SetTrigger("trainingThowPuck");
            player.GetComponent<PlayerMovement>().animator.SetBool("faceoff", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("puckInPlayerControl", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("loadShotRight", false);
            player.GetComponent<PlayerMovement>().animator.SetBool("shootPuckRight", false);
            player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);

            GameObject spawnedObjectTransform = Instantiate(puck, rightHand.transform.position, player.transform.localRotation);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            spawnedObjectTransform.GetComponent<Rigidbody>().velocity = player.transform.forward * 2 + player.transform.up * Random.Range(4.00f, 5.00f);
            spawnedObjectTransform.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(Random.Range(0.00f, 180.00f), 0, Random.Range(0.00f, 180.00f));
        }
    }
}
