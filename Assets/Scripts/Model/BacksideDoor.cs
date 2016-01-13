using UnityEngine;
using System.Collections;

public class BacksideDoor : MonoBehaviour {

	public GameObject Door;

	void OnTriggerEnter(Collider other) {
		Door.GetComponent<BoxCollider> ().enabled = false;
	}

	void OnTriggerExit(Collider other) {
		if(Door.GetComponent<MeshRenderer> ().enabled)
			Door.GetComponent<BoxCollider> ().enabled = true;
	}

}
