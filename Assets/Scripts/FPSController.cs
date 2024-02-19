using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
// Définition de la classe FPSController qui hérite de MonoBehaviour, ce qui permet à ce script d'être attaché à un GameObject dans Unity.

{
    public Camera playerCamera; // Référence à la caméra attachée au joueur
    public float walkSpeed = 6f; // Vitesse de marche du joueur
    public float runSpeed = 12f; // Vitesse de course du joueur
    public float jumpPower = 7f; // Puissance de saut du joueur
    public float gravity = 10f; // Gravité appliquée au joueur


    public float lookSpeed = 2f; // Vitesse de rotation de la caméra
    public float lookXLimit = 45f; // Limite de rotation de la caméra sur l'axe X


    // Animation lorsque le joueur court (pour un effet plus réaliste)
    public float bobbingAmount = 0.03f; // Amplitude du balancement de la caméra
    public float bobbingSpeed = 25f; // Vitesse du balancement de la caméra



    Vector3 moveDirection = Vector3.zero; // Stocke la direction de déplacement du joueur
    float rotationX = 0; // Stocke la rotation de la caméra
    float bobbingTimer = 0; // Timer pour le balancement de la caméra
    float bobbingAmountY = 0; // Stocke l'amplitude du balancement de la caméra


    public bool canMove = true; // Indique si le joueur peut bouger ou non


    CharacterController characterController; // Référence au composant CharacterController attaché à l'objet (du joueur)
    Vector3 initialPosition; // Stocke la position initiale du joueur


    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Obtient la référence au composant CharacterController attaché à l'objet
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
        Cursor.visible = false; // Masque le curseur


        // Stocke la position initiale du joueur
        initialPosition = transform.position;


    }

    void Update()
    {
        // Handles Movement : Gère le déplacement du joueur.
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward); // Transforme la direction vers l'avant relative au joueur
        Vector3 right = transform.TransformDirection(Vector3.right); // Transforme la direction vers la droite relative au joueur

        // Left Shift pour courir (augmente la vitesse de déplacement du joueur)
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Vérifie si la touche Maj gauche est enfoncée
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0; // Calcule la vitesse de déplacement avant/arrière en fonction de la touche enfoncée et de la vitesse de déplacement actuelle
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0; // " gauche/droite
        float movementDirectionY = moveDirection.y; // Stocke la composante Y de la direction de déplacement actuelle
        moveDirection = (forward * curSpeedX) + (right * curSpeedY); // Calcule la nouvelle direction de déplacement totale


        // Modifie l'amplitude du balancement en fonction de la vitesse de déplacement
        bobbingAmount = isRunning ? 0.1f : 0.03f; // Définit l'amplitude du balancement en fonction de si le joueur court ou marche


        #endregion

        // Handles Jumping : Gère le saut du joueur
        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) // Si la touche de saut est enfoncée, que le joueur peut se déplacer et qu'il est au sol
        {
            moveDirection.y = jumpPower; // Applique la force de saut vers le haut
        }
        else
        {
            moveDirection.y = movementDirectionY; // Maintient la composante Y de la direction de déplacement actuelle
        }

        if (!characterController.isGrounded) // Si le joueur n'est pas au sol
        {
            moveDirection.y -= gravity * Time.deltaTime; // Applique la gravité au joueur
        }

        #endregion

        // Handles Rotation : Gère la rotation du joueur
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime); // Applique le déplacement au joueur

        if (canMove) // Si le joueur peut bouger
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

        #endregion


        // Vérifie si le joueur est tombé en dessous d'une certaine hauteur
        if (transform.position.y < -10f)
        {
            Respawn();
        }

    }

    // Fonction pour respawn le joueur à sa position initiale
    void Respawn()
    {
        // Réinitialise la position du joueur à la position initiale
        transform.position = initialPosition;
    }

}
