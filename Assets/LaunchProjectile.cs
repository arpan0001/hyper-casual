using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectile;
    public float launchVelocity = 10f;
    public float firingInterval = 1f; // Interval time in seconds
    private bool targetInRange = false;
    private bool isFiring = false;

    void Update()
    {
        if (targetInRange && !isFiring)
        {
            StartCoroutine(FireProjectiles());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            targetInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            targetInRange = false;
            StopCoroutine(FireProjectiles());
            isFiring = false;
        }
    }

    IEnumerator FireProjectiles()
    {
        isFiring = true;
        while (targetInRange)
        {
            Launch();
            yield return new WaitForSeconds(firingInterval);
        }
        isFiring = false;
    }

    void Launch()
    {
        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);
        _projectile.GetComponent<Rigidbody>().velocity = launchPoint.up * launchVelocity;
    }
}
