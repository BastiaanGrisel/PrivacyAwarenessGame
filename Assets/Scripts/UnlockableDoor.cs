using UnityEngine;
using System.Collections;

public class UnlockableDoor : MonoBehaviour {

	public int counter = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(counter == 3)
			gameObject.SetActive (false);
	}

	void OnCollisionEnter(Collision collision) {
		
	}

}
