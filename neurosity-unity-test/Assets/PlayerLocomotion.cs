using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    // Reference to the InputManager component
    InputManager inputManager;
    // Reference to the Rigidbody component
    Rigidbody playerRigidbody;

    Animator animator;

    // Speed at which the player moves
    public float movementSpeed = 7f;
    // Speed at which the player rotates
    public float rotationSpeed = 15f;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the InputManager component attached to the same GameObject
        inputManager = GetComponent<InputManager>();
        // Get the Rigidbody component attached to the same GameObject
        playerRigidbody = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the same GameObject as PlayerLocomotion.");
        }
    }

    // Handles all movement-related functions
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        HandleAnimation(); // Call HandleAnimation method
    }

    // Handles the player's movement
    private void HandleMovement()
    {
        // Get the forward and right vectors of the camera in world space
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Project movement input onto the camera's forward and right vectors
        Vector3 moveDirection = cameraForward * inputManager.verticalInput + cameraRight * inputManager.horizontalInput;
        moveDirection.Normalize(); // Normalize to ensure consistent movement speed
        moveDirection.y = 0; // Ensure no vertical movement

        // Move the player in world space
        Vector3 moveVelocity = moveDirection * movementSpeed;
        playerRigidbody.velocity = moveVelocity; // Apply movement to the Rigidbody
    }

    // Handles the player's rotation
    private void HandleRotation()
    {
        // Calculate the target direction based on input
        Vector3 targetDirection = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput);
        if (targetDirection == Vector3.zero) return; // Exit if no input

        // Calculate the target rotation based on the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // Smoothly interpolate the player's rotation towards the target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Apply the smoothed rotation to the player
        transform.rotation = smoothedRotation;
    }

    private void HandleAnimation()
    {
        float speed = new Vector2(inputManager.horizontalInput, inputManager.verticalInput).magnitude;
        animator.SetFloat("Speed", speed);
    }
}


