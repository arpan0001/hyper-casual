using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    public TextMeshPro GateNo; // Reference to a TextMeshPro component that displays the gate number.
    public int randomNumber;   // Stores the generated random number.
    public bool multiply;      // Determines whether to display a multiplication factor or a random number.

    void Start()
    {
        if (multiply)
        {
            // If 'multiply' is true, generate a random number between 1 and 6 (inclusive)
            randomNumber = Random.Range(1, 7);
            
            // Set the text of GateNo to "X" followed by the random number
            GateNo.text = "X" + randomNumber;
        }
        else
        {
            // If 'multiply' is false, generate a random number between 40 and 59 (inclusive)
            randomNumber = Random.Range(40, 60);
            
            // Set the text of GateNo to the generated random number
            GateNo.text = randomNumber.ToString();
        }
    }
}
