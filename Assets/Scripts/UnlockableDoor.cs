using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnlockableDoor : NetworkBehaviour {

	[SyncVar]
	public int counter;

	private List<LockCube> locks;

	void Awake() {
		counter = 0;
		locks = new List<LockCube> ();
	}

	public void ResetLocks(int[] keys) {
		for(int i = 0; i < locks.Count; i++) {
			locks[i].SetKey(keys[i]);
		}
	}

	public void RegisterLock(LockCube l) {
		locks.Add (l);
	}
}
