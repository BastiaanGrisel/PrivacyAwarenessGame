using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    private const uint MaxConnections = 10;

    class UsernameMsg : MessageBase
    {
        public string username;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        UsernameMsg user = new UsernameMsg();
        user.username = GameObject.Find("Username").GetComponent<UnityEngine.UI.InputField>().text;
        GameObject.Find("Launcher").SetActive(false);
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn, 0, user);
    }

    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId, NetworkReader extraMessageReader)
    {
        if (Network.connections.Length <= MaxConnections)
        {
            // Instantiate a Player
            GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
            player.GetComponent<PlayerState>().username = extraMessageReader.ReadMessage<UsernameMsg>().username;
            NetworkServer.AddPlayerForConnection(connection, player, playerControllerId);
        }
        else
            // [Todo] Handle players disconnecting, and reconnecting later.
            Network.maxConnections = 0;
    }
}
