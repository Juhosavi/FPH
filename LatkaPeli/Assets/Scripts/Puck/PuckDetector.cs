using UnityEngine;
using Unity.Netcode;
using FMODUnity;

public class PuckDetector : NetworkBehaviour
{
    //public int HomeTeamScore;
    //public NetworkVariable<int> HomeTeamScore = new NetworkVariable<int>();
    //public int AwayTeamScore;
    //public NetworkVariable<int> AwayTeamScore = new NetworkVariable<int>();
    public ProjectSettings project;
    public GameManager gameManager;
    private float collisionCooldownTimer = 1;
    //public ulong owner;

    private void Start()
    {
        collisionCooldownTimer = 1;
        project = GameObject.Find("ProjectSettings").GetComponent<ProjectSettings>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (IsServer)
        {
            gameManager.homeScored.Value = false;
            gameManager.awayScored.Value = false;
            gameManager.PuckOutOfPlay.Value = false;
        }
    }

    private void Update()
    {
        collisionCooldownTimer += Time.deltaTime;
        //gameObject.GetComponent<NetworkObject>().ChangeOwnership(owner);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (IsServer)
        {
            if (other.gameObject.tag == "AwayGoalDetector" && gameManager.homeScored.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                gameObject.tag = "elakoskekiekoon";
                gameManager.UpdateHomeScore(1);
                project.FaceOff();
                gameManager.homeScored.Value = true;
            }

            if (other.gameObject.tag == "HomeGoalDetector" && gameManager.awayScored.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                gameObject.tag = "elakoskekiekoon";
                gameManager.UpdateAwayScore(1);
                project.FaceOff();
                gameManager.awayScored.Value = true;
            }
            if (other.gameObject.tag == "Out of Play" && gameManager.PuckOutOfPlay.Value == false && TimerController.instance.endOfPeriod.Value == !true)
            {
                gameObject.tag = "elakoskekiekoon";
                PlaySound("event:/Game/Whistle");
                project.FaceOff();
                gameManager.PuckOutOfPlay.Value = true;

                // gameObject.tag = "elakoskekiekoon";
                // Debug.Log("Puck out of play");
                // gameManager.UpdateAwayScore(1);
                // project.FaceOff();
                // gameManager.PuckOutOfPlay.Value = true;

                //
                //StartCoroutine(project.PuckTimer());
            }

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
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
            if (collision.gameObject.tag == "tolppa")
            {
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
            if (collision.gameObject.tag == "glass")
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
