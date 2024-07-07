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
            var multiplier = other.gameObject.name;
            float rewardAmount = 500 * float.Parse(multiplier);
            
            rewardToShow.text = rewardAmount.ToString();
            PlayerPrefs.SetFloat("reward", rewardAmount);

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
