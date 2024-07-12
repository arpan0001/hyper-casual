using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explosionEffect; // Assign your explosion particle system prefab in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red")) // Check if the bomb collides with an object tagged as "red"
        {
            ActivateExplosion();
        }
    }

    void ActivateExplosion()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Destroy the bomb after triggering the explosion
    }
}
