using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerLocoMotion : MonoBehaviour
{
    public TMP_Text superJumpAlert;
    public TMP_Text superSpeedAlert;

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
    public float movementSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;
    public float maxSpeed = 8f;
    public float acceleration = 5f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float superJumpHeightIncrease = 1f;
    public float gravityIntensity = -15;

    [Header("SuperSpeed")]
    public float superSpeedMultiplier = 1.5f; // Increased speed for the superpower
    public float superSpeedDuration = 8f; // Duration of the super speed effect
    private bool hasSuperSpeedEffect;

    private float rotationSmoothVelocity;
    private float moveSpeedSmoothVelocity;
    private bool hasSuperJumpEffect;
    private float superJumpDuration = 10f; // Duration of the super jump effect

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

        if (hasSuperSpeedEffect)
        {
            moveDirection = Vector3.Lerp(moveDirection, moveDirection * superSpeedMultiplier, Time.deltaTime * 5f);
        }

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
            float actualJumpHeight = jumpHeight;

            if (CheckForSuperJump())
            {
                actualJumpHeight += superJumpHeightIncrease;
                Debug.Log("Super Jump activated! Increased jump height by " + superJumpHeightIncrease);

                hasSuperJumpEffect = true;

                //StartCoroutine(RemoveSuperJumpEffect());
            }

            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * actualJumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }
    }

    private IEnumerator RemoveSuperJumpEffect()
    {
        yield return new WaitForSeconds(superJumpDuration);

        hasSuperJumpEffect = false;
        Debug.Log("Super Jump effect removed.");

        // Reset the jump height to the original value after the super jump duration
        jumpHeight -= superJumpHeightIncrease;
    }

    private bool CheckForSuperJump()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("SuperJump"))
            {
                // Destroy the object tagged as SuperJump
                Destroy(col.gameObject);

                StartCoroutine(ShowMessage(superJumpAlert));

                // Increase the jump height for the super jump duration
                jumpHeight += superJumpHeightIncrease;
                //StartCoroutine(RemoveSuperJumpEffect());
                return true;
            }
        }
        return false;
    }

    private IEnumerator RemoveSuperSpeedEffect()
    {
        float originalMaxSpeed = 8f;
        float currentSpeed = Mathf.Clamp(playerRigidbody.velocity.magnitude, 0f, originalMaxSpeed);
        yield return new WaitForSeconds(superSpeedDuration);

        hasSuperSpeedEffect = false;
        Debug.Log("Super Speed effect removed.");

        // Reset the max speed to the original value after the super speed duration
        maxSpeed = originalMaxSpeed;
        playerRigidbody.velocity = playerRigidbody.velocity.normalized * currentSpeed;
    }

    private bool CheckForSuperSpeed()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("SuperSpeed"))
            {
                // Destroy the object tagged as SuperSpeed
                Destroy(col.gameObject);
                StartCoroutine(ShowMessage(superSpeedAlert));

                // Apply super speed effect
                hasSuperSpeedEffect = true;

                maxSpeed = 30f;
                Debug.Log(maxSpeed);
                //StartCoroutine(RemoveSuperSpeedEffect());

                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForSuperJump();
        CheckForSuperSpeed();
    }

    IEnumerator ShowMessage(TMP_Text message)
    {
        message.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        message.gameObject.SetActive(false);
    }
}
