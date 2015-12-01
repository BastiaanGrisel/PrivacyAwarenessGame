using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour {
	
	private bool open = false;
	private float smooth = 2.0f;
	private float DoorOpenAngle = 90.0f;
	
	private Vector3 defaultRot;
	private Vector3 openRot;
	
	// Use this for initialization
	void Start () {
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(open) {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
		} else {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
			open = true;
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") 
			open = false;
	}
}