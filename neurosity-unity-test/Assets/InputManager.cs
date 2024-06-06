using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Instance of the PlayerInputActions class to manage input actions
    PlayerInputActions playerControls;
    // Variables to store movement and look input values
    public Vector2 movementInput;
    public Vector2 lookInput;
    public float verticalInput;
    public float horizontalInput;
    public float mouseX;
    public float mouseY;

    // Called when the script is enabled
    private void OnEnable()
    {
        // Initialize playerControls if not already done
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
            // Set up callbacks for movement and look input
            playerControls.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        }
        // Enable the input actions
        playerControls.Enable();
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        // Disable the input actions
        playerControls.Disable();
    }

    // Method to handle all input processing
    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleLookInput();
    }

    // Method to handle movement input processing
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    // Method to handle look input processing
    private void HandleLookInput()
    {
        mouseX = lookInput.x;
        mouseY = lookInput.y;
    }
}

