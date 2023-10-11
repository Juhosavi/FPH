using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMolari : MonoBehaviour
{
    public Rigidbody rb;
    public float moveForwardSpeed;
    public GameObject puck;

    public GameObject basicPosition;
    public GameObject holdPuckPosition;
    public float distanceToPuck;
    public GameManager gameManager;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.PuckOutOfPlay.Value == false && gameManager.homeScored.Value == false && gameManager.awayScored.Value == false)
        {
            if (GameObject.FindGameObjectWithTag("puck") != null)
            {
                puck = GameObject.FindGameObjectWithTag("puck");
                transform.LookAt(puck.transform.position);
            }

            /*
             if (distanceToPuck < 0.5)
            {
                rb.AddRelativeForce(Vector3.forward * moveForwardSpeed);
               puck.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

               puck.transform.position = holdPuckPosition.transform.position;
            }
            */

        }
    }
}
