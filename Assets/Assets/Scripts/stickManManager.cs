using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class stickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem ch_blood;
    [SerializeField] private ParticleSystem Explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            if (PlayerManager.PlayerManagerInstance.w4Activated)
            {
                Instantiate(Explosion, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(ch_blood, transform.position, Quaternion.identity);
            }
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

            case "obstacle":
            case "damage":
                Destroy(gameObject);
                
                if (PlayerManager.PlayerManagerInstance.w4Activated)
                {
                    Instantiate(Explosion, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(ch_blood, transform.position, Quaternion.identity);
                }
                break;
        }

        // if (other.CompareTag("obstacle") || other.CompareTag("damage"))
        // {
        //     PlayerManager.PlayerManagerInstance.UpdateStickmanCount();
        // }
        
    }
}
