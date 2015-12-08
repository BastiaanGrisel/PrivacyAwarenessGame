using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnlockableDoor : NetworkBehaviour {

	[SyncVar]
	public int counter;

	public List<LockCube> locks = new List<LockCube> ();

	void Awake() {
		counter = 0;
//		locks = 
	}

	public void ResetLocks(int[] keys) {
		for(int i = 0; i < locks.Count; i++) {
			locks[i].SetKey(keys[i]);
		}
	}

	public void RegisterLock(LockCube l) {
		locks.Add (l);
	}

	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.SetActive (on);
	}
}
