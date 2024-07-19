using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    public LevelManager levelManager; // Reference to the LevelManager

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnNextLevelButtonClicked()
    {
        levelManager.LoadNextLevel();
    }
}
