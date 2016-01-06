using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LockCube : NetworkBehaviour {

	public ProfileAttribute Key;
	public GameObject Door;	

	[ClientRpc]
	public void RpcSetActive (bool on) {
		gameObject.SetActive (on);
	}

	[ClientRpc]
	public void RpcSetKey(ProfileAttribute p) {
		gameObject.GetComponent<TextMesh> ().text = p.ToFriendlyString();
		Key = p;
	}

	void OnTriggerEnter(Collider other) {
//		Debug.Log (Key +" ("+other.gameObject.GetComponent<PlayerState> ().SelectedAttributes.Contains((int) Key)+")");
		if (other.gameObject.tag == "Player")
        {
            var iter = other.gameObject.GetComponent<PlayerState>().collectedData.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key == this.Key && iter.Current.Value != "")
                {
                    other.gameObject.GetComponent<PlayerState>().CmdIncrementCounter(Door.GetComponent<UnlockableDoor>().netId);
                    other.gameObject.GetComponent<PlayerState>().CmdDestroyLockCube(netId);

                    other.gameObject.GetComponent<PlayerState>().collectedData.Remove(iter.Current);
                    break;
                }
            }

		}
	}
}
