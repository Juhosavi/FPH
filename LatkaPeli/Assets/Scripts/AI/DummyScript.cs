using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public GameObject puck;
    public Rigidbody rb;
    public float moveForwardSpeed;
    public GameObject stickBottom;
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
            puck = GameObject.FindGameObjectWithTag("puck");
            transform.LookAt(puck.transform.position);
            rb.AddRelativeForce(Vector3.forward * moveForwardSpeed);

            distanceToPuck = Vector3.Distance(stickBottom.transform.position, puck.transform.position);

            if (distanceToPuck < 0.5)
            {
                puck.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                puck.transform.position = stickBottom.transform.position;
            }
        }
    }
}
