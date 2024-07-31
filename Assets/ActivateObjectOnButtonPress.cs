using UnityEngine;
using UnityEngine.UI;

public class ActivateObjectOnButtonPress : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public GameObject objectToActivate; // Reference to the GameObject to activate

    void Start()
    {
        if (button != null && objectToActivate != null)
        {
            // Add a listener to the button to call the ActivateObject method when clicked
            button.onClick.AddListener(ActivateObject);
        }
        else
        {
            Debug.LogError("Button or ObjectToActivate is not assigned.");
        }
    }

    void ActivateObject()
    {
        // Activate the GameObject
        objectToActivate.SetActive(true);
    }
}
