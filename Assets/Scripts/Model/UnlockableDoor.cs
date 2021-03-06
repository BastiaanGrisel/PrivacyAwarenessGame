﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnlockableDoor : NetworkBehaviour {

	[SyncVar]
	public int Counter;
	public List<GameObject> Locks;
	
	void Awake() {
		Counter = 0;
	}
	
	[Server]
	public void InitializeDoor(int NumberOfAttributes) {
		List<ProfileAttribute> RandomAttributes = new List<ProfileAttribute> ();
		ProfileAttribute attr;

		foreach(GameObject l in Locks) {
			do {
				attr = (ProfileAttribute)UnityEngine.Random.Range (0, NumberOfAttributes);
			} while (RandomAttributes.Contains(attr));

			RandomAttributes.Add (attr);
			l.GetComponent<LockCube>().RpcSetKey(attr);
		}
	}

	void Update() {
//		if (Input.GetKeyDown (KeyCode.Return) && isServer) {
//			InitializeDoor();
//		}
	}
	
	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.GetComponent<MeshRenderer> ().enabled = on;
		gameObject.GetComponent<BoxCollider> ().enabled = on;
	}

    [ClientRpc]
    public void RpcSetCounter(int counter)
    {
        Counter = counter;
    }
}
