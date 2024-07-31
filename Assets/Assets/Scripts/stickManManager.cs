
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class stickManManager : MonoBehaviour
{
    [SerializeField] private GameObject ch_blood;
    [SerializeField] private GameObject Explosion;
    
    [SerializeField] private TextMeshPro CounterTxt;
    
    private SoundEffectManager soundEffectManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        
        soundEffectManager = FindObjectOfType<SoundEffectManager>();
        playerManager = PlayerManager.PlayerManagerInstance;
        CounterTxt = playerManager.CounterTxt; 
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "red":
                HandleRedCollision(other);
                break;
            case "jump":
                HandleJumpCollision();
                break;
            case "obstacle":
            case "damage":
                HandleDamageCollision();
                break;
            
        }
    }

    private void HandleRedCollision(Collider other)
    {
        if (other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            if (playerManager.w4Activated)
            {
                Instantiate(Explosion, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(ch_blood, transform.position, Quaternion.identity);
            }

            
            soundEffectManager?.PlayDestroySound();
            CheckPlayerStickmanCount();
        }
    }

    private void HandleJumpCollision()
    {
        transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(playerManager.FormatStickMan);
    }

    private void HandleDamageCollision()
    {
        Destroy(gameObject);

        if (playerManager.w4Activated)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(ch_blood, transform.position, Quaternion.identity);
        }

        soundEffectManager?.PlayDestroySound();
        CheckPlayerStickmanCount();
    }

   

    public void CheckPlayerStickmanCount()
    {
        int numberOfStickmans = playerManager.transform.childCount - 6;
        CounterTxt.text = numberOfStickmans.ToString();
        
    }
   
}
