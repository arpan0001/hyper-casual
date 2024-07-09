using UnityEngine;

public class destroysound: MonoBehaviour
{
    [SerializeField] private AudioClip DestroySound;
    
    [SerializeField] private float pitch = 1.0f; // Default pitch is 1 (normal speed)

    private AudioSource audioSource;

    void Start()
    {
        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set up the AudioSource properties for immediate playback
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red"))
        {
            PlaySoundEffect(DestroySound);
        }
        else if (other.CompareTag("obstacle"))
        {
            PlaySoundEffect(DestroySound);
        }
        
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = pitch;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

