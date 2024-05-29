using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInputActions playerControls;
    public Vector2 movementInput;
    public Vector2 lookInput;
    public float verticalInput;
    public float horizontalInput;
    public float mouseX;
    public float mouseY;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
            playerControls.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleLookInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    private void HandleLookInput()
    {
        mouseX = lookInput.x;
        mouseY = lookInput.y;
    }
}

