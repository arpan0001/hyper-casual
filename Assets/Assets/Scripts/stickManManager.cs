using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class stickManManager : MonoBehaviour
{
    [SerializeField] private GameObject ch_blood;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;
    private SoundEffectManager soundEffectManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        soundEffectManager = FindObjectOfType<SoundEffectManager>();
    }

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

            PlayHitSound();
            soundEffectManager?.PlayDestroySound();
        }

        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount > 1)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    PlayHitSound();
                    soundEffectManager?.PlayDestroySound();
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
               // transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.PlayerManagerInstance.FormatStickMan);
                PlayHitSound();
                soundEffectManager?.PlayDestroySound();

                CheckPlayerStickmanCount();
                break;

            case "weapongate":
                ActivateWeapon(other);
                break;
        }
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void CheckPlayerStickmanCount()
    {
        PlayerManager.PlayerManagerInstance.UpdateStickmanCount();
        if (PlayerManager.PlayerManagerInstance.transform.childCount == 1)
        {
            PlayerManager.PlayerManagerInstance.LoadTryAgainScene();
        }
    }

    private void ActivateWeapon(Collider other)
    {
        PlayerManager playerManager = PlayerManager.PlayerManagerInstance;

        if (other.CompareTag("nogate"))
        {
            if (playerManager.weapon1 != null)
            {
                playerManager.weapon1.SetActive(true);
                playerManager.w1Activated = true;
                playerManager.w3Activated = false;
                playerManager.w2Activated = false;
                playerManager.w4Activated = false;
            }
        }

        if (other.CompareTag("bombgate"))
        {
            if (playerManager.weapon4 != null)
            {
                playerManager.weapon4.SetActive(true);
                playerManager.w4Activated = true;
                playerManager.w3Activated = false;
                playerManager.w2Activated = false;
                playerManager.w1Activated = false;
            }
        }

        if (other.CompareTag("weapongate"))
        {
            if (playerManager.weapon2 != null)
            {
                playerManager.weapon2.SetActive(true);
                playerManager.w2Activated = true;
            }
        }

        if (other.CompareTag("gun"))
        {
            if (playerManager.weapon3 != null)
            {
                playerManager.weapon3.SetActive(true);
                playerManager.w2Activated = false;
                playerManager.w3Activated = true;
                playerManager.w4Activated = false;
            }
        }
    }
}
