using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ResetDoorCube : NetworkBehaviour {

	public GameObject Door;

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerSetup>().CmdResetLocks(
				Door.GetComponent<UnlockableDoor>().netId,
				Door.GetComponent<UnlockableDoor>().locks[0].netId,
				Door.GetComponent<UnlockableDoor>().locks[1].netId,
				Door.GetComponent<UnlockableDoor>().locks[2].netId);
		}
	}

}
