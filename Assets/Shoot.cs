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
            }
        }
    }

    public void StartShooting()
    {
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
}
