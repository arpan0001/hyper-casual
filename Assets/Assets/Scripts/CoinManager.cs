using UnityEngine;
using TMPro;
using DailyRewardSystem;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private TextMeshProUGUI coinsText;

    private int coins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinsText();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        PlayerPrefs.SetInt("Coins", coins);
        UpdateCoinsText();

        // Notify other scripts about the coin update
        if (DailyRewards.Instance != null)
        {
            DailyRewards.Instance.UpdateCoinsTextUI();
        }

        if (CoinReward.CoinRewardInstance != null)
        {
            CoinReward.CoinRewardInstance.UpdateCoinCounter();
        }

        if (Game.Instance != null)
        {
            Game.Instance.UpdateAllCoinsUIText();
        }
    }

    public void SubtractCoins(int amount)
    {
        coins -= amount;
        PlayerPrefs.SetInt("Coins", coins);
        UpdateCoinsText();

        // Notify other scripts about the coin update
        if (DailyRewards.Instance != null)
        {
            DailyRewards.Instance.UpdateCoinsTextUI();
        }

        if (CoinReward.CoinRewardInstance != null)
        {
            CoinReward.CoinRewardInstance.UpdateCoinCounter();
        }

        if (Game.Instance != null)
        {
            Game.Instance.UpdateAllCoinsUIText();
        }
    }

    public int GetCoins()
    {
        return coins;
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }
}
