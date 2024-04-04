using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction moveAction;
    public Camera playerCamera;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 2f;
    public float jumpPower = 4f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    // Animation lorsque le joueur court (pour un effet plus réaliste)
    public float bobbingAmount = 0.01f;
    public float bobbingSpeed = 25f;

    Vector3 moveDirection = Vector3.zero; 
    float rotationX = 0;
    float bobbingTimer = 0; 
    float bobbingAmountY = 0;
    float movementDirectionY = 0;

    public bool canMove = true;
    private bool isMoving = false;
    private bool isCrouching = false;

    CharacterController characterController;
    Vector3 initialPosition; 

    private float sniperFOV = 30f;
    private float defaultFOV = 60f;
    private float targetFOV;
    private float currentFOV;
    public Image sniperOverlay;
    public GameObject sniperGun;
    public Animator animator;

    public float zoomSpeed = 5f;
    private bool isSniperMode = false;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        initialPosition = transform.position;
        defaultFOV = playerCamera.fieldOfView;
        currentFOV = playerCamera.fieldOfView;
        sniperOverlay.gameObject.SetActive(false);

    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleRotation();
        HandleCrouching();
        HandleSniperMode();
        HandleRespawn();
        HandleAnimation();
    }
    void HandleMovement()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 right = transform.right; 

        bool isRunning = playerInput.actions["Run"].ReadValue<float>() > 0.5f;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.y : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.x : 0;

        movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        bobbingAmount = isRunning ? 0.05f : 0.01f; 

        if (isCrouching || isSniperMode)
        {
            bobbingAmount = 0;
        }
    }

    void HandleJumping()
    {
        if (canMove && characterController.isGrounded && !isCrouching && !isSniperMode && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Applique la gravité au joueur
        }
    }

    void HandleRotation()
    {
        characterController.Move(moveDirection * Time.deltaTime);
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
            {
                bobbingTimer += Time.deltaTime * bobbingSpeed;
                bobbingAmountY = Mathf.Sin(bobbingTimer) * bobbingAmount;
            }
            else
            {
                bobbingTimer = 0;
                bobbingAmountY = 0;
            }
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, bobbingAmountY, playerCamera.transform.localPosition.z);
        }
    }

    void HandleCrouching()
    {
        if (playerInput.actions["Crouch"].triggered)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                characterController.height = 1f; 
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;
                moveDirection.y = 0f;
            }
            else
            {
                isCrouching = false;
                characterController.height = 2; 
                walkSpeed = 5f;
                runSpeed = 10f;
            }
            bobbingTimer = 0;
            bobbingAmountY = 0;
        }
    }


    void HandleRespawn()
    {
        if (transform.position.y < -10f)
        {
            transform.position = initialPosition;
        }
    }

    void HandleSniperMode()
    {
        targetFOV = Input.GetMouseButton(1) ? sniperFOV : defaultFOV;
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomSpeed);
        playerCamera.fieldOfView = currentFOV;
        sniperOverlay.gameObject.SetActive(Input.GetMouseButton(1));

        if (Input.GetMouseButton(1))
        {
            isSniperMode = true;
            sniperGun.SetActive(false);
        }
        else
        {
            isSniperMode = false;
            sniperGun.SetActive(true);
        }
    }


    void HandleAnimation()
    {
        bool isMoving = (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !isCrouching;

        if (isMoving)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }

    }

}
