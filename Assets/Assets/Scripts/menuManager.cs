using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    [SerializeField] private Text TapToPlay; 
    [SerializeField] private GameObject Hand_Ico;
    [SerializeField] private GameObject Tap_line;
    [SerializeField] private GameObject DailyRewardButton;
    [SerializeField] private RectTransform tap_handRectTransform;
    [SerializeField] private GameObject shop;
    

    [SerializeField] private float tapToPlaySpeed = 2f;  
    [SerializeField] private float handIcoSpeed = 1f;    

    private bool gameStarted = false; 

    void Start()
    {
        TapToPlay.transform.DOScale(3f, tapToPlaySpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        Hand_Ico.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-289f, -21f), handIcoSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
    }

    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(tap_handRectTransform, mousePos))
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;

        TapToPlay.transform.DOKill();
        Hand_Ico.GetComponent<RectTransform>().DOKill();

        TapToPlay.gameObject.SetActive(false);
        Hand_Ico.SetActive(false);
        Tap_line.SetActive(false);
        DailyRewardButton.SetActive(false);
        shop.SetActive(false);

        PlayerManager.PlayerManagerInstance.StartGame();

        Debug.Log("Game Started!");
    }
}
