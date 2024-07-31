using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;

public class RewardBox : MonoBehaviour
{
    public enum UserRewardType
    {
        Coins
    }

    [Serializable]
    public struct UserReward
    {
        public UserRewardType RewardType;
        public Sprite Icon;
        public int Amount;
    }

    [Header("UI References")]
    [SerializeField] GameObject rewardBoxUICanvas;
    [SerializeField] Transform rewardsParent;
    [SerializeField] Transform rewardsCheckMarksParent;
    [SerializeField] GameObject noMoreRewardsPanel;

    [Header("Progress Bar UI")]
    [SerializeField] Image progressBarFill;

    [Header("Remaining Ads UI & Watch Ad Button")]
    [SerializeField] GameObject remainingAdsBadge;
    [SerializeField] Button watchAdButton;
    [SerializeField] Text watchedAdsText;
    [SerializeField] Text coinsText;

    [Header("Rewards FX")]
    [SerializeField] ParticleSystem coinsRewardFx;

    [Header("Admob Reference")]
    //[SerializeField] admob admob;

    [Header("Rewards Activation Time (Minutes)")]
    public double waitTimeToActivateRewards;

    [Header("Rewards Information")]
    const int TOTAL_REWARDS = 6;
    public UserReward[] userRewards = new UserReward[TOTAL_REWARDS];

    private static UserReward currentReward;
    private static int currentRewardIndex = 0;
    private bool isAdWatched;

    private Text remainingAdsBadgeText;
    private Text watchAdButtonText;
    private string watchAdButtonDefaultText;

    void Awake()
    {
        // Get the remaining ads text UI element inside the remainingAdsBadge.
        remainingAdsBadgeText = remainingAdsBadge.transform.GetChild(0).GetComponent<Text>();

        // Get watched ads Text UI element and save the default text of the button
        watchAdButtonText = watchAdButton.transform.GetChild(0).GetComponent<Text>();
        watchAdButtonDefaultText = watchAdButtonText.text;
    }

    void Start()
    {
        CheckForAvailableRewards();
        DrawRewardsUI();
        UpdateCoinsTextUI();
        UpdateRemainingRewardsTextUI();
        UpdateWatchedADsTextUI();
    }

    void DrawRewardsUI()
    {
        for (int i = currentRewardIndex; i < TOTAL_REWARDS; i++)
        {
            UserReward reward = userRewards[i];

            // Update UI elements
            // Reward Icon UI
            rewardsParent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = reward.Icon;
            // Reward Amount UI
            rewardsParent.GetChild(i).GetChild(2).GetComponent<Text>().text = reward.Amount.ToString();
        }
    }

    public void WatchAdButtonClick()
    {
        isAdWatched = false;
        watchAdButton.interactable = false;
        watchAdButtonText.text = "LOADING..";

        // Request & Show Ad
#if UNITY_EDITOR
        StartCoroutine(SimulateEditorRequestRewardAd());
#elif UNITY_ANDROID
        // Uncomment the following line when Admob is properly set up
        // admob.RequestRewardAd();
#endif
    }

#if UNITY_EDITOR
    IEnumerator SimulateEditorRequestRewardAd()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.3f));

        isAdWatched = true;
        AdClose();
    }
#endif

    public void AdClose()
    {
        watchAdButtonText.text = watchAdButtonDefaultText;

        // on ad closed
        if (isAdWatched)
        {
            // User watched the full AD
            watchAdButton.interactable = false;
            currentReward = userRewards[currentRewardIndex];
            currentRewardIndex++;
            float progressValue = (float)currentRewardIndex / TOTAL_REWARDS;

            progressBarFill.DOFillAmount(progressValue, 1.5f).OnComplete(RewardUser);
        }
        else
        {
            // User didn't complete the AD
            watchAdButton.interactable = true;
        }
    }

    void RewardUser()
    {
        watchAdButton.interactable = true;

        // Check Reward type
        if (currentReward.RewardType == UserRewardType.Coins)
        {
            // Coins Reward
            Debug.Log("<color=orange>Coins Reward : +" + currentReward.Amount + "</color>");

            // Use CoinManager to add coins
            CoinManager.Instance.AddCoins(currentReward.Amount);

            coinsRewardFx.Play();
        }

        UpdateRemainingRewardsTextUI();
        UpdateWatchedADsTextUI();

        MarkRewardAsChecked(currentRewardIndex - 1);

        // Save Progress
        PlayerPrefs.SetInt("CurrentRewardIndex", currentRewardIndex);

        // Check if it's the last Reward
        if (currentRewardIndex == TOTAL_REWARDS)
        {
            // Save current system DateTime
            PlayerPrefs.SetString("RewardsCompletionDateTime", DateTime.Now.ToString());
        }
    }

    void MarkRewardAsChecked(int rewardIndex)
    {
        // Hide the reward & show its corresponding check mark.
        rewardsParent.GetChild(rewardIndex).gameObject.SetActive(false);
        rewardsCheckMarksParent.GetChild(rewardIndex).gameObject.SetActive(true);

        // Update Progress Bar
        float progressValue = (float)currentRewardIndex / TOTAL_REWARDS;
        progressBarFill.fillAmount = progressValue;

        // If it's the last Reward
        if (rewardIndex == TOTAL_REWARDS - 1)
        {
            watchAdButton.interactable = false;
            remainingAdsBadge.SetActive(false);
            noMoreRewardsPanel.SetActive(true);

            currentRewardIndex = TOTAL_REWARDS;
        }
    }

    void CheckForAvailableRewards()
    {
        currentRewardIndex = PlayerPrefs.GetInt("CurrentRewardIndex", 0);

        // Check if it's the last Reward
        if (currentRewardIndex == TOTAL_REWARDS)
        {
            // Get saved date time
            DateTime rewardsCompletionDateTime = DateTime.Parse(PlayerPrefs.GetString("RewardsCompletionDateTime", DateTime.Now.ToString()));
            DateTime currentDateTime = DateTime.Now;

            // Get total minutes between these two dates
            double elapsedMinutes = (currentDateTime - rewardsCompletionDateTime).TotalMinutes;

            Debug.Log("Time Passed Since Last Reward: " + elapsedMinutes);

            if (elapsedMinutes >= waitTimeToActivateRewards)
            {
                // Activate Rewards again
                PlayerPrefs.SetString("RewardsCompletionDateTime", "");
                PlayerPrefs.SetInt("CurrentRewardIndex", 0);
                currentRewardIndex = 0;
            }
            else
            {
                // Show message to the user to wait more.
                Debug.Log("wait for " + (waitTimeToActivateRewards - elapsedMinutes) + " Minutes");
            }
        }

        // Check if already watched some ads
        if (currentRewardIndex > 0)
        {
            for (int i = 0; i < currentRewardIndex; i++)
            {
                MarkRewardAsChecked(i);
            }
        }
    }

    // Watched Ads & Remaining Rewards Text UI Update
    void UpdateRemainingRewardsTextUI()
    {
        remainingAdsBadgeText.text = (TOTAL_REWARDS - currentRewardIndex).ToString();
    }

    void UpdateWatchedADsTextUI()
    {
        watchedAdsText.text = string.Format("{0}/{1}", currentRewardIndex, TOTAL_REWARDS);
    }

    // Coins Text UI Update
    void UpdateCoinsTextUI()
    {
        coinsText.text = CoinManager.Instance.GetCoins().ToString();
    }

    // Open & Close Reward box
    public void OpenUI()
    {
        rewardBoxUICanvas.SetActive(true);
    }

    public void CloseUI()
    {
        rewardBoxUICanvas.SetActive(false);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Player Prefs deleted ...");
        }
    }
#endif
}
