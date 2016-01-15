using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;

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
    [SerializeField]
    private GameObject DataExchangeCanvasPrefab;
    [SerializeField]
    private GameObject ButtonPrefab;
    private GameObject DataExchangeCanvas;

    [SerializeField]
    private GameObject AnswerCanvasPrefab;
    private GameObject AnswerCanvas;

    private System.Random rnd = new System.Random();

    void Awake()
    {
		motor = GetComponent<PlayerMotor>();
		state = GetComponent<PlayerState>();
        serverLogic = GameObject.Find("Game").GetComponent<ServerLogic>();
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            DataExchangeCanvas = Instantiate(DataExchangeCanvasPrefab) as GameObject;
            DataExchangeCanvas.SetActive(false);
            AnswerCanvas = Instantiate(AnswerCanvasPrefab) as GameObject;
            AnswerCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // When freeze is on (I.E a forced GUI) then the player will not move or rotate the camera.
        if (gameObject.GetComponent<PlayerState>().freeze)
        {
            Vector3 velocity = Vector3.zero;
            motor.Move(velocity);
            motor.RotateCamera(new Vector3(0.0f, 0.0f, 0.0f));
            motor.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
            return;
        }

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

	[Command]
	public void CmdAddPointTo(int Team) {
		RpcAddPointTo (Team);
		int points = serverLogic.ScoreBoardInstance.GetComponentInChildren<Score> ().getPoints (Team) + 1;
		Debug.Log (points + " " + serverLogic.Players.Count (p => p.Team == Team));

		if (serverLogic.Players.Count (p => p.Team == Team) == points) {
			state.CmdBroadcastNotification ("Game Over! Team " + Team + " wins!", float.PositiveInfinity);
			CmdEndGame ();
		}
	}

	[ClientRpc]
	public void RpcAddPointTo(int Team) {
		serverLogic.ScoreBoardInstance.GetComponentInChildren<Score>().AddOnePointTo(Team);
	}

    void OnCollisionEnter(Collision c)
    {
        // Also when the controller is disabled it will enter the OnCollisionEnter.
        if (!isLocalPlayer)
            return;

        if (c.gameObject.tag == "Trophy" && state.Route.Count > 0 && c.gameObject.GetComponent<Trophy> ().Number == state.Route [0]) {

			c.gameObject.SetActive(false);
			state.RemoveFirstRouteItem();
			String extra = state.Route.Count > 0 ? " Only "+state.Route.Count+" to go! Next up: Trophy "+state.Route[0] : " You have scored one point for your team!";
			GameObject.Find("Notification").GetComponent<Notification>().Notify("Picked up Trophy "+c.gameObject.GetComponent<Trophy> ().Number+"!"+extra, 2000);

			serverLogic.revealTeamMember(state.Team);

			if(state.Route.Count == 0) {
				CmdAddPointTo(state.Team);
			}
		}

        if (c.gameObject.tag == "Player")
        {
            CmdStartCommunication(this.gameObject, c.gameObject);
        }
    }

    [Command]
    void CmdStartCommunication(GameObject player1, GameObject player2)
    {
        PlayerState player1State = player1.GetComponent<PlayerState>();
        PlayerState player2State = player2.GetComponent<PlayerState>();
        if (!player1State.isQuestioning && !player1State.isAnswering && !player1State.isWaitingforQuestion && !player2State.isQuestioning && !player2State.isAnswering && !player2State.isWaitingforQuestion)
        {
            player1State.isQuestioning = true;
            player1State.isWaitingforQuestion = true;
            player2State.isQuestioning = true;
            player2State.isWaitingforQuestion = true;

            RpcStartCommunication(player1, player2);
        }
    }

    [ClientRpc]
    void RpcStartCommunication(GameObject player1, GameObject player2)
    {
        player1.GetComponent<PlayerController>().StartCommunication(player1, player2);
        player2.GetComponent<PlayerController>().StartCommunication(player2, player1);
    }

    void StartCommunication(GameObject local, GameObject other)
    {
        if (isLocalPlayer && this.gameObject.Equals(local))
        {
            PlayerState otherPlayerState = other.GetComponent<PlayerState>();

            state.CmdCommunicationWithId(otherPlayerState.netId.Value);
            state.freeze = true;

            Transform panel = DataExchangeCanvas.transform.Find("DataExchangePanel");
            Text dataExchangeGUIText = panel.transform.Find("DataExchangePlayerIDText").GetComponent<Text>();
            dataExchangeGUIText.text = otherPlayerState.username;

            Transform QuestionsPanel = panel.Find("Panel").Find("ScrollView").Find("QuestionsPanel");
            foreach (Transform trans in QuestionsPanel)
            {
                Destroy(trans.gameObject);
            }

            foreach (ProfileAttribute attr in Enum.GetValues(typeof(ProfileAttribute)))
            {
                if (otherPlayerState.SelectedAttributes.Contains((int)attr))
                {
                    GameObject newButton = Instantiate(ButtonPrefab) as GameObject;
                    newButton.transform.Find("Text").GetComponent<Text>().text = ProfileAttributeExt.ToFriendlyString(attr);
                    newButton.transform.SetParent(QuestionsPanel);
                    Button button = newButton.GetComponent<Button>();
                    ProfileAttribute attrClone = attr;
                    button.onClick.AddListener(() =>
                    {
                        state.freeze = false;
                        CmdAskQuestion(attrClone, this.gameObject, other);
                        DataExchangeCanvas.SetActive(false);
                        state.CmdIsQuestioning(false);
                    });
                }
            }

            DataExchangeCanvas.SetActive(true);
        }
    }

    [Command]
    void CmdEndGame()
    {
        serverLogic.GameStarted = false;
    }

    [Command]
    void CmdAskQuestion(ProfileAttribute attr, GameObject requester, GameObject questioned)
    {
        RpcAskQuestion(attr, requester, questioned);
    }

    [ClientRpc]
    void RpcAskQuestion(ProfileAttribute attr, GameObject requester, GameObject questioned)
    {
        questioned.GetComponent<PlayerController>().AskQuestion(attr, requester, questioned);
    }

    void AskQuestion(ProfileAttribute attr, GameObject requester, GameObject questioned)
    {
        if (this.gameObject.Equals(questioned) && isLocalPlayer)
        {
            state.CmdIsAnswering(true);
            Transform panel = AnswerCanvas.transform.Find("AnswerPanel");
            Text questionsGUIText = panel.transform.Find("AnswerPlayerIDText").GetComponent<Text>();
            PlayerState requesterPlayerState = requester.GetComponent<PlayerState>();
            questionsGUIText.text = requesterPlayerState.username;

            Transform AnswerPanel = panel.Find("Panel").Find("ScrollView").Find("AnswerPanel");
            foreach (Transform trans in AnswerPanel)
            {
                Destroy(trans.gameObject);
            }

            Profile profile = serverLogic.Profiles[questioned.GetComponent<PlayerState>().ProfileIndex];
            string answerTruth = profile[(int)attr];
            GameObject truthButton = Instantiate(ButtonPrefab) as GameObject;
            //Debug.Log(questioned.GetComponent<PlayerState>().ProfileIndex + "|" + questioned.GetComponent<PlayerState>().username + "|" + "Waarheid (" + ProfileAttributeExt.ToFriendlyString(attr) + "=" + answerTruth + ")");
            truthButton.transform.Find("Text").GetComponent<Text>().text = "Waarheid (" + ProfileAttributeExt.ToFriendlyString(attr) + "=" + answerTruth + ")";
            truthButton.transform.SetParent(AnswerPanel);
            Button tButton = truthButton.GetComponent<Button>();
            tButton.onClick.AddListener(() =>
            {
                CmdAnswerQuestion(attr, answerTruth, requester, questioned);
                AnswerCanvas.SetActive(false);
                state.CmdIsAnswering(false);
                if (!state.isQuestioning)
                    state.CmdCommunicationWithId(NetworkInstanceId.Invalid.Value);
            });

            GameObject lieButton = Instantiate(ButtonPrefab) as GameObject;
            lieButton.transform.Find("Text").GetComponent<Text>().text = "Leugen";
            lieButton.transform.SetParent(AnswerPanel);
            Button lButton = lieButton.GetComponent<Button>();
            lButton.onClick.AddListener(() =>
            {
                GameObject logic = GameObject.Find("Game");

                string data = logic.GetComponent<ServerLogic>().Profiles[rnd.Next() % logic.GetComponent<ServerLogic>().Profiles.Count - 1][(int)attr];
                if(data == requesterPlayerState.GetProfile()[(int)attr])
                    data = logic.GetComponent<ServerLogic>().Profiles[logic.GetComponent<ServerLogic>().Profiles.Count - 1][(int)attr];
                
                CmdAnswerQuestion(attr, data + " ", requester, questioned);
                AnswerCanvas.SetActive(false);
                state.CmdIsAnswering(false);
                if (!state.isQuestioning)
                    state.CmdCommunicationWithId(NetworkInstanceId.Invalid.Value);
            });

            state.CmdIsWaitingforQuestion(false);
            if (!state.isAnswering)
                state.CmdCommunicationWithId(NetworkInstanceId.Invalid.Value);

            AnswerCanvas.SetActive(true);
        }
    }

    [Command]
    void CmdAnswerQuestion(ProfileAttribute attr, string answer, GameObject requester, GameObject questioned)
    {
        RpcAnswerQuestion(attr, answer, requester, questioned);
    }

    [ClientRpc]
    void RpcAnswerQuestion(ProfileAttribute attr, string answer, GameObject requester, GameObject questioned)
    {
        requester.GetComponent<PlayerController>().AnswerQuestion(attr, answer, requester, questioned);
    }

    void AnswerQuestion(ProfileAttribute attr, string answer, GameObject requester, GameObject questioned)
    {
        if (this.gameObject.Equals(requester) && isLocalPlayer)
        {
            state.AddCollectedData(new KeyValuePair<ProfileAttribute, string>(attr, answer));
            GameObject.Find("Notification").GetComponent<Notification>().Notify(questioned.GetComponent<PlayerState>().username + " has answerred your question about " + ProfileAttributeExt.ToFriendlyString(attr));
        }
    }
}