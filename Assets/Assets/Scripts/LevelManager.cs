using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelData levelData; // Reference to the LevelData ScriptableObject
    private int currentLevelIndex = 0;

    private void Start()
    {
        LoadLevel(currentLevelIndex); // Load the first level at the start
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levelData.levelNames.Length - 1)
        {
            UnloadLevel(currentLevelIndex);
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private void LoadLevel(int index)
    {
        if (index >= 0 && index < levelData.levelNames.Length)
        {
            SceneManager.LoadScene(levelData.levelNames[index], LoadSceneMode.Additive);
        }
    }

    private void UnloadLevel(int index)
    {
        if (index >= 0 && index < levelData.levelNames.Length)
        {
            SceneManager.UnloadSceneAsync(levelData.levelNames[index]);
        }
    }
}
