using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerState : NetworkBehaviour
{
    // Profile that the player is using
    [SyncVar] public int ProfileIndex;
	public SyncListInt SelectedAttributes;
    [SyncVar] public string username;

	// The number of times a player has cheated
	public int Cheated;
	public int Team;  
	public SyncListInt Route;

    [SerializeField]
	private Behaviour[] ComponentsToDisable;
    private Camera SceneCamera;

	public GameObject KeysHUD;
	[SerializeField]
	private GameObject ScoreBoard;
	public GameObject ScoreBoardInstance;

	private ServerLogic ServerLogic;

	void Awake() {
		SelectedAttributes = new SyncListInt();
		Route = new SyncListInt ();
		Cheated = 0;
	}

    void Start()
    {
		ServerLogic = GameObject.Find ("Game").GetComponent<ServerLogic> ();
		ServerLogic.RegisterPlayer(this);

        if (!isLocalPlayer)
        {
            foreach (Behaviour comp in ComponentsToDisable){
                comp.enabled = false;
            }
        }
        else
        {
            SceneCamera = Camera.main;
            if (SceneCamera != null)
            {
                SceneCamera.gameObject.SetActive(false);
            }
        }

        ScoreBoardInstance = Instantiate(ScoreBoard);
        setPlayerTag();
    }

    public void setPlayerTag()
    {
        this.gameObject.AddComponent<Tag3D>();
        this.gameObject.GetComponent<Tag3D>().tagText = username;
    }

	void Update(){
		if (Input.GetKeyDown ("v"))
			Debug.Log (Route [0] + ", " + Route [1] + ", " + Route [2] + ", " + Route [3]);
	}

    void OnDisable()
    {
        if (SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }
    }
	
	public Profile GetProfile() {
		return ServerLogic.Profiles[ProfileIndex];
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

		if (++theObject.GetComponent<UnlockableDoor> ().Counter == 3)
			NetworkManager.Destroy (theObject);
//			theObject.GetComponent<UnlockableDoor>().RpcSetActive(false);
	}

	[Command]
	// Networkinstance should be a door!
	public void CmdResetLocks(NetworkInstanceId doorNetID, NetworkInstanceId netID1, NetworkInstanceId netID2, NetworkInstanceId netID3)
    {
        System.Random rnd = new System.Random();

        GameObject door = NetworkServer.FindLocalObject(doorNetID);
		door.GetComponent<UnlockableDoor>().RpcSetActive(true);
		door.GetComponent<UnlockableDoor> ().Counter = 0;

		GameObject lock1 = NetworkServer.FindLocalObject(netID1);
		lock1.GetComponent<LockCube> ().Key = (ProfileAttribute) rnd.Next (1, 4);
		lock1.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock2 = NetworkServer.FindLocalObject(netID2);
		lock2.GetComponent<LockCube> ().Key = (ProfileAttribute) rnd.Next (1, 4);
		lock2.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock3 = NetworkServer.FindLocalObject(netID3);
		lock3.GetComponent<LockCube> ().Key = (ProfileAttribute) rnd.Next (1, 4);
		lock3.GetComponent<LockCube> ().RpcSetActive (true);
	}
}
