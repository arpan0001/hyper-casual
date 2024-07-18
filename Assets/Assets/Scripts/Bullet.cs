using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;
    public float destroyTime = 2f;
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

            
        }
    }

    void StopFiring()
    {
        isFiring = false;
        
        MyRb.velocity = Vector3.zero; 
        MyRb.isKinematic = true; 
    }
}
