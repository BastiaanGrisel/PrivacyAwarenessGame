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

	void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player")
        {
            var iter = other.gameObject.GetComponent<PlayerState>().GetCollectedDataEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key == this.Key && !iter.Current.Value.EndsWith(" "))
                {
                    other.gameObject.GetComponent<PlayerState>().CmdIncrementCounter(Door.GetComponent<UnlockableDoor>().netId);
                    other.gameObject.GetComponent<PlayerState>().CmdDestroyLockCube(netId);
                    other.gameObject.GetComponent<PlayerState>().RemoveCollectedData(iter.Current);
                    break;
                }
                else if (iter.Current.Key == this.Key && iter.Current.Value.EndsWith(" "))
                {
                    GameObject.Find("Notification").GetComponent<Notification>().Notify(iter.Current.Value + "is fake data!");
                    other.gameObject.GetComponent<PlayerState>().RemoveCollectedData(iter.Current);
                    break;
                }
            }

		}
	}
}
