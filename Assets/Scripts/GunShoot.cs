// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class GunShoot : MonoBehaviour
// {
//     // public float damage = 10f;
//     // public float impactForce = 100f;
//     // public float range = 100f;
//     // public float fireRate = 15f; // 15 bullets per second
//     // public Camera FPSCam;
//     [SerializeField] private PlayerInput playerInput; // Ajoutez cette ligne

//     [SerializeField] private float damage = 10f;
//     [SerializeField] private float impactForce = 100f;
//     [SerializeField] private float range = 100f;
//     [SerializeField] private float fireRate = 15f; // 15 bullets per second
//     [SerializeField] private Camera FPSCam;
//     [SerializeField] private ScopeImageController ScopeImageController;

//     // public CrosshairController CrosshairController; // Référence au script CrosshairController
//     private float nextTimeToFire = 1f;
//     // Référence au Canvas contenant le script ScopeImageController
//     // public ScopeImageController ScopeImageController;

//     // private void FixedUpdate()
//     // {
//     //     if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
//     //     {
//     //         nextTimeToFire = Time.time + 1f / fireRate;
//     //         Shoot();
//     //     }
//     // }

//     void Start()
//     {
//         playerInput = GetComponent<PlayerInput>(); // Décommentez cette ligne
//     }

//     void FixedUpdate()
//     {
//         // Vérifie si le joueur appuie sur le bouton de tir, que le temps écoulé est supérieur ou égal au prochain temps de tir,
//         // et que le joueur n'est pas en train de s'accroupir (en appuyant sur la touche de contrôle)
//         // if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !playerInput.actions["Crouch"].ReadValue<float>().Equals(1))
//         if (playerInput.actions["Fire"].ReadValue<float>().Equals(1) && Time.time >= nextTimeToFire && !playerInput.actions["Crouch"].ReadValue<float>().Equals(1))
//         {
//             nextTimeToFire = Time.time + 1f / fireRate;
//             Shoot();
//         }
//     }


//     void Shoot()
//     {
//         RaycastHit hit;
//         if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
//         {
//             // Debug.Log(hit.transform.name);

//             // Vérifie si l'objet touché a le composant TargetableObject
//             TargetableObject targetableObject = hit.collider.GetComponent<TargetableObject>();
//             if (targetableObject != null && targetableObject.isTargetable)
//             {
//                 // Augmente le score si l'objet est ciblable
//                 ScoreManager.Instance.AddScore();
//                 Debug.Log("Score: " + ScoreManager.Instance.Score);

//                 // Rend l'objet non ciblable après une touche
//                 targetableObject.SetTargetable(false);

//                 // Afficher "TryScope" si aucune cible n'est touchée
//                 ScopeImageController.ShowHitScope();
//                 ScopeImageController.PlayScopeZoom();

//                 // Appelle la méthode UpdateCrosshair() avec targetHit = true
//                 // CrosshairController.UpdateCrosshair(true);

//             }
//             else
//             {
//                 // Afficher "TryScope" si aucune cible n'est touchée
//                 ScopeImageController.ShowTryScope();
//                 ScopeImageController.PlayScopeFade();

//                 // Appelle la méthode UpdateCrosshair() avec targetHit = false
//                 // CrosshairController.UpdateCrosshair(false);
//             }

//             // Applique une force à l'objet touché
//             if (hit.rigidbody != null)
//             {
//                 hit.rigidbody.AddForce(-hit.normal * impactForce);
//                 // Debug.Log("hit.rigidbody");

//             }
//         }
//     }

// }


// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class GunShoot : MonoBehaviour
// // {
// //     public float damage = 10f;
// //     public float impactForce = 100f;
// //     public float range = 100f;
// //     public float fireRate = 15f; // 15 bullets per second
// //     public Camera FPSCam;
// //     private float nextTimeToFire = 0f;

// //     private void Update()
// //     {
// //         if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
// //         {
// //             nextTimeToFire = Time.time + 1f / fireRate;
// //             Shoot();
// //         }
// //     }

// //     void Shoot()
// //     {
// //         RaycastHit hit;
// //         if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
// //         {
// //             Debug.Log(hit.transform.name);

// //             if (hit.rigidbody != null)
// //             {
// //                 hit.rigidbody.AddForce(-hit.normal * impactForce);
// //                 Debug.Log("hit.rigidbody");
// //             }
// //         }
// //     }
// // }













using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class GunShoot : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float damage = 10f;
    [SerializeField] private float impactForce = 100f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private Camera FPSCam;
    [SerializeField] private ScopeImageController ScopeImageController;
    private float nextTimeToFire = 1f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        if (playerInput.actions["Fire"].ReadValue<float>().Equals(1) && Time.time >= nextTimeToFire && !playerInput.actions["Crouch"].ReadValue<float>().Equals(1))
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
        {
            TargetableObject targetableObject = hit.collider.GetComponent<TargetableObject>();
            if (targetableObject != null && targetableObject.isTargetable)
            {
                ScoreManager.Instance.AddScore();
                Debug.Log(ScoreManager.Instance.Score);
                targetableObject.SetTargetable(false);
                ScopeImageController.ShowHitScope();
                ScopeImageController.PlayScopeZoom();
            }
            else
            {
                ScopeImageController.ShowTryScope();
                ScopeImageController.PlayScopeFade();
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
