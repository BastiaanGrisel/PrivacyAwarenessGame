using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    // Global Game State and Game Logic
    public ServerLogic logic;

    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId)
    {
        // Initialize the Game Logic the first time a player connects to the game.
        if (numPlayers == 0)
        {
            logic = GameObject.Find("Game").GetComponent<ServerLogic>();
            logic.Initialize();
        }

        // Instantiate a Player
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player.AddComponent<Tag3D>();
        NetworkServer.AddPlayerForConnection(connection, player, playerControllerId);

        // Set initial Player state.
        logic.InitializePlayer(player);
    }
}
