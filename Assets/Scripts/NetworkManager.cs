using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    // Global Game State and Game Logic
    public ServerLogic logic;

	public void Awake() {
		logic = GameObject.Find("Game").GetComponent<ServerLogic>();
	}

    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId)
    {
        // Instantiate a Player
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player.AddComponent<Tag3D>();
        NetworkServer.AddPlayerForConnection(connection, player, playerControllerId);

        // Set initial Player state.
        logic.InitializePlayer(player);
    }
}
