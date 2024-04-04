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
    [SerializeField] private float nextTimeToFire = 1f;

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
                // Debug.Log(ScoreManager.Instance.Score);
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
