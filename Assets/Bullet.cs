using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;

    // Start is called before the first frame update

   void Start()
   {
     MyRb. AddForce(transform. forward * speed);
     Destroy(this.gameObject, 4);
   }
 
 void OnTriggerEnter(Collider col)
 {
    
   GameObject go = Instantiate(Explosion, transform.position, transform.rotation);
   go.SetActive(true);
   Destroy(go, 1);
   Destroy(this.gameObject);

   //col .GetComponent<Script>() .doDamage ()

   // Update is called once per frame
 }

  void Update()
  {

   }

}
