using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerState))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensitivity = 3f;

	private PlayerMotor motor;
	private PlayerState state;
    private ServerLogic serverLogic;
    private GameObject DataExchangeCanvas;
    [SerializeField]
    private GameObject DataExchangeCanvasPrefab;
    [SerializeField]
    private GameObject QuestionButtonPrefab;
    private GameObject Canvas;


    void Awake()
    {
		motor = GetComponent<PlayerMotor>();
		state = GetComponent<PlayerState>();
        serverLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
        Canvas = Instantiate(DataExchangeCanvasPrefab) as GameObject;
        foreach (ProfileAttribute attr in Enum.GetValues(typeof(ProfileAttribute)))
        {
            GameObject newButton = Instantiate(QuestionButtonPrefab) as GameObject;
            Transform QuestionsPanel = Canvas.transform.Find("Panel").Find("ScrollView").Find("QuestionsPanel");
            newButton.transform.parent = QuestionsPanel;
        }
        Canvas.SetActive(false);
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
        if (c.gameObject.tag == "Trophy" && state.Route.Count > 0 && c.gameObject.GetComponent<Trophy> ().Number == state.Route [0]) {

			c.gameObject.SetActive(false);
			state.Route.RemoveAt(0);

			if(state.Route.Count == 0) {
				state.ScoreBoardInstance.GetComponentInChildren<Score>().AddOnePointTo(state.Team);
			}
		}
//        {
//            c.gameObject.SetActive(false);
//            if (GameObject.FindGameObjectsWithTag("Trophy").Length == 0)
//                this.CmdEndGame();
//        }

        if (c.gameObject.tag == "Player")
        {
            
            Transform panel = Canvas.transform.Find("DataExchangePanel");
            Text dataExchangeGUIText = panel.transform.Find("DataExchangePlayerIDText").GetComponent<Text>();
            dataExchangeGUIText.text = c.gameObject.GetInstanceID().ToString();
            panel.GetComponent<DataExchangePanel>().otherPlayer = c.gameObject.GetInstanceID();
            Canvas.SetActive(true);
        }
    }

    [Command]
    public void CmdEndGame()
    {
        serverLogic.GameStarted = false;
    }
}