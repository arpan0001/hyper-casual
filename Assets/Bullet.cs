using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;
    public float destroyTime = 10f;
    private bool isFiring = true;

    void Start()
    {
        MyRb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        Destroy(this.gameObject, destroyTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (isFiring)
        {
            GameObject go = Instantiate(Explosion, transform.position, transform.rotation);
            go.SetActive(true);
            Destroy(go, destroyTime);

            if (col.CompareTag("red"))
            {
                Destroy(col.gameObject);
            }
            else
            {
                StopFiring();
            }

            Destroy(this.gameObject);

            // col.GetComponent<Script>().doDamage();
        }
    }

    void StopFiring()
    {
        isFiring = false;
        // Add any additional logic to stop the bullet from firing, such as disabling its Rigidbody or removing it from any firing lists.
        MyRb.velocity = Vector3.zero; // Stop the bullet's movement
        MyRb.isKinematic = true; // Make the Rigidbody kinematic to stop further physics interactions
    }
}
