using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;

    void Awake ()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    [SerializeField] Text[] allCoinsUIText;

    void Start ()
    {
        UpdateAllCoinsUIText ();
    }

    public void UseCoins (int amount)
    {
        CoinManager.Instance.SubtractCoins(amount); // Update coins in CoinManager
    }

    public bool HasEnoughCoins (int amount)
    {
        return (CoinManager.Instance.GetCoins() >= amount); // Check coins in CoinManager
    }

    public void UpdateAllCoinsUIText ()
    {
        for (int i = 0; i < allCoinsUIText.Length; i++) {
            allCoinsUIText [i].text = CoinManager.Instance.GetCoins().ToString(); // Update from CoinManager
        }
    }
}
