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

    // Référence au Canvas contenant le script ScopeImageController
    public ScopeImageController ScopeImageController;


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

            // Vérifie si l'objet touché a le composant TargetableObject
            TargetableObject targetableObject = hit.collider.GetComponent<TargetableObject>();
            if (targetableObject != null && targetableObject.isTargetable)
            {
                // Augmente le score si l'objet est ciblable
                ScoreManager.Instance.AddScore();
                Debug.Log("Score: " + ScoreManager.Instance.Score);

                // Rend l'objet non ciblable après une touche
                targetableObject.SetTargetable(false);

                // Afficher "TryScope" si aucune cible n'est touchée
                ScopeImageController.ShowHitScope();
                ScopeImageController.PlayScopeZoom();



            }
            else
            {
                // Afficher "TryScope" si aucune cible n'est touchée
                ScopeImageController.ShowTryScope();

            }

            // Applique une force à l'objet touché
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
                // Debug.Log("hit.rigidbody");

            }
        }
    }

}
