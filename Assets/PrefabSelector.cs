using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabSelector : MonoBehaviour
{
    public void SelectPrefab(int prefabIndex)
    {
        // Save the selected prefab index to PlayerPrefs
        PlayerPrefs.SetInt("SelectedPrefab", prefabIndex);
        PlayerPrefs.Save();

        // Load the second scene
        SceneManager.LoadScene("Menu"); // Replace with the actual name of the second scene
    }
}
