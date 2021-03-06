﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Gets a movement vector
    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    // Gets a rotation vector
    public void Rotate(Vector3 rotation)
    {
        this.rotation = rotation;
    }

    // Gets a rotation vector for the camera
    public void RotateCamera(Vector3 cameraRotation)
    {
        this.cameraRotation = cameraRotation;
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    // Perform movement based on velocity veriable
    void PerformMovement()
    {
        if (!velocity.Equals(Vector3.zero))
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (Camera.main != null)
        {
            Camera.main.transform.Rotate(-cameraRotation);
        }
    }
}
