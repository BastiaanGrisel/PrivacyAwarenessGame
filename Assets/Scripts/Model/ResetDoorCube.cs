using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ResetDoorCube : NetworkBehaviour {

	public GameObject Door;

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerState>().CmdResetLocks(
				Door.GetComponent<UnlockableDoor>().netId,
				Door.GetComponent<UnlockableDoor>().Locks[0].netId,
				Door.GetComponent<UnlockableDoor>().Locks[1].netId,
				Door.GetComponent<UnlockableDoor>().Locks[2].netId);
		}
	}

}