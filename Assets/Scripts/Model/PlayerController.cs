using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private PlayerMotor motor;
    private ServerLogic serverLogic;
    private GameObject DataExchangePanel;

    void Awake()
    {
		motor = GetComponent<PlayerMotor>();
        serverLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
        DataExchangePanel = serverLogic.DataExchangePanel;
    }

    void Update()
    {
        if (serverLogic.GameStarted)
        {
            // Calculate velocity as a 3D vector
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            Vector3 movHorizontal = transform.right * xMov;
            Vector3 movVertical = transform.forward * zMov;

            Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

            // Apply movement
            motor.Move(velocity);
        }
        else
        {
            Vector3 velocity = Vector3.zero;
            motor.Move(velocity);
        }

        // Calculate rotation as a 3D vector (turning around)
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        // Apply rotation
        motor.Rotate(rotation);

        // Calculate camera rotation as a 3D vector (turning around)
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * mouseSensitivity;

        // Apply camera rotation
        motor.RotateCamera(cameraRotation);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Trophy" && ((Trophy)c.gameObject.GetComponent<Trophy>()).isWinningTrophy)
        {
            c.gameObject.SetActive(false);
            if (GameObject.FindGameObjectsWithTag("Trophy").Length == 0)
                this.CmdEndGame();
        }

        if (c.gameObject.tag == "Player")
        {
            Transform text = DataExchangePanel.transform.Find("DataExchangePlayerIDText");
            Text dataExchangeGUIText = text.GetComponent<Text>();
            dataExchangeGUIText.text = c.gameObject.GetInstanceID().ToString();
            DataExchangePanel.GetComponent<DataExchangePanel>().otherPlayer = c.gameObject.GetInstanceID();
            DataExchangePanel.gameObject.SetActive(true);
        }
    }

    [Command]
    public void CmdEndGame()
    {
        serverLogic.GameStarted = false;
    }
}