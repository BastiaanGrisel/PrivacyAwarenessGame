using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player : NetworkBehaviour
{
    // Player
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Rigidbody rb;
    private Camera sceneCamera;
    public GameObject HealthUI;
    public SyncListInt keys;
    private System.Random rnd;
    private ServerLogic serverLogic;

    // Editor Fields
    [SerializeField] private Behaviour[] componetsToDisable;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 3f;

    void Start()
    {
        keys = new SyncListInt();
        rnd = new System.Random();
        serverLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
        rb = GetComponent<Rigidbody>();

        if (!isLocalPlayer)
        {
            foreach (Behaviour comp in componetsToDisable)
            {
                comp.enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            GameObject ui = Instantiate(HealthUI);
            for (int i = 0; i < keys.Count; i++)
            {
                ui.GetComponent<HUDKeys>().KeyTexts[i].text = keys[i].ToString();
            }
        }
    }

    void Awake()
    {

    }

    void Update()
    {
        if (serverLogic.gameStarted)
        {
            // Calculate velocity as a 3D vector
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            Vector3 movHorizontal = transform.right * xMov;
            Vector3 movVertical = transform.forward * zMov;

            Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

            // Apply movement
            Move(velocity);
        }
        else
        {
            Vector3 velocity = Vector3.zero;
            Move(velocity);
        }

        // Calculate rotation as a 3D vector (turning around)
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        // Apply rotation
        Rotate(rotation);

        // Calculate camera rotation as a 3D vector (turning around)
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * mouseSensitivity;

        // Apply camera rotation
        RotateCamera(cameraRotation);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Trophy" && ((Trophy)c.gameObject.GetComponent<Trophy>()).isWinningTrophy)
        {
            c.gameObject.SetActive(false);
            if (GameObject.FindGameObjectsWithTag("Trophy").Length == 0)
                this.CmdEndGame();
        }
    }

    /*
     * Commands to the Server
     */ 

    [Command]
    public void CmdEndGame()
    {
        serverLogic.gameStarted = false;
    }

    [Command]
    public void CmdDestroyLockCube(NetworkInstanceId netID)
    {
        GameObject theObject = NetworkServer.FindLocalObject(netID);
        theObject.GetComponent<LockCube>().RpcSetActive(false);
    }

    [Command]
    public void CmdIncrementCounter(NetworkInstanceId netID)
    {
        GameObject theObject = NetworkServer.FindLocalObject(netID);

        if (++theObject.GetComponent<UnlockableDoor>().counter == 3)
            theObject.GetComponent<UnlockableDoor>().RpcSetActive(false);
    }

    [Command]
    // Networkinstance should be a door!
    public void CmdResetLocks(NetworkInstanceId doorNetID, NetworkInstanceId netID1, NetworkInstanceId netID2, NetworkInstanceId netID3)
    {
        GameObject door = NetworkServer.FindLocalObject(doorNetID);
        door.GetComponent<UnlockableDoor>().RpcSetActive(true);
        door.GetComponent<UnlockableDoor>().counter = 0;

        GameObject lock1 = NetworkServer.FindLocalObject(netID1);
        lock1.GetComponent<LockCube>().Key = rnd.Next(1, 4);
        lock1.GetComponent<LockCube>().RpcSetActive(true);

        GameObject lock2 = NetworkServer.FindLocalObject(netID2);
        lock2.GetComponent<LockCube>().Key = rnd.Next(1, 4);
        lock2.GetComponent<LockCube>().RpcSetActive(true);

        GameObject lock3 = NetworkServer.FindLocalObject(netID3);
        lock3.GetComponent<LockCube>().Key = rnd.Next(1, 4);
        lock3.GetComponent<LockCube>().RpcSetActive(true);
    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    // Gets a movement vector
    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    // Gets a rotation vector
    public void Rotate(Vector3 rotation)
    {
        this.rotation = rotation;
    }

    // Gets a rotation vector for the camera
    public void RotateCamera(Vector3 cameraRotation)
    {
        this.cameraRotation = cameraRotation;
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    // Perform movement based on velocity veriable
    void PerformMovement()
    {
        if (!velocity.Equals(Vector3.zero))
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (Camera.main != null)
        {
            Camera.main.transform.Rotate(-cameraRotation);
        }
    }
}
