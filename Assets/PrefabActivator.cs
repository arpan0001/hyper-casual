using UnityEngine;

public class PrefabActivator : MonoBehaviour
{
    public GameObject[] prefabs;

    void Start()
    {
        // Get the selected avatar index from PlayerPrefs
        int selectedIndex = PlayerPrefs.GetInt("SelectedAvatarIndex", -1);

        // Activate the selected prefab and deactivate the others
        if (selectedIndex >= 0 && selectedIndex < prefabs.Length)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].SetActive(i == selectedIndex);
            }
        }
        else
        {
            // If no valid index, deactivate all prefabs
            foreach (var prefab in prefabs)
            {
                prefab.SetActive(false);
            }
        }
    }
}
