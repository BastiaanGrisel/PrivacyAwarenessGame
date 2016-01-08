using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PlayerState : NetworkBehaviour
{
    // Profile that the player is using
    [SyncVar] public int ProfileIndex;
	public SyncListInt SelectedAttributes = new SyncListInt();
    [SyncVar] public string username;

    public bool freeze = false;

    private List<KeyValuePair<ProfileAttribute, string>> collectedData = new List<KeyValuePair<ProfileAttribute, string>>();
    private int doorResetTime = 10;

	// The number of times a player has cheated
	public int Cheated;
	public int Team;  
	public List<int> Route = new List<int>();

    [SerializeField]
    private Behaviour[] ComponentsToDisable;
    private Camera SceneCamera;

    public GameObject KeysHUD;
    [SerializeField]
    private GameObject ScoreBoard;
    public GameObject ScoreBoardInstance;
    [SerializeField]
    private GameObject RouteUI;
    public GameObject RouteUIInstance;

    private ServerLogic ServerLogic;

    void Awake()
    {
		Cheated = 0;
		Enumerable.Range(0,4).OrderBy(r => UnityEngine.Random.value).ToList().ForEach(r => Route.Add(r));
	}

	public override void OnStartClient() {
		SelectedAttributes.Callback = OnSelectedAttributedChanged;
	}

    void Start()
    {
        ServerLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
        ServerLogic.RegisterPlayer(this);
        ScoreBoardInstance = Instantiate(ScoreBoard);

        setPlayerTag();

        if (!isLocalPlayer)
        {
            foreach (Behaviour comp in ComponentsToDisable) {
                comp.enabled = false;
            }

            ScoreBoardInstance.SetActive(false);
        }
        else
        {
            SceneCamera = Camera.main;
            if (SceneCamera != null)
            {
                SceneCamera.gameObject.SetActive(false);
            }
        }
    }

	public void RemoveFirstRouteItem() {
		Route.RemoveAt (0);
		UpdateRouteUI ();
	}

	public void OnSelectedAttributedChanged(SyncListInt.Operation op, int index) {
		UpdateOwnDataUI ();
		UpdateRouteUI();
	}

    public void UpdateRouteUI() {
        ScoreBoardInstance.transform.Find("RouteText").GetComponent<Text>().text = "Route: " + string.Join(", ", Route.Select(r => r.ToString()).ToArray());
    }

    public void UpdateOwnDataUI() {
        string[] OwnData = SelectedAttributes.Select(a => ServerLogic.Profiles[ProfileIndex][a]).ToArray();
        ScoreBoardInstance.transform.Find("OwnDataText").GetComponent<Text>().text = string.Join("\n", OwnData);
    }

    public void UpdateCollectedDataUI() {
        ScoreBoardInstance.transform.Find("CollectedDataText").GetComponent<Text>().text = string.Join("\n", collectedData.Select(d => d.Value).ToArray());
    }

    public void AddCollectedData(KeyValuePair<ProfileAttribute, string> data) {
        collectedData.Add(data);
        UpdateCollectedDataUI();
    }

    public void RemoveCollectedData(KeyValuePair<ProfileAttribute, string> data) {
        collectedData.Remove(data);
        UpdateCollectedDataUI();
    }

    public List<KeyValuePair<ProfileAttribute, string>>.Enumerator GetCollectedDataEnumerator() {
        return collectedData.GetEnumerator();
    }

    public void setPlayerTag()
    {
        gameObject.AddComponent<Tag3D>();
        gameObject.GetComponent<Tag3D>().tagText = username;
        gameObject.GetComponent<Tag3D>().color = Color.white;
    }

	void Update(){
		if (Input.GetKeyDown ("v") && isLocalPlayer)
			Debug.Log (Route [0] + ", " + Route [1] + ", " + Route [2] + ", " + Route [3]);

		if (Input.GetKeyDown ("b") && isLocalPlayer)
			Debug.Log (string.Join(", ", SelectedAttributes.Select (a => ServerLogic.Profiles [ProfileIndex] [a]).ToArray ()));
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


    private NetworkInstanceId _Door_ID;

    [Command]
	public void CmdIncrementCounter(NetworkInstanceId netID)
	{
		GameObject theObject = NetworkServer.FindLocalObject(netID);
        if (++theObject.GetComponent<UnlockableDoor>().Counter == 3)
        {
            // Reset the door.
            theObject.GetComponent<UnlockableDoor>().RpcSetActive(false);
            theObject.GetComponent<UnlockableDoor>().RpcSetCounter(0);

            _Door_ID = netID;
            Invoke("ResetLocks", doorResetTime);
        }
    }


	// Networkinstance should be a door!
	public void ResetLocks()
    {
        GameObject door = NetworkServer.FindLocalObject(_Door_ID);
        door.SetActive(true);

        door.GetComponent<UnlockableDoor>().RpcSetActive(true);
		door.GetComponent<UnlockableDoor>().Counter = 0;

		GameObject lock1 = NetworkServer.FindLocalObject(door.GetComponent<UnlockableDoor>().Locks[0].GetComponent<LockCube>().netId);
		lock1.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock2 = NetworkServer.FindLocalObject(door.GetComponent<UnlockableDoor>().Locks[1].GetComponent<LockCube>().netId);
		lock2.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock3 = NetworkServer.FindLocalObject(door.GetComponent<UnlockableDoor>().Locks[2].GetComponent<LockCube>().netId);
        lock3.GetComponent<LockCube> ().RpcSetActive (true);
	}

    [Command]
    void CmdOnExchangeComplete(ProfileAttribute attribute, string data)
    {
        // [TODO] ... Add pair to collectedData.

    }

    [Command]
    public void CmdBroadcastNotification(string message)
    {
        RpcBroadcastNotification(message);
    }

    [ClientRpc]
    public void RpcBroadcastNotification(string message)
    {
        GameObject notification = GameObject.Find("Notification");
        notification.GetComponent<Notification>().Notify(message);
    }
}