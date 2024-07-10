using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public GameObject levelPrefab;
    public string levelName;
    public int levelIndex;
}
