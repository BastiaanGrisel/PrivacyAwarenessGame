using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MyNetworkManager : NetworkManager {
	
	public List<int> AllKeys;
	private List<int> KeysNotYetInGame;
	System.Random rnd = new System.Random();

	public void Start() {
		KeysNotYetInGame = new List<int> (AllKeys);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		var player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);

		for (int i = 0; i < 3; i++) {
			if (KeysNotYetInGame.Count > 0) {
				int index = rnd.Next (0, KeysNotYetInGame.Count - 1);
				player.GetComponent<PlayerSetup> ().keys.Add (KeysNotYetInGame[index]);
				KeysNotYetInGame.RemoveAt(index);
			} else
				player.GetComponent<PlayerSetup> ().keys.Add (
					AllKeys[rnd.Next(0, AllKeys.Count - 1)]);
		}
		
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

}
