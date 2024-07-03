using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneWeaponActive : MonoBehaviour
{
    public Animator character_animator;

    public GameObject w1;
    public GameObject w2;
    public GameObject w3;
    private PlayerManager manager;

    
    void Start()
    {
        character_animator = GetComponent<Animator>();

        character_animator.SetBool("GunRun", false);


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
        if(manager.gameState){
        character_animator.SetBool("run", true);


        }
        if (manager != null && manager.w2Activated)
        {
            character_animator.SetBool("run", true);


            Debug.Log("Weapon 2 Activated");
            w2.SetActive(true);
            w1.SetActive(false);
        }
        if (manager != null && manager.w3Activated)
        {
            character_animator.SetBool("run", false);

            character_animator.SetBool("GunRun", true);

            Debug.Log("Weapon 3 Activated");
            w3.SetActive(true);
            w1.SetActive(false);
            w2.SetActive(false);

        }
    }
}
