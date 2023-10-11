using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerMovement : NetworkBehaviour
{
    #region Variables
    public Animator animator;
    public NetworkAnimator networkAnimator; //Tarvitaan animaatio triggereiden lähettämiseen

    // Kropan liikkuminen
    public float strafeSpeed;
    public float moveForwardSpeed;
    public float moveBackSpeed;
    public float turnSpeed;
    float maxForward;
    public float ForwardBoostSpeed;
    public Rigidbody rb;
    public bool aiming;
    public GameObject playerCamera;

    // Head liikkuminen
    [SerializeField]
    private PlayerInput playerInput;
    public GameObject playerHead;
    public GameObject spineHead;
    public Transform stickBottom;
    float mouseX;
    float mouseY;
    public float gamePadSensitivity = 100f;
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float minAngle = -100f;
    public float maxAngle = 100;
    private bool lockHead;

    //Kiakko

    public float shootTimer;
    public float boostTimer;
    [SerializeField]
    private GameObject fakePuck;
    public GameObject puck;
    public float distanceToPuck;
    public float puckForce;
    public bool isPuckOnPlayerControl;
    public bool loadingShot;

    // Maila    
    public GameObject[] puckCatchers;
    public bool isStickOnPlayerControl;
    public bool playerIsOnPlayerControl;
    public bool faceOff;
    //private PlayerInfo playerInfo;

    #endregion
    #region Updates  

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerIsOnPlayerControl = false;
        isStickOnPlayerControl = false;

        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();

        isPuckOnPlayerControl = false;
        loadingShot = false;
        lockHead = true;
        animator.SetBool("faceoff", true);
        faceOff = true;

        fakePuck.GetComponent<MeshRenderer>().enabled = false;

    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
    void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            HandleMouseLook();
        }
    }
    void Update()
    {
        if (GetComponent<ActionReplay>().isInReplayMode && IsOwner)
        {
            playerCamera.gameObject.SetActive(false);
            //replayCamera.gameObject.SetActive(true);
        }
        else if (!GetComponent<ActionReplay>().isInReplayMode && IsOwner)
        {
            playerCamera.gameObject.SetActive(true);
            //replayCamera.gameObject.SetActive(false);
        }
        shootTimer += Time.deltaTime;

        //puckArray = GameObject.FindGameObjectsWithTag("puck");

        if (IsLocalPlayer)
        {
            PuckArrayHandler();

            if (faceOff)
            {
                HandleFaceOff();
            }
            else if (!faceOff && isStickOnPlayerControl)
            {
                PuckControl();
                PuckLost();
            }

            if (playerIsOnPlayerControl)
            {
                HandleMovement();
            }
            else
            {
                //animator.SetBool("faceoff", true);
            }
        }
    }
    void LateUpdate()
    {
        // Nämä LateUpdatessa, ettei sotke animaatiota
        if (IsLocalPlayer && isStickOnPlayerControl)
        {
            StickControl();
        }
    }
    #endregion
    #region Methods

    void HandleMovement()
    {
        // New Input systemin liikkuminen
        float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;
        float strafe = playerInput.actions["Strafe"].ReadValue<Vector2>().x;

        // Eteen ja taakse liikkuminen
        if (vertical != 0)
        {
            //Buustattu luistelu eteen
            if (playerInput.actions["Boost"].IsPressed() && vertical > 0 && !isPuckOnPlayerControl)
            {
                // Boostaus kesken. Jätetty testailun ajaksi näin
                if (boostTimer < 4)
                {
                    boostTimer += Time.deltaTime;
                }
                if (boostTimer < 2)
                {
                    animator.SetBool("skateForward", true);
                    rb.AddRelativeForce(Vector3.forward * ForwardBoostSpeed * vertical);
                }
                else
                {
                    if (boostTimer > 0)
                    {
                        boostTimer -= Time.deltaTime;
                    }
                    animator.SetBool("skateForward", true);
                    rb.AddRelativeForce(Vector3.forward * moveForwardSpeed * vertical);
                }
            }
            //Normaali luisteluvauhti
            else if (vertical > 0)
            {
                if (boostTimer > 0)
                {
                    boostTimer -= Time.deltaTime;
                }
                animator.SetBool("skateForward", true);
                rb.AddRelativeForce(Vector3.forward * moveForwardSpeed * vertical);
            }
            // Jos ei mennä eteenpäin, niin sitten taaksepäin
            else
            {
                // Tuohon joskus Skatebackward
                animator.SetBool("skateForward", true);
                rb.AddRelativeForce(Vector3.forward * moveBackSpeed * vertical);
                if (boostTimer > 0)
                {
                    boostTimer -= Time.deltaTime;
                }
            }

        }
        else
        {
            if (boostTimer > 0)
            {
                boostTimer -= Time.deltaTime;
            }
            animator.SetBool("skateForward", false);
        }

        // Strafen ohjaus
        if (strafe != 0)
        {
            rb.AddRelativeForce(Vector3.right * strafeSpeed * strafe);
        }
        if (horizontal != 0 && lockHead)
        {
            rb.AddRelativeForce(Vector3.right * strafeSpeed * horizontal);
        }
        // Kääntymisen ohjaus
        if (horizontal != 0 && !lockHead)
        {
            rb.AddTorque(Vector3.up * turnSpeed * horizontal);
        }


        if (rb.velocity.magnitude > maxForward)
        {
            rb.velocity = rb.velocity.normalized * maxForward;
        }

        if (!playerInput.actions["Boost"].IsPressed())
        {
            maxForward = moveForwardSpeed;
        }

    }
    void HandleMouseLook()
    {
        if (playerInput.actions["ChangeViewMode"].WasPressedThisFrame() && !lockHead)
        {
            lockHead = true;
        }
        else if (playerInput.actions["ChangeViewMode"].WasPressedThisFrame() && lockHead)
        {
            lockHead = false;
        }

        if (aiming == false)
        {
            // New input systemin head movement
            mouseX = playerInput.actions["Look"].ReadValue<Vector2>().x * gamePadSensitivity * Time.deltaTime;
            mouseY = playerInput.actions["Look"].ReadValue<Vector2>().y * gamePadSensitivity * Time.deltaTime;

            // Toistaiseksi mmousella vielä oma. Koska eri senset(Jos yhdistetään. Lisää mouse Delta player inputtiin)

            // mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            // mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //Clamppaa katsomisen pystysuunnassa
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, -45, 45);

            //Clamppaa katsomisen vaakasuunnassa
            rotationX += mouseX;
            rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

            if (!lockHead)
            {
                // Hiiren rullanapista vaihdettava katsomis mode
                playerHead.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
                spineHead.transform.localRotation = Quaternion.Euler(rotationY / 5, rotationX / 5, 0);
            }
            else if (lockHead)
            {
                // Hiiren rullanapista vaihdettava katsomis mode
                rotationX = 0;
                float horizontal = playerInput.actions["Look"].ReadValue<Vector2>().x;

                playerHead.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
                spineHead.transform.localRotation = Quaternion.Euler(rotationY / 5, 0, 0);

                rb.AddTorque(Vector3.up * turnSpeed * horizontal);
                //rb.AddTorque(Vector3.up * rotationX * 1);
            }
        }

        if (playerInput.actions["LookaAtPuck"].IsPressed())
        {
            // Kääntää pään kiekkoa kohti (Ei pystynyt Clampata jostain syystä. Ehkä LookaAt ei anna...)
            if (PuckManager.Instance.pucksInGame.Length != 0 && shootTimer > 1)
            {
                playerHead.transform.LookAt(PuckManager.Instance.pucksInGame[0].transform.position);
            }
        }

    }
    void PuckArrayHandler()
    {
        // Kiekko tarkastetaan Serverin puolelta pucksIngame arraysta
        if (PuckManager.Instance.pucksInGame.Length == 0 && !isPuckOnPlayerControl)
        {
            distanceToPuck = 50;
            animator.SetBool("puckInPlayerControl", false);
            return;
        }
        else if (PuckManager.Instance.pucksInGame.Length == 0 && isPuckOnPlayerControl)
        {
            distanceToPuck = 50;
            animator.SetBool("puckInPlayerControl", true);
        }
        else
        {
            animator.SetBool("puckInPlayerControl", false);
            animator.SetBool("loadShotRight", false);
            animator.SetBool("shootPuckRight", false);
            distanceToPuck = Vector3.Distance(stickBottom.transform.position, PuckManager.Instance.pucksInGame[0].transform.position);
        }
    }
    void PuckControl()
    {

        if (playerInput.actions["Shoot"].IsPressed() && !isPuckOnPlayerControl)
        {
            // Lämärin lataus            
            animator.SetBool("isLoadigShotRight", true);
            isPuckOnPlayerControl = false;
            shootTimer = 0;
            loadingShot = true;
        }
        else if (playerInput.actions["Shoot"].WasReleasedThisFrame() && distanceToPuck < 0.5f)
        {
            // Lämärin napin päästö. Lämäri lähtee jos kiekko tarpeeksi lähellä lapaa
            DestroyPucksServerRpc();

            animator.SetBool("isLoadigShotRight", false);

            ShootPuckServerRpc(puckForce * 1.3f);
            shootTimer = 0;
            isPuckOnPlayerControl = false;
        }
        else
        {
            animator.SetBool("isLoadigShotRight", false);
            loadingShot = false;
        }

        if (!isPuckOnPlayerControl && distanceToPuck < 0.3 && PuckManager.Instance.pucksInGame.Length != 0)
        {
            if (shootTimer > 1 && !loadingShot && !faceOff)
            {
                animator.SetFloat("Blend", 0);
                DestroyPucksServerRpc();
                isPuckOnPlayerControl = true;
            }
            else if (shootTimer > 1 && loadingShot)
            {

            }

        }
        if (isPuckOnPlayerControl)
        {
            if (playerInput.actions["Shoot"].IsPressed())
            {
                // Jos kiekko on hallussa. Lämärinappi pudottaa ensin kiekon eteen
                // Kiekolle annetaan pieni vauhti pelaajan vauhdin mukaan
                LosePuckServerRpc(rb.velocity, rb.velocity.magnitude / 3);

                animator.SetBool("isLoadigShotRight", true);
                isPuckOnPlayerControl = false;
                shootTimer = 0;
                loadingShot = true;
            }

            if (playerInput.actions["Pass"].WasPressedThisFrame() && shootTimer > 1)
            {
                // Syöttö
                networkAnimator.SetTrigger("passPuck");

                PassPuckServerRpc(Mathf.Abs(Mathf.Clamp(-rotationY, 0, 10) + puckForce / 4));
                shootTimer = 0;
                isPuckOnPlayerControl = false;
                networkAnimator.ResetTrigger("passPuck");


            }
            if (playerInput.actions["PassBack"].IsPressed() && shootTimer > 1)
            {
                // Syöttää kiekon taaksepäin
                networkAnimator.SetTrigger("passPuck");

                PassPuckServerRpc(-puckForce / 2);
                shootTimer = 0;
                isPuckOnPlayerControl = false;
                networkAnimator.ResetTrigger("passPuck");

            }
        }
    }
    void PuckLost()
    {
        // Jos kiekko on hallussa ja vastustaja yrittää  ottaa pois
        //  
        if (isPuckOnPlayerControl)
        {
            puckCatchers = GameObject.FindGameObjectsWithTag("puckcatch");
            foreach (GameObject puckCatcher in puckCatchers)
            {
                if (Vector3.Distance(stickBottom.transform.position, puckCatcher.transform.position) < 0.3)
                {
                    if (shootTimer > 1 && puckCatcher.activeInHierarchy == true)
                    {
                        LosePuckServerRpc(puckCatcher.transform.forward, puckForce / 4);
                        shootTimer = 0;
                        animator.SetBool("puckInPlayerControl", false);
                        animator.SetBool("isLoadigShotRight", false);
                        isPuckOnPlayerControl = false;
                    }
                }
            }
        }
    }
    void StickControl()
    {

        if (!isPuckOnPlayerControl)
        {
            if (playerInput.actions["Pass"].IsPressed() && shootTimer > 1)
            {
                //Animaatio "puckaway" laittaa animaation ajaksi puckcatcerin päälle
                //jolla saa kiekon pois toiselta

                networkAnimator.SetTrigger("puckawaySweep");
                animator.SetBool("puckInPlayerControl", false);
                shootTimer = 0;
            }
        }
    }
    void HandleFaceOff()
    {
        // Alotuksessa tapahtuvat asiat
        if (playerInput.actions["Shoot"].WasPerformedThisFrame() && shootTimer > 1)
        {
            networkAnimator.SetTrigger("faceOffHit");
            if (playerIsOnPlayerControl)
            {
                //faceOff = false;
            }
            shootTimer = 0;
        }
        if (distanceToPuck < 0.5f && PuckManager.Instance.pucksInGame.Length == 1 && shootTimer < 1)
        {
            DestroyPucksServerRpc();

            PassPuckServerRpc(-puckForce / 2);

            isPuckOnPlayerControl = false;
            // float randomX = Random.Range(-0.80f, 0.80f);
            // float randomY = Random.Range(-1.00f, 0.00f);
            // float randomZ = Random.Range(0.00f, 1.00f);
            // float randomFaceOffForce = Random.Range(5, 15);
            // Vector3 faceOffDirection = new Vector3(randomX, randomY, randomZ);

            // Debug.Log(faceOffDirection);
            // Debug.Log(randomFaceOffForce);

            // LosePuckServerRpc(faceOffDirection, randomFaceOffForce);
            // animator.SetBool("puckInPlayerControl", false);
            // animator.SetBool("isLoadigShotRight", false);
            //PassPuckServerRpc(-puckForce);
            //isPuckOnPlayerControl = false;

            animator.SetBool("faceoff", false);
            faceOff = false;
            shootTimer = 0;
        }

        //LosePuckServerRpc(Vector3.right, Random.Range(puckForce - 10, puckForce - 2));
        //faceOff = false;

    }
    void HandlePuckAtDangerZone()
    {
        if (fakePuck.GetComponent<FacekPuckDetector>().puckOutOfControl && shootTimer > 1)
        {
            // Kiekon menetys osuessa laitaan.
            // Tällä yritetään myös estää kiekon clippaus laidasta läpi
            LosePuckToCollision();
            fakePuck.GetComponent<FacekPuckDetector>().puckOutOfControl = false;
            animator.SetBool("puckInPlayerControl", false);
            isPuckOnPlayerControl = false;
        }
        else if (fakePuck.GetComponent<FacekPuckDetector>().puckOutOfControlGoalLine && shootTimer > 1)
        {
            // Kiekon menetys maaliviivalla            
            LosePuckAtGoalLine();
            fakePuck.GetComponent<FacekPuckDetector>().puckOutOfControlGoalLine = false;
            isPuckOnPlayerControl = false;
        }
        else
        {

        }
    }
    public void LosePuckToCollision()
    {
        if (IsLocalPlayer)
        {
            PassPuckServerRpc(-1f);

            animator.SetBool("puckInPlayerControl", false);
            animator.SetBool("isLoadigShotRight", false);
            isPuckOnPlayerControl = false;
            shootTimer = 0;
        }

    }
    public void LosePuckAtGoalLine()
    {
        if (IsLocalPlayer)
        {
            PassPuckServerRpc(puckForce / 4);

            animator.SetBool("puckInPlayerControl", false);
            animator.SetBool("isLoadigShotRight", false);
            isPuckOnPlayerControl = false;
            shootTimer = 0;
        }


    }
    #endregion
    #region RPC:s


    [ServerRpc(RequireOwnership = false)]
    void ShootPuckServerRpc(float power)
    {
        // Tällähetkellä lämy lähtee katseen suuntaan

        stickBottom.transform.rotation = playerHead.transform.rotation;

        // Allaoleva toimii jostain syystä vain Hostilla. Clienteillä ei kiekko nouse.
        // Korjataan syssymmällä....

        // float shootX = playerHead.transform.localRotation.x;
        // float shootY = playerHead.transform.localRotation.y;
        // float shootZ = playerHead.transform.localRotation.z;

        // shootY = shootY - 45f;
        // stickBottom.transform.localRotation = Quaternion.Euler(Mathf.Clamp(shootX, -45, 0), Mathf.Clamp(shootY, -90, 0), shootZ);
        // Quaternion lamari = new Quaternion(Mathf.Clamp(shootX, -45, 0), Mathf.Clamp(shootY, -90, 0), shootZ, 1);
        // stickBottom.transform.localRotation = lamari;
        //Debug.Log("Kiekon pysty akseli: " + shootX + "Kiekon pysty vaakaakseli : " + shootY);
        //  float shootX = stickBottom.transform.rotation.x;
        //  float shootY = stickBottom.transform.rotation.y;
        //  float shootZ = stickBottom.transform.rotation.z;

        //  stickBottom.transform.localRotation = new Quaternion(shootX, shootY, shootZ, 1);
        // Quaternion ShootPuckDirection = new Quaternion(shootX, shootY, shootZ, 1);


        //Quaternion.Euler(playerHead.transform.rotation.x, playerHead.transform.rotation.y, playerHead.transform.rotation.z);

        if (puck != null)
        {
            var abilityObject = (GameObject)Instantiate(puck, stickBottom.position, stickBottom.transform.rotation);

            abilityObject.GetComponent<NetworkObject>().Spawn();

            abilityObject.GetComponent<Rigidbody>().velocity = abilityObject.transform.forward * power;

        }
    }
    [ServerRpc]
    void PassPuckServerRpc(float power)
    {
        stickBottom.transform.rotation = playerHead.transform.rotation;
        // stickBottom.transform.localRotation = Quaternion.Euler(Mathf.Clamp(rotationY, -90, 0), Mathf.Clamp(rotationX, -30, 0), 0);
        // Debug.Log("Kiekon pysty akseli: " + stickBottom.transform.localRotation.x + "Kiekon pysty vaakaakseli : " + stickBottom.transform.localRotation.y);
        //  float shootX = stickBottom.transform.rotation.x;
        //  float shootY = stickBottom.transform.rotation.y;
        //  float shootZ = stickBottom.transform.rotation.z;

        //  stickBottom.transform.localRotation = new Quaternion(shootX, shootY, shootZ, 1);
        // Quaternion ShootPuckDirection = new Quaternion(shootX, shootY, shootZ, 1);


        //Quaternion.Euler(playerHead.transform.rotation.x, playerHead.transform.rotation.y, playerHead.transform.rotation.z);

        if (puck != null)
        {
            var abilityObject = (GameObject)Instantiate(puck, stickBottom.position, stickBottom.transform.rotation);

            abilityObject.GetComponent<NetworkObject>().Spawn();

            abilityObject.GetComponent<Rigidbody>().velocity = abilityObject.transform.forward * power;

        }
    }

    [ServerRpc]
    public void LosePuckServerRpc(Vector3 dir, float power)
    {
        if (puck != null)
        {
            var abilityObject = (GameObject)Instantiate(puck, stickBottom.position, Quaternion.identity);

            abilityObject.GetComponent<NetworkObject>().Spawn();

            abilityObject.GetComponent<Rigidbody>().velocity = dir * power;

        }
    }
    [ServerRpc(RequireOwnership = false)]
    void DestroyPucksServerRpc()
    {
        foreach (GameObject kiekko in PuckManager.Instance.pucksInGame)
        {
            kiekko.GetComponent<NetworkObject>().Despawn();
        }
    }
    [ServerRpc]
    void FakePucksServerRpc(bool onOff)
    {
        fakePuck.GetComponent<MeshRenderer>().enabled = onOff;
    }
    [ServerRpc(RequireOwnership = false)]
    void PlaySoundServerRpc(string soundPath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundPath, transform.position);
    }
    [ClientRpc]
    void PlaySoundClientRpc(string soundPath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundPath, transform.position);
    }
    #endregion
    #region Collision   
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BarrierCollider")
        {
            animator.SetFloat("Blend", 1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BarrierCollider")
        {
            animator.SetFloat("Blend", 0);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Other= " + other + " Impulse= " +
        // other.impulse + " Body= " + other.body + " RelativeVelocity= " +
        // other.relativeVelocity);
        if (other.gameObject.CompareTag("Player"))
        {
            // Vector3 collisionImpulse = other.impulse;
            // ulong id = other.gameObject.GetComponent<NetworkBehaviour>().OwnerClientId;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(other.impulse, ForceMode.Impulse);
            rb.AddForce(other.impulse, ForceMode.Impulse);
            //Debug.Log(id);
            //PlayerCollideServerRpc(id, OwnerClientId, collisionImpulse);
        }
    }
    #endregion
}