using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnlockableDoor : NetworkBehaviour {

	[SyncVar]
	public int Counter;

	public List<LockCube> Locks = new List<LockCube> ();

	private ServerLogic ServerLogic;

	void Awake() {
		Counter = 0;
		ServerLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
	}

	void Start() {
		List<int> RandomNumbers = new List<int>();
		int rnd;

		for(int i = 0; i < Locks.Count; i++) {
			do {
				rnd = UnityEngine.Random.Range(0, ServerLogic.Categories.Count);
			} while (RandomNumbers.Contains(rnd));

			RandomNumbers.Add(rnd);
			Locks[i].SetKey(rnd);
			Locks[i].ShowText(ServerLogic.Categories[rnd]);
		}
	}
	
	public void RegisterLock(LockCube l) {
		Locks.Add (l);
	}

	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.GetComponent<MeshRenderer> ().enabled = on;
		gameObject.GetComponent<BoxCollider> ().enabled = on;

	}
}
