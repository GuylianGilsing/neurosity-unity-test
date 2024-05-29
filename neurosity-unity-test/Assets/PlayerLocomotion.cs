using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRigidbody;

    public float movementSpeed = 7f;
    public float rotationSpeed = 15f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
{
    // Get the forward and right vectors of the camera in world space
    Vector3 cameraForward = Camera.main.transform.forward;
    Vector3 cameraRight = Camera.main.transform.right;

    // Project movement input onto the camera's forward and right vectors
    Vector3 moveDirection = cameraForward * inputManager.verticalInput + cameraRight * inputManager.horizontalInput;
    moveDirection.Normalize();
    moveDirection.y = 0; // Ensure no vertical movement

    // Move the player in world space
    Vector3 moveVelocity = moveDirection * movementSpeed;
    playerRigidbody.velocity = moveVelocity;
    }



    private void HandleRotation()
    {
        Vector3 targetDirection = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput);
        if (targetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = smoothedRotation;
    }
}

