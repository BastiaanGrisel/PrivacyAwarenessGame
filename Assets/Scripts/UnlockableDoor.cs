using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnlockableDoor : NetworkBehaviour {

	[SyncVar]
	public int counter;

	void Awake() {
		counter = 0;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		bool destory = true;
//
//		foreach (GameObject l in locks)
//			if (l != null)
//				destory = false;
//			
//		if(destory) Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		
	}

}
