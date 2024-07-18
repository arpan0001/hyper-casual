using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gatesubstract : MonoBehaviour
{
    public TextMeshPro GateNo; 
    public int randomNumber;   
    public bool substract;    

    void Start()
    {
        if (substract)
        {
            randomNumber = Random.Range(20, 30); 
            GateNo.text = "-" + randomNumber;
        }
    }
}
