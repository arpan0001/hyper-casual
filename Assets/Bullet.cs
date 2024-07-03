using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;
    public float destroyTime = 10f;

    void Start()
    {
        MyRb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        Destroy(this.gameObject, destroyTime);
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject go = Instantiate(Explosion, transform.position, transform.rotation);
        go.SetActive(true);
        Destroy(go, destroyTime);
        Destroy(this.gameObject);

        // col.GetComponent<Script>().doDamage();
    }
}
