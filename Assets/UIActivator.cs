using UnityEngine;
using System.Collections;

public class UIActivator : MonoBehaviour
{
    public void ActivateUIWithDelay(GameObject uiElement, float delay)
    {
        StartCoroutine(ActivateUIAfterDelay(uiElement, delay));
    }

    private IEnumerator ActivateUIAfterDelay(GameObject uiElement, float delay)
    {
        yield return new WaitForSeconds(delay);
        uiElement.SetActive(true);
    }
}
