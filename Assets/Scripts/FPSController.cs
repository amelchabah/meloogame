using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
// Définition de la classe FPSController qui hérite de MonoBehaviour, ce qui permet à ce script d'être attaché à un GameObject dans Unity.
{
    public PlayerInput playerInput;
    public InputAction moveAction;
    public Camera playerCamera; // Référence à la caméra attachée au joueur
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

    Vector3 moveDirection = Vector3.zero; // Stocke la direction de déplacement du joueur
    float rotationX = 0; // Stocke la rotation de la caméra
    float bobbingTimer = 0; // Timer pour le balancement de la caméra
    float bobbingAmountY = 0; // Stocke l'amplitude du balancement de la caméra
    float movementDirectionY = 0; // Déclaration de movementDirectionY au niveau de la classe

    public bool canMove = true;
    private bool isMoving = false;
    private bool isCrouching = false; // Définir isCrouching comme un booléen

    CharacterController characterController; // Référence au composant CharacterController attaché à l'objet (du joueur)
    Vector3 initialPosition; // Stocke la position initiale du joueur
    // Variable booléenne pour suivre si le joueur est accroupi


    // Variable pour stocker la taille du champ de vision (FOV) de la caméra en mode sniper
    private float sniperFOV = 30f;
    // Variable pour stocker la taille du champ de vision (FOV) de la caméra en mode normal
    private float defaultFOV = 60f;
    private float targetFOV; // Champ de vision cible (sniperFOV ou defaultFOV)
    private float currentFOV; // Champ de vision actuel de la caméra
    public Image sniperOverlay; // Référence à l'Image de l'overlay du viseur de sniper
    // Référence au gameobject du sniper (le gun)
    public GameObject sniperGun;
    public Animator animator; // Référence à l'animator du personnage
    // Vitesse de transition entre les valeurs de FOV
    public float zoomSpeed = 5f;
    private bool isSniperMode = false;

    void Start()
    {
        // Obtenez le composant PlayerInput attaché au GameObject du joueur
        playerInput = GetComponent<PlayerInput>();

        // Associez l'action de déplacement de votre configuration d'entrée à votre action moveAction
        moveAction = playerInput.actions["Move"];
        characterController = GetComponent<CharacterController>(); // Obtient la référence au composant CharacterController attaché à l'objet
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Stocke la position initiale du joueur
        initialPosition = transform.position;
        // Stocke le champ de vision (FOV) par défaut de la caméra
        defaultFOV = playerCamera.fieldOfView;
        // Initialisation de la valeur actuelle du champ de vision
        currentFOV = playerCamera.fieldOfView;
        // Désactiver l'overlay du viseur de sniper au démarrage
        sniperOverlay.gameObject.SetActive(false);

    }

    void Update()
    {
        // Gestion de la logique de mise à jour du jeu
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
        // Obtenez les valeurs d'entrée de déplacement à partir de l'Input System
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        Vector3 forward = transform.forward; // Direction vers l'avant du joueur
        Vector3 right = transform.right; // Direction vers la droite du joueur

        // Déterminez la vitesse de déplacement en fonction de l'entrée de mouvement et de l'état du joueur (marche ou course)0
        bool isRunning = playerInput.actions["Run"].ReadValue<float>() > 0.5f;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.y : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.x : 0;

        // Stockez la composante Y de la direction de déplacement actuelle
        movementDirectionY = moveDirection.y;

        // Calculez la nouvelle direction de déplacement totale
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Modifiez l'amplitude du balancement en fonction de la vitesse de déplacement
        bobbingAmount = isRunning ? 0.05f : 0.01f; // Définit l'amplitude du balancement en fonction de si le joueur court ou marche ou est accroupi

        // Si le joueur est accroupi ou en mode sniper, désactivez le balancement
        if (isCrouching || isSniperMode)
        {
            bobbingAmount = 0;
        }
    }

    void HandleJumping()
    {
        if (canMove && characterController.isGrounded && !isCrouching && !isSniperMode && Input.GetButtonDown("Jump")) // Si le joueur est au sol et appuie sur la touche de saut
        {
            moveDirection.y = jumpPower; // Applique la force de saut vers le haut
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
        characterController.Move(moveDirection * Time.deltaTime); // Applique le déplacement au joueur
        if (canMove)
        {
            // Gérer la rotation de la caméra
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed; // Calcule la rotation de la caméra sur l'axe X
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); // Limite la rotation sur l'axe X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // Applique la rotation à la caméra
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); // Applique la rotation horizontale au joueur
            // Effet de balancement de la caméra
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded) // Si le joueur est en mouvement et au sol
            {
                bobbingTimer += Time.deltaTime * bobbingSpeed; // Incrémente le timer de balancement
                bobbingAmountY = Mathf.Sin(bobbingTimer) * bobbingAmount; // Calcule l'amplitude du balancement
            }
            else
            {
                // Réinitialise le balancement
                bobbingTimer = 0;
                bobbingAmountY = 0;
            }

            // Appliquer le balancement à la caméra
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, bobbingAmountY, playerCamera.transform.localPosition.z);
        }
    }

    void HandleCrouching()
    {
        if (playerInput.actions["Crouch"].triggered) // Si le joueur appuie sur la touche de contrôle
        {
            if (!isCrouching)
            {
                isCrouching = true;
                characterController.height = 1f; // Réduit la hauteur du CharacterController
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;
                moveDirection.y = 0f; // Désactive le saut lorsque le joueur est accroupi
            }
            else // Si le joueur est déjà accroupi
            {
                isCrouching = false;
                characterController.height = 2; // Rétablit la hauteur du CharacterController
                walkSpeed = 5f;
                runSpeed = 10f;
            }
            // Réinitialise le balancement de la caméra
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
        // Gérer l'effet de zoom pour le mode sniper
        // Détermine la valeur cible du champ de vision en fonction de si le bouton droit de la souris est maintenu enfoncé ou non
        targetFOV = Input.GetMouseButton(1) ? sniperFOV : defaultFOV;

        // Ajuste progressivement le champ de vision actuel vers la valeur cible
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomSpeed);
        playerCamera.fieldOfView = currentFOV; // Applique le champ de vision actuel à la caméra

        // Activer ou désactiver l'overlay du viseur de sniper en fonction de l'état du mode sniperFOV
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
        isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
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
