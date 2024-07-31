using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections;

namespace DailyRewardSystem
{
    public enum RewardType
    {
        Coins,
    }

    [Serializable]
    public struct Reward
    {
        public RewardType Type;
        public int Amount;
    }

    public class DailyRewards : MonoBehaviour
    {
        public static DailyRewards Instance;

        [Header("Main Menu")]
        [SerializeField] TextMeshProUGUI coinsText;

        [Space]
        [Header("Reward UI")]
        [SerializeField] GameObject rewardsCanvas;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;
        [SerializeField] Image rewardImage;
        [SerializeField] TextMeshProUGUI rewardAmountText;
        [SerializeField] Button claimButton;
        [SerializeField] GameObject rewardsNotification;
        [SerializeField] GameObject noMoreRewardsPanel;

        [Space]
        [Header("Rewards Images")]
        [SerializeField] Sprite iconCoinsSprite;

        [Space]
        [Header("Rewards Database")]
        [SerializeField] RewardsDatabase rewardsDB;

        [Space]
        [Header("FX")]
        [SerializeField] ParticleSystem fxCoins;

        [Space]
        [Header("Timing")]
        [SerializeField] int nextRewardDelayMinutes = 0;
        [SerializeField] int nextRewardDelaySeconds = 10;
        [SerializeField] float checkForRewardDelay = 2f;

        private int nextRewardIndex;
        private bool isRewardReady = false;

        void Start()
        {
            if (Instance == null)
                Instance = this;

            Initialize();

            StopAllCoroutines();
            StartCoroutine(CheckForRewards());
        }

        void Initialize()
        {
            nextRewardIndex = PlayerPrefs.GetInt("Next_Reward_Index", 0);

            UpdateCoinsTextUI();

            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(OnOpenButtonClick);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButtonClick);

            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(OnClaimButtonClick);

            if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime")))
            {
                PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
            }
        }

        IEnumerator CheckForRewards()
        {
            while (true)
            {
                if (!isRewardReady)
                {
                    DateTime currentDatetime = DateTime.Now;
                    DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));

                    TimeSpan elapsed = currentDatetime - rewardClaimDatetime;

                    double elapsedSeconds = elapsed.TotalSeconds;

                    double nextRewardDelayInSeconds = (nextRewardDelayMinutes * 60) + nextRewardDelaySeconds;

                    if (elapsedSeconds >= nextRewardDelayInSeconds)
                        ActivateReward();
                    else
                        DeactivateReward();
                }

                yield return new WaitForSeconds(checkForRewardDelay);
            }
        }

        void ActivateReward()
        {
            isRewardReady = true;

            noMoreRewardsPanel.SetActive(false);
            rewardsNotification.SetActive(true);

            Reward reward = rewardsDB.GetReward(nextRewardIndex);

            rewardImage.sprite = iconCoinsSprite;
            rewardAmountText.text = string.Format("+{0}", reward.Amount);
        }

        void DeactivateReward()
        {
            isRewardReady = false;

            noMoreRewardsPanel.SetActive(true);
            rewardsNotification.SetActive(false);
        }

        void OnClaimButtonClick()
        {
            Reward reward = rewardsDB.GetReward(nextRewardIndex);

            if (reward.Type == RewardType.Coins)
            {
                Debug.Log("<color=yellow>" + reward.Type.ToString() + " Claimed : </color>+" + reward.Amount);
                CoinManager.Instance.AddCoins(reward.Amount); // Update coins in CoinManager
                fxCoins.Play();
                UpdateCoinsTextUI();
            }

            isRewardReady = false;

            nextRewardIndex++;
            if (nextRewardIndex >= rewardsDB.rewardsCount)
                nextRewardIndex = 0;

            PlayerPrefs.SetInt("Next_Reward_Index", nextRewardIndex);
            PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());

            DeactivateReward();
        }

        public void UpdateCoinsTextUI()
        {
            coinsText.text = CoinManager.Instance.GetCoins().ToString();
        }

        void OnOpenButtonClick()
        {
            rewardsCanvas.SetActive(true);
        }

        void OnCloseButtonClick()
        {
            rewardsCanvas.SetActive(false);
        }
    }
}
