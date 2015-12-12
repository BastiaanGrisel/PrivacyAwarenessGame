using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LauncherGUI : MonoBehaviour
{
    // UI Elements
    private Button hostGame;
    private Button joinGame;
    private Button settings;
    private Button exit;
    private InputField networkAddress;

    // Networking Attributes
    private NetworkManager manager;

    void Start()
    {
        // Find all UI Elements
        manager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
        hostGame = GameObject.Find("Host Game").GetComponent<Button>();
        joinGame = GameObject.Find("Join Game").GetComponent<Button>();
        settings = GameObject.Find("Settings").GetComponent<Button>();
        exit = GameObject.Find("Exit").GetComponent<Button>();

        hostGame.onClick.AddListener(() =>
        {
            manager.StartHost();
            gameObject.SetActive(false);
        });

        joinGame.onClick.AddListener(() =>
        {
            manager.networkAddress = GameObject.Find("IP").GetComponent<InputField>().text;
            manager.StartClient();
            gameObject.SetActive(false);
        });

        settings.onClick.AddListener(() =>
        {
            Debug.Log("Settings pressed!");
            gameObject.SetActive(false);
        });

        exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
