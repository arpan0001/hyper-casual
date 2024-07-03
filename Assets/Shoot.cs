using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireRate;
    float timer;
    public GameObject SpawnPos;

    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if (timer >= FireRate)
            {
                timer = 0;
                GameObject go = Instantiate(BulletPrefab, SpawnPos.transform.position, SpawnPos.transform.rotation);
                
                go.SetActive(true);
                
            }




        }
    

    }
        
     


        
    
}
