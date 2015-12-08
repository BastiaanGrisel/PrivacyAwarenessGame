using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
	private Behaviour[] componetsToDisable;
    private Camera sceneCamera;

	public GameObject HealthUI;

	public SyncListInt keys;

	System.Random rnd;

	void Awake() {
		keys = new SyncListInt ();
		rnd = new System.Random();
	}

    void Start ()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour comp in componetsToDisable){
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
			for (int i = 0; i < keys.Count; i++) {
				ui.GetComponent<HUDKeys>().KeyTexts[i].text = keys[i].ToString();
			}
        }
	}

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
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

		if (++theObject.GetComponent<UnlockableDoor> ().counter == 3)
			theObject.GetComponent<UnlockableDoor>().RpcSetActive(false);
	}

	[Command]
	// Networkinstance should be a door!
	public void CmdResetLocks(NetworkInstanceId doorNetID, NetworkInstanceId netID1, NetworkInstanceId netID2, NetworkInstanceId netID3) {
		GameObject door = NetworkServer.FindLocalObject(doorNetID);
		door.GetComponent<UnlockableDoor>().RpcSetActive(true);
		door.GetComponent<UnlockableDoor> ().counter = 0;

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

    [Command]
    public void CmdEndGame()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().ableToMove = false;
        }

    }
}
