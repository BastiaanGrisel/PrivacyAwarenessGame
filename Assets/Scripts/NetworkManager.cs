using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    private const uint MaxConnections = 10;

    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId)
    {
        if (Network.connections.Length <= MaxConnections)
        {
            // Instantiate a Player
            GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(connection, player, playerControllerId);
        }
        else
            // [Todo] Handle players disconnecting, and reconnecting later.
            Network.maxConnections = 0;
    }
}
