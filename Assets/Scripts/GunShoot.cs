// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GunShoot : MonoBehaviour
// {
//     public float damage = 10f;
//     public float impactForce = 100f;
//     public float range = 100f;
//     public float fireRate = 15f; // 15 bullets per second
//     public Camera FPSCam;
//     private float nextTimeToFire = 0f;

//     private void Update()
//     {
//         if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
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
//             Debug.Log(hit.transform.name);

//             if (hit.rigidbody != null)
//             {
//                 hit.rigidbody.AddForce(-hit.normal * impactForce);
//                 Debug.Log("hit.rigidbody");
//             }
//         }
//     }
// }







using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public float damage = 10f;
    public float impactForce = 100f;
    public float range = 100f;
    public float fireRate = 15f; // 15 bullets per second
    public Camera FPSCam;
    private float nextTimeToFire = 0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
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
            // Debug.Log(hit.transform.name);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
                // Debug.Log("hit.rigidbody");


                // Vérifie si l'objet touché a le tag "Target"
                if (hit.collider.CompareTag("Target"))
                {
                    // Augmente le score du personnage uniquement pour les objets avec le tag "Target"
                    ScoreManager.Instance.AddScore();

                    // Debug le score du joueur
                    Debug.Log("Score: " + ScoreManager.Instance.Score);
                }
            }
        }
    }
}
