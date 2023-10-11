using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScript : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        // Nä rivit auttaa ettei pelaaja lähde kaatumaan
        // Laitetaan erikseen päälle ja kropalle
        // Ainakin vielä... Ehkä riittää pelkkä kroppa

        rb = GetComponentInParent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

}
