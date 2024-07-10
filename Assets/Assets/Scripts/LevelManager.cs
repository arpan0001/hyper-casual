using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    private int currentLevelIndex = 0;

    void Start()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levels.Length)
        {
            Instantiate(levels[currentLevelIndex].levelPrefab);
            Debug.Log("Loaded Level: " + levels[currentLevelIndex].levelName);
            currentLevelIndex++;
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }

    // Call this method to reset the levels and start from the beginning
    public void ResetLevels()
    {
        currentLevelIndex = 0;
        LoadNextLevel();
    }
}
