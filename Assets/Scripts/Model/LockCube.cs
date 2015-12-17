﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LockCube : NetworkBehaviour {

	[SyncVar(hook = "SetKey")] 
	public ProfileAttribute Key;

	public GameObject Door;

	// Use this for initialization
	void OnValidate () {	
//		gameObject.GetComponent<TextMesh> ().text = Key.ToString ();
	}

	void Awake() {
		Door.GetComponent<UnlockableDoor>().RegisterLock(this);
	}

	void Start() {

	}

	// Update is called once per frame
	void Update () {

	}

	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.SetActive (on);
	}

	public void SetKey(ProfileAttribute k) {
		Key = k;
		gameObject.GetComponent<TextMesh> ().text = k.ToFriendlyString();
	}

//	public void ShowText(string t) {
//		gameObject.GetComponent<TextMesh> ().text = t;
//	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerState> ().SelectedAttributes.Contains((int) Key)) {
			other.gameObject.GetComponent<PlayerState>().CmdIncrementCounter(Door.GetComponent<UnlockableDoor>().netId);
			other.gameObject.GetComponent<PlayerState>().CmdDestroyLockCube(netId);
		}
	}
}
