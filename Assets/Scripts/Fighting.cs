/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Same script as PlayerLocoMotion but has added punching and kicking 
//Ask avanish to look at it and see if he can implement it better 
public class Fighting : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffSet = 0.5f;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float movementSmoothTime = 0.1f; // Smoothing time for movement
    public float rotationSmoothTime = 0.1f;  // Smoothing time for rotation
    public float maxSpeed = 8f;
    public float acceleration = 5f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    private float rotationSmoothVelocity; // Variable for rotation smoothing
    private float moveSpeedSmoothVelocity; // Variable for movement speed smoothing

    private void HandleMovement()
    {
        if (isJumping) { return; }

        Vector2 input = new Vector2(inputManager.horizontalInput, inputManager.verticalInput);
        input = Vector2.ClampMagnitude(input, 1f);

        float targetSpeed = maxSpeed * input.magnitude;
        float currentSpeed = Mathf.SmoothDamp(playerRigidbody.velocity.magnitude, targetSpeed, ref moveSpeedSmoothVelocity, movementSmoothTime);

        moveDirection = cameraObject.forward * input.y + cameraObject.right * input.x;
        moveDirection.y = 0;
        moveDirection.Normalize();
        moveDirection *= currentSpeed;

        playerRigidbody.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        if (isJumping) { return; }

        Vector2 input = new Vector2(inputManager.horizontalInput, inputManager.verticalInput);
        if (input.magnitude > 0.1f)
        {
            float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraObject.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();

        if (inputManager.attackInput && isGrounded && !playerManager.isInteracting)
        {
            HandleAttack();
        }
    }

    private void HandleAttack()
    {
        if (inputManager.punchInput)
        {
            animatorManager.PlayTargetAnimation("Punch", true);
        }
        else if (inputManager.kickInput)
        {
            animatorManager.PlayTargetAnimation("Kick", true);
        }
    }

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelcoity = moveDirection;
            playerVelcoity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelcoity;
        }
    }
}*/