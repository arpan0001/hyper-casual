using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 2.0f; // Speed of movement
    public float range = 5.0f; // Range of movement

    private float startPosX;

    void Start()
    {
        startPosX = transform.position.x;
    }

    void Update()
    {
        // Calculate the new position using Mathf.PingPong
        float newPosX = startPosX + Mathf.PingPong(Time.time * speed, range * 2) - range;
        
        // Update the object's position
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}
