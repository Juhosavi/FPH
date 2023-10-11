using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject puck;
    public GameObject stick;
    public GameObject stickBottom;

    public float distanceToObject;
    public float distanceToPuck;
    Transform puckTarget;

    public float puckForce;
    public bool isPuckOnPlayerControl;
    public bool ispuckPlayable;

    float mouseX;
    float mouseY;

    public float sensitivity = 100f;
    //public Transform player;
    public float rotationX = 0f;
    public float rotationY = 0f;

    public float minAngle = -45f;
    public float maxAngle = 45;



    void Start()
    {
        isPuckOnPlayerControl = false;
        ispuckPlayable = true;
        Cursor.lockState = CursorLockMode.Locked;
        puck = GameObject.FindGameObjectWithTag("puck");
    }

    void FixedUpdate()
    {
        HandleMouseLook();
        PuckControl();
    }

    void HandleMouseLook()
    {
        //t = 0.0f;

        if (playerMovement.aiming == false)
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, -30, 30);

            rotationX += mouseX;
            rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        }

        if (playerMovement.aiming == true)
        {

        }
        if (Input.GetMouseButton(1))
        {

            //stick.transform.localRotation = Quaternion.Euler(0, rotationX, 0);

        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            stick.transform.localRotation = Quaternion.Euler(0, 0, 0);


        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //transform.localRotation = Quaternion.Euler(0, 1, 0);

            transform.LookAt(puck.transform.position);
        }

    }
    void PuckControl()
    {
        puck = GameObject.FindGameObjectWithTag("puck");

        distanceToPuck = Vector3.Distance(stickBottom.transform.position, puck.transform.position);
        Vector3 mousePos = Input.mousePosition;

        puck.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (distanceToPuck < 0.5)
        {
            puck.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            puck.transform.position = stickBottom.transform.position;

            puck.transform.rotation = gameObject.transform.rotation;

            if (Input.GetMouseButton(0))
            {
                puck.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * puckForce, ForceMode.Impulse);
            }

            if (Input.GetMouseButton(1))
            {
                //stick.transform.localRotation = Quaternion.Euler(0, rotationX, 0);
                //puck.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up + Vector3.forward * puckForce / 2, ForceMode.Impulse);

                puck.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up + Vector3.forward * puckForce / 2, ForceMode.Impulse);

            }
        }
    }
}
