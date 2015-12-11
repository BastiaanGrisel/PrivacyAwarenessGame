using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private PlayerMotor motor;
    private ServerLogic serverLogic;
    void Start ()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Awake(){
        serverLogic = GameObject.FindObjectOfType<ServerLogic>();
    }

    void Update ()
    {
        if (serverLogic.isRunning)
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
            this.GetComponent<PlayerSetup>().CmdEndGame();
        }
    }
}
