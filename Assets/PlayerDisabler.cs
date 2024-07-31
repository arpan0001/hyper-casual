using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    private PlayerManager playerManager;

    [SerializeField] private GameObject gameOverUI; // Reference to the UI element to activate
    [SerializeField] private float uiActivationDelay = 2f; // Delay before activating UI

    private UIActivator uiActivator;

    void Start()
    {
        // Get the PlayerManager component from the same GameObject
        playerManager = GetComponent<PlayerManager>();

        // Ensure the UI is initially inactive
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Find the UIActivator in the scene
        uiActivator = FindObjectOfType<UIActivator>();

        if (uiActivator == null)
        {
            Debug.LogError("UIActivator not found in the scene.");
        }
    }

    void Update()
    {
        CheckStickmenCount();
    }

    void CheckStickmenCount()
    {
        // Check if there are no stickmen left (assuming non-stickmen children count is 4)
        if (transform.childCount <= 5) // Adjust based on how many non-stickmen children there are
        {
            DisablePlayer();
        }
    }

    void DisablePlayer()
    {
        if (playerManager != null)
        {
            playerManager.gameState = false;
            gameObject.SetActive(false);

            // Trigger the UI activation in the UIActivator
            if (uiActivator != null && gameOverUI != null)
            {
                uiActivator.ActivateUIWithDelay(gameOverUI, uiActivationDelay);
            }
        }
    }
}
