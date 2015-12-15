using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId)
    {
        // Instantiate a Player
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(connection, player, playerControllerId);
    }
}
