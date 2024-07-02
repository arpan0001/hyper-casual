using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneWeaponActive : MonoBehaviour
{
    public GameObject w1;
    public GameObject w2;
    public GameObject w3;
    private PlayerManager manager;

    // Start is called before the first frame update
    void Start()
    {
        w1.SetActive(true);
        w2.SetActive(false);

        
        GameObject playerManagerObject = GameObject.FindWithTag("Player");
        if (playerManagerObject != null)
        {
            manager = playerManagerObject.GetComponent<PlayerManager>();
        }
        else
        {
            Debug.LogError("PlayerManager not found in the scene with tag 'PlayerManager'.");
        }
    }

    
    void Update()
    {
        if (manager != null && manager.w2Activated)
        {
            Debug.Log("Weapon 2 Activated");
            w2.SetActive(true);
            w1.SetActive(false);
        }
        if (manager != null && manager.w3Activated)
        {
            Debug.Log("Weapon 3 Activated");
            w3.SetActive(true);
            w1.SetActive(false);
            w2.SetActive(false);

        }
    }
}
