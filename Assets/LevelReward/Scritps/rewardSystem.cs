using System;
using TMPro;
using UnityEngine;

public class rewardSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardToShow;
    [SerializeField] private Transform Hand;
    [SerializeField] private Animator handAnim;

    void Start()
    {
        handAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rewardNo"))
        {
            var coinToAdd = other.gameObject.name;
            float rewardAmount = float.Parse(coinToAdd);

            // Add the reward amount to the current reward
            float currentReward = PlayerPrefs.GetFloat("reward", 0);
            float newRewardAmount = currentReward + rewardAmount;
            
            rewardToShow.text = newRewardAmount.ToString();
            PlayerPrefs.SetFloat("reward", newRewardAmount);

            // Update the CoinManager with the new reward amount
            CoinManager.Instance.AddCoins((int)rewardAmount);
        }
    }

    public void GetTheReward()
    {
        CoinReward.CoinRewardInstance.CountCoins();
        handAnim.enabled = false;
    }
}
