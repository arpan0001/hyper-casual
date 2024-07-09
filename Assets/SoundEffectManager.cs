using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private AudioClip gateSound;
    [SerializeField] private AudioClip gunGateSound;
    [SerializeField] private AudioClip bombGateSound;
    [SerializeField] private AudioClip weaponGateSound;
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private float pitch = 1.0f;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
            PlaySoundEffect(gateSound);
        }
        else if (other.CompareTag("gun"))
        {
            PlaySoundEffect(gunGateSound);
        }
        else if (other.CompareTag("red") || other.CompareTag("obstacle") || other.CompareTag("damage"))
        {
            PlaySoundEffect(destroySound);
        }
        else if (other.CompareTag("bombgate"))
        {
            PlaySoundEffect(bombGateSound);
        }
        else if (other.CompareTag("weapongate"))
        {
            PlaySoundEffect(weaponGateSound);
        }
    }

    public void PlayDestroySound()
    {
        PlaySoundEffect(destroySound);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
            audioSource.PlayOneShot(clip);
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = pitch;
        }
    }
}
