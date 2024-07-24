using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels; // Array to hold references to your level GameObjects
    private int currentLevelIndex = 0;

    void Start()
    {
        // Ensure only the first level is active initially
        LoadLevel(currentLevelIndex);
    }

    public void LoadNextLevel()
    {
        // Deactivate the current level
        if (levels[currentLevelIndex] != null)
        {
            levels[currentLevelIndex].SetActive(false);
        }

        // Increment the level index
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            currentLevelIndex = 0; // Loop back to the first level or handle as per your requirement
        }

        // Activate the next level
        LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int levelIndex)
    {
        // Activate the specified level
        if (levels[levelIndex] != null)
        {
            levels[levelIndex].SetActive(true);
        }

        // Deactivate all other levels
        for (int i = 0; i < levels.Length; i++)
        {
            if (i != levelIndex && levels[i] != null)
            {
                levels[i].SetActive(false);
            }
        }
    }
}
