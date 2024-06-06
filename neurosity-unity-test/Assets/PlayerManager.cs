using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Reference to the InputManager component
    InputManager inputManager;
    // Reference to the PlayerLocomotion component
    PlayerLocomotion playerLocomotion;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the InputManager component attached to the same GameObject
        inputManager = GetComponent<InputManager>();
        // Get the PlayerLocomotion component attached to the same GameObject
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    // Called once per frame
    private void Update()
    {
        // Handle all input in the InputManager
        inputManager.HandleAllInput();
    }

    // Called at a fixed time interval (used for physics updates)
    private void FixedUpdate()
    {
        // Handle all movement in the PlayerLocomotion
        playerLocomotion.HandleAllMovement();
    }
}
