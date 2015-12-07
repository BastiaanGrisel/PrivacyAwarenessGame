using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockableDoor : MonoBehaviour {

	public List<GameObject> locks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool destory = true;

		foreach (GameObject l in locks)
			if (l != null)
				destory = false;
			
		if(destory) Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		
	}

}
