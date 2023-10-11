using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMove : MonoBehaviour
{

    [SerializeField] float objectSpeed;
    [SerializeField] Transform[] positions;
    Transform NextPos;
    int NextPosIndex;
    public GameObject mainMenuCamera;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, objectSpeed * Time.deltaTime, 0);

       

    }
    void MoveGameObject()
    {
        if (transform.position == NextPos.position)
        {
            NextPosIndex++;
            if (NextPosIndex >= positions.Length)
            {
                NextPosIndex = 0;
            }
            NextPos = positions[NextPosIndex];
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPos.position, objectSpeed * Time.deltaTime);
        }
    }
}
