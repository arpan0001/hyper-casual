using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireRate;
    private float timer;
    public GameObject SpawnPos;
    private bool isShooting;
    public AudioSource shootingAudio; // Reference to the AudioSource component

    void Start()
    {
        timer = 0;
        isShooting = false;
    }

    void Update()
    {
        // Check the number of objects with the tag "red"
        if (CountRedObjects() == 0)
        {
            StopShooting();
        }

        if (isShooting)
        {
            timer += Time.deltaTime;
            if (timer >= FireRate)
            {
                timer = 0;
                GameObject go = Instantiate(BulletPrefab, SpawnPos.transform.position, SpawnPos.transform.rotation);
                go.SetActive(true);

                // Play shooting audio
                PlayShootingAudio();
            }
        }
    }

    public void StartShooting()
    {
        StartCoroutine(StartShootingAfterDelay(0.5f)); // Start shooting after a delay of 0.5 seconds
    }

    private IEnumerator StartShootingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    // Method to count the number of objects with the tag "red"
    private int CountRedObjects()
    {
        return GameObject.FindGameObjectsWithTag("red").Length;
    }

    // Method to play the shooting audio
    private void PlayShootingAudio()
    {
        if (shootingAudio != null)
        {
            if (shootingAudio.clip != null)
            {
                shootingAudio.Play();
            }
            else
            {
                Debug.LogWarning("Shooting audio clip is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Shooting AudioSource component is not assigned.");
        }
    }
}
