using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class stickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem ch_blood;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            Instantiate(ch_blood, transform.position, Quaternion.identity);
        }

        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount > 1)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;

            case "jump":
             transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.PlayerManagerInstance.FormatStickMan);
             break;

                other.gameObject.GetComponent<memeberManager>().member = true;  
        }

        if (other.GetComponent<Collider>().CompareTag("obstacle"))
        {
            Destroy(gameObject);

            Instantiate(ch_blood, transform.position, Quaternion.identity);
            
            
        }

        if (other.GetComponent<Collider>().CompareTag("damage"))
        {
            Destroy(gameObject);

            Instantiate(ch_blood, transform.position, Quaternion.identity);
            
            
        }
    }
}