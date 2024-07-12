using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverChecker : MonoBehaviour
{
    [SerializeField] private float checkInterval = 1f; // How often to check for objects with tags
    private bool gameState = true;

    void Start()
    {
        StartCoroutine(CheckForGameOver());
    }

    IEnumerator CheckForGameOver()
    {
        while (gameState)
        {
            yield return new WaitForSeconds(checkInterval);

            GameObject[] blueObjects = GameObject.FindGameObjectsWithTag("blue");
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("player");

            if (blueObjects.Length == 0 && playerObjects.Length == 0)
            {
                gameState = false;
                LoadTryAgainScene();
            }
        }
    }

    private void LoadTryAgainScene()
    {
        SceneManager.LoadScene("TryAgain");
    }
}
