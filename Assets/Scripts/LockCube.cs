using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class LockCube : NetworkBehaviour {

	[SyncVar(hook = "SetKey")] 
	public int Key;

	public GameObject Door;

	// Use this for initialization
	void OnValidate () {	
		gameObject.GetComponent<TextMesh> ().text = Key.ToString ();
	}

	void Awake() {
		Door.GetComponent<UnlockableDoor>().RegisterLock(this);
	}

	// Update is called once per frame
	void Update () {

	}

	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.SetActive (on);
	}

	public void SetKey(int k) {
		gameObject.GetComponent<TextMesh> ().text = k.ToString ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerSetup> ().keys.Contains (Key)) {
				other.gameObject.GetComponent<PlayerSetup>().CmdIncrementCounter(Door.GetComponent<UnlockableDoor>().netId);

			other.gameObject.GetComponent<PlayerSetup>().CmdDestroyLockCube(netId);
		}
	}
}
