using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed = 0.1f;
    public Vector3 direction;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

	private UIManager MyUIManager;

	public IDictionary<string, string> personalData;

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = GetComponent<Rigidbody>().position;
            stream.Serialize(ref syncPosition);

            syncPosition = GetComponent<Rigidbody>().velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = GetComponent<Rigidbody>().position;
        }
    }

    void Start()
    {

    }

    void Awake()
    {
        lastSynchronizationTime = Time.time;
		MyUIManager = GameObject.Find("UIManager").GetComponent<UIManager>();     
	}

    void Update()
    {
        if (GetComponent<NetworkView>().isMine)
        {
            InputMovement();
            Camera.main.GetComponent<SmoothFollow>().target = GetComponent<NetworkView>().transform;
        }
        else
            SyncedMovement();
    }

    private void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
            direction += transform.forward * 1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            direction -= transform.forward * 1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            direction += transform.right * 1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            direction -= transform.right * 1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, 1))
            direction += transform.up * 10f * Time.deltaTime;

        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + direction);
        direction /= 1.1f;
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			MyUIManager.TradeUI.SetActive(true);
			// Get other players name
			Debug.Log(other.gameObject.GetComponent<Player>().speed);
//			Debug.Log (other.gameObject.GetComponent<Player>().personalData["name"]);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			MyUIManager.TradeUI.SetActive(false);
		}
	}
}