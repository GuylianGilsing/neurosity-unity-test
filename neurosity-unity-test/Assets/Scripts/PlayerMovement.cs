using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings movementSettings;

    private Vector3 horizontalMovement = Vector3.zero;
    private Vector3 verticalMovement = Vector3.zero;

    private float startAccelerationTimestamp = 0.0f;
    private float startDecelerationTimestamp = 0.0f;
    private float turnSmoothVelocity;

    private Vector2 movementInput = Vector2.zero;

    private Camera camera;
    private CharacterController controller;


    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        this.movementInput = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        this.camera = this.GetComponent<PlayerInput>().camera;
        this.controller = this.GetComponent<CharacterController>();

        // Hide the mouse and keep it in the center of the window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!this.movementSettings)
        {
            return;
        }

        // Get seperate horizontal and vertical movement vectors
        this.horizontalMovement = this.GetHorizontalMovement(this.horizontalMovement);
        this.verticalMovement = this.GetVerticalMovement(this.verticalMovement);

        // Combine horizontal and vertical movement into one final movement vector
        Vector3 movement = new Vector3(
            this.horizontalMovement.x + this.verticalMovement.x,
            this.horizontalMovement.y + this.verticalMovement.y,
            this.horizontalMovement.z + this.verticalMovement.z
        );

        // Move player
        this.controller.Move(movement * Time.deltaTime);
    }

    private Vector3 GetHorizontalMovement(Vector3 existingMovement)
    {
        Vector3 horizontalMovement = existingMovement;

        if(this.movementInput != Vector2.zero)
        {
            if(this.startAccelerationTimestamp == 0)
            {
                this.startAccelerationTimestamp = Time.time;
            }

            // PlayFootStepAudio();

            Vector3 moveDirection = this.GetMoveDirectionInDirectionOfCamera(this.movementInput, this.camera, ref this.turnSmoothVelocity, this.movementSettings.turnSmoothTime);
            moveDirection *= this.movementSettings.moveSpeed;

            moveDirection = this.ApplyAccelerationCurve(moveDirection, this.startAccelerationTimestamp);
            horizontalMovement = moveDirection;

            return horizontalMovement;
        }

        this.startAccelerationTimestamp = 0;

        if(this.startDecelerationTimestamp == 0)
        {
            this.startDecelerationTimestamp = Time.time;
        }

        horizontalMovement = this.ApplyDecelerationCurve(horizontalMovement, this.startDecelerationTimestamp);

        if(horizontalMovement == Vector3.zero)
        {
            this.startDecelerationTimestamp = 0;
        }

        return horizontalMovement;
    }

    private Vector3 GetVerticalMovement(Vector3 existingMovement)
    {
        Vector3 verticalMovement = existingMovement;

        float gravity = this.movementSettings.gravity * this.movementSettings.gravityMultiplier;

        // Handle consistent gravity
        if (verticalMovement.y > -gravity)
        {
            verticalMovement.y -= gravity * Time.deltaTime;
        }

        if(this.controller.isGrounded)
        {
            verticalMovement.y = 0.0f;

            // this.ResetDoubleJump();
            // this.PlayLandSound();
        }

        return verticalMovement;
    }

    private Vector3 GetMoveDirectionInDirectionOfCamera(Vector2 initialMoveDirection, Camera cam, ref float turnSmoothVelocity, float turnSmoothTime)
    {
        float targetAngle = Mathf.Atan2(initialMoveDirection.x, initialMoveDirection.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        float _angleY = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, _angleY, 0f);

        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    private Vector3 ApplyAccelerationCurve(Vector3 input, float timeStamp)
    {
        return this.ApplyAnimationCurveToVector3(this.movementSettings.normalAcceleration, input, timeStamp);
    }

    private Vector3 ApplyDecelerationCurve(Vector3 input, float timeStamp)
    {
        return this.ApplyAnimationCurveToVector3(this.movementSettings.normalDeceleration, input, timeStamp);
    }

    private Vector3 ApplyAnimationCurveToVector3(AnimationCurve curve, Vector3 input, float timeStamp)
    {
        return new Vector3(
            this.ApplyAnimationCurve(curve, input.x, timeStamp),
            this.ApplyAnimationCurve(curve, input.y, timeStamp),
            this.ApplyAnimationCurve(curve, input.z, timeStamp)
        );
    }

    private float ApplyAnimationCurve(AnimationCurve curve, float input, float timeStamp)
    {
        return input * curve.Evaluate(Time.time - timeStamp);
    }
}
