using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObjects : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 0f, -250f * Time.deltaTime, Space.Self);
    }
}
