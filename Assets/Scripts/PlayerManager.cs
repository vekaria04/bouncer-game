using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    PlayerLocoMotion playerLocoMotion;
    CameraManager cameraManager;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocoMotion = GetComponent<PlayerLocoMotion>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocoMotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        isInteracting = animator.GetBool("isInteracting");

        playerLocoMotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocoMotion.isGrounded);
    }
}
