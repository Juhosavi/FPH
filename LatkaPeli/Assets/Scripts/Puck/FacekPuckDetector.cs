using UnityEngine;
using Unity.Netcode;

public class FacekPuckDetector : NetworkBehaviour
{
    private float collisionCooldownTimer = 1;
    private GameObject player;
    public GameManager gameManager;
    public bool puckOutOfControl, puckOutOfControlGoalLine;
    //public ulong owner;

    private void Start()
    {
        collisionCooldownTimer = 1;
        puckOutOfControl = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        collisionCooldownTimer += Time.deltaTime;

        if (GetComponent<MeshRenderer>().enabled == true)
        {

        }
        else
        {

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter");

        if (GetComponent<MeshRenderer>().enabled == true && IsLocalPlayer)
        {
            if (other.gameObject.tag == "HomeGoalDetector" && gameManager.homeScored.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                player.GetComponent<PlayerMovement>().LosePuckAtGoalLine();
                //player.GetComponent<PlayerMovement>().faceOff = true;
                //puckOutOfControlGoalLine = true;
                //gameObject.tag = "elakoskekiekoon";

            }
            else if (other.gameObject.tag == "AwayGoalDetector" && gameManager.awayScored.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                player.GetComponent<PlayerMovement>().LosePuckAtGoalLine();
                //player.GetComponent<PlayerMovement>().faceOff = true;
                //puckOutOfControlGoalLine = true;
                //gameObject.tag = "elakoskekiekoon";

            }
            else if (other.gameObject.tag == "Out of Play" && gameManager.PuckOutOfPlay.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                player.GetComponent<PlayerMovement>().LosePuckToCollision();
            }

            if (other.gameObject.tag == "Barrier")
            {
                //player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 1);
                if (collisionCooldownTimer > 3)
                {
                    player.GetComponent<PlayerMovement>().LosePuckToCollision();
                    //player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
                    collisionCooldownTimer = 0;
                }
            }
            else if (other.gameObject.tag == "tolppa")
            {
                //player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 1);
                if (collisionCooldownTimer > 3)
                {
                    player.GetComponent<PlayerMovement>().LosePuckToCollision();
                    //player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
                    collisionCooldownTimer = 0;
                }
            }

            else
            {
                player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
            }
        }
        if (other.gameObject.tag == "Barrier" || other.gameObject.tag == "tolppa" && player.GetComponent<PlayerMovement>().isPuckOnPlayerControl)
        {
            //player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 1);
        }
        // else
        // {
        //     player.GetComponent<PlayerMovement>().animator.SetFloat("Blend", 0);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);

        if (collision.gameObject.tag == "ice")
        {
            //Debug.Log("ICE");
            if (collision.relativeVelocity.magnitude > 1)
            {

                PlaySound("event:/Kiekko/Ice/Ice");

            }
        }

        if (collisionCooldownTimer > 0.5)
        {
            if (collision.gameObject.tag == "Barrier")
            {
                puckOutOfControl = true;
                if (collision.relativeVelocity.magnitude > 18)
                {
                    Debug.Log("Kova barrier");
                    PlaySound("event:/Kiekko/Reuna/ReunaKova");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Reuna/ReunaKova", transform.position);
                }
                else if (collision.relativeVelocity.magnitude <= 18 && collision.relativeVelocity.magnitude >= 8)
                {
                    Debug.Log("Medium barrier");
                    PlaySound("event:/Kiekko/Reuna/ReunaMedium");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Reuna/ReunaMedium", transform.position);
                }
                if (collision.relativeVelocity.magnitude < 8)
                {
                    Debug.Log("Hiljainen barrier");
                    PlaySound("event:/Kiekko/Reuna/ReunaHiljaa");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Reuna/ReunaHiljaa", transform.position);
                }
            }
            else if (collision.gameObject.tag == "tolppa")
            {
                puckOutOfControl = true;
                //Debug.Log("Tolppa velocity = " + collision.relativeVelocity.magnitude);
                //Debug.Log(collision.gameObject.name + "  " + collision.relativeVelocity.magnitude);
                if (collision.relativeVelocity.magnitude > 18)
                {
                    PlaySound("event:/Kiekko/Tolppa/TolppaKova");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Tolppa/TolppaKova", transform.position);
                }
                else if (collision.relativeVelocity.magnitude <= 18 && collision.relativeVelocity.magnitude >= 8)
                {
                    PlaySound("event:/Kiekko/Tolppa/TolppaMedium");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Tolppa/TolppaMedium", transform.position);
                }
                else if (collision.relativeVelocity.magnitude < 8)
                {
                    PlaySound("event:/Kiekko/Tolppa/TolppaHiljaa");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Tolppa/TolppaHiljaa", transform.position);
                }
            }
            else if (collision.gameObject.tag == "glass")
            {
                if (collision.relativeVelocity.magnitude >= 18)
                {
                    PlaySound("event:/Kiekko/Lasi/GlassHitKova");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Lasi/GlassHitKova", transform.position);
                }
                else if (collision.relativeVelocity.magnitude <= 18 && collision.relativeVelocity.magnitude >= 8)
                {
                    PlaySound("event:/Kiekko/Lasi/GlassHitMedium");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Lasi/GlassHitMedium", transform.position);
                }
                else if (collision.relativeVelocity.magnitude < 8)
                {
                    PlaySound("event:/Kiekko/Lasi/GlassHitHiljaa");
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Kiekko/Lasi/GlassHitHiljaa", transform.position);
                }
            }
            else
            {
                puckOutOfControl = false;
            }
        }
        collisionCooldownTimer = 0;
    }
    void PlaySound(string sound)
    {
        PlaySoundClientRpc(sound);
        PlaySoundServerRpc(sound);
    }
    [ClientRpc]
    void PlaySoundClientRpc(string sound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
    }
    [ServerRpc(RequireOwnership = false)]
    void PlaySoundServerRpc(string sound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
    }
}
