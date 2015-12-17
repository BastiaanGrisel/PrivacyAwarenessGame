using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerState : NetworkBehaviour
{
    // Profile that the player is using
    [SyncVar] public int ProfileIndex;
	public List<int> SelectedAttributes = new List<int>();

	// The number of times a player has cheated
	public int Cheated = 0;

	public int Team;  

    [SerializeField]
	private Behaviour[] ComponentsToDisable;
    private Camera SceneCamera;

	public GameObject KeysHUD;
    public List<int> keys = new List<int>();

    void Start ()
    {
        GameObject.Find("Game").GetComponent<ServerLogic>().RegisterPlayer(this);

        if (!isLocalPlayer)
        {
			this.gameObject.AddComponent<Tag3D>();

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

			GameObject ui = Instantiate(KeysHUD);
			for (int i = 0; i < keys.Count; i++) {
				ui.GetComponent<HUDKeys>().KeyTexts[i].text = keys[i].ToString();
			}
        }
	}

    void OnDisable()
    {
        if (SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }
    }

	void Update() {
		if (Input.GetKeyDown ("n")) {
			Debug.Log (Team + " T-P " + ProfileIndex);
			SelectedAttributes.ForEach(a => Debug.Log(a.ToString()));
		}
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
			theObject.GetComponent<UnlockableDoor>().RpcSetActive(false);
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
		lock1.GetComponent<LockCube> ().Key = rnd.Next (1, 4);
		lock1.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock2 = NetworkServer.FindLocalObject(netID2);
		lock2.GetComponent<LockCube> ().Key = rnd.Next (1, 4);
		lock2.GetComponent<LockCube> ().RpcSetActive (true);

		GameObject lock3 = NetworkServer.FindLocalObject(netID3);
		lock3.GetComponent<LockCube> ().Key = rnd.Next (1, 4);
		lock3.GetComponent<LockCube> ().RpcSetActive (true);
	}
}
