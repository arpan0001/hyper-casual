using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinReward : MonoBehaviour
{
    public static CoinReward CoinRewardInstance;
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;

    void Start()
    {
        if (coinsAmount == 0) 
            coinsAmount = 10; // Set a default value if not initialized

        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];
        
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }

        CoinRewardInstance = this;
    }

    public void CountCoins()
    {
        pileOfCoins.SetActive(true);
        var delay = 0f;
        
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).position = initialPos[i];
            pileOfCoins.transform.GetChild(i).rotation = initialRotation[i];

            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(400f, 840f), 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);

            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);

            pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;

            CoinManager.Instance.AddCoins(1); // Update coins in CoinManager
        }

        StartCoroutine(countCoins());
    }

    IEnumerator countCoins()
    {
        yield return new WaitForSecondsRealtime(2f);
        counter.text = PlayerPrefs.GetFloat("reward").ToString();
    }

    public void UpdateCoinCounter()
    {
        counter.text = CoinManager.Instance.GetCoins().ToString();
    }
}
