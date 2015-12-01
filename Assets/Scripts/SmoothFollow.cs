// Smooth Follow from Standard Assets
// Converted to C# because I fucking hate UnityScript and it's inexistant C# interoperability
// If you have C# code and you want to edit SmoothFollow's vars ingame, use this instead.
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    // The target we are following
    public Transform target;
    private Vector3 position;

    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

 

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target) return;

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        //transform.position = target.position;
        //transform.position -= currentRotation * Vector3.forward * distance;
        //transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        Vector3 direction = position - target.position;
        transform.position -= direction;

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(target.position, target.up, Input.GetAxis("Mouse X") * 10.0f);
            transform.RotateAround(target.position, target.right, -Input.GetAxis("Mouse Y") * 5.0f);
        }

        // Always look at the target from a specific distance.
        transform.LookAt(target);
        

        // Update the forward and right vectors of the entity to make it move with the camera.
        target.forward = transform.forward;
        target.right = transform.right;

        position = target.position;
    }
}