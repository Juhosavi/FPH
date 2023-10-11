using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuckPhysics : NetworkBehaviour
{
    public Rigidbody rb;    
    
    
    [ClientRpc]
    public void ShootPuckClientRpc(float force)
    {
        
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);

    }
    
}
