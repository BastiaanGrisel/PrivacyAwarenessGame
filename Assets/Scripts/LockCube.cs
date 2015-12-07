using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class LockCube : NetworkBehaviour {

	public int Key;
//	public GameObject Door;

	// Use this for initialization
	void OnValidate () {
		gameObject.GetComponent<TextMesh> ().text = Key.ToString ();
	}

	void Start() {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerSetup> ().keys.Contains (Key)) {
			other.gameObject.GetComponent<PlayerSetup>().CmdDestroyObject(netId);
		}
	}
}
