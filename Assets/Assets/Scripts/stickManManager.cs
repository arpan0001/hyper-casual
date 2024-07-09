using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
                PlayHitSound();
                soundEffectManager?.PlayDestroySound();
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
}
