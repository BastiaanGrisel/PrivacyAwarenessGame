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
	}

	void Start() {
		ServerLogic = GameObject.Find("Game").GetComponent<ServerLogic>();

		List<ProfileAttribute> RandomAttributes = new List<ProfileAttribute>();
		ProfileAttribute attr;

		for(int i = 0; i < Locks.Count; i++) {
			do {
				attr = (ProfileAttribute) UnityEngine.Random.Range(0, Profile.TotalNumberOfAttributes());
			} while (RandomAttributes.Contains(attr));

			RandomAttributes.Add(attr);
			Locks[i].SetKey(attr);
			Locks[i].ShowText(attr.ToFriendlyString());
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
