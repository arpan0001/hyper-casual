using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Profile : MonoBehaviour
{
    #region Singleton:Profile

    public static Profile Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    public class Avatar
    {
        public Sprite Image;
        public GameObject Prefab;
    }

    public List<Avatar> AvatarsList;

    [SerializeField] GameObject AvatarUITemplate;
    [SerializeField] Transform AvatarsScrollView;

    GameObject g;
    int newSelectedIndex, previousSelectedIndex;

    [SerializeField] Color ActiveAvatarColor;
    [SerializeField] Color DefaultAvatarColor;

    [SerializeField] Image CurrentAvatar;

    void Start()
    {
        GetAvailableAvatars();
        newSelectedIndex = previousSelectedIndex = 0;
    }

    void GetAvailableAvatars()
    {
        for (int i = 0; i < Shop.Instance.ShopItemsList.Count; i++)
        {
            if (Shop.Instance.ShopItemsList[i].IsPurchased)
            {
                AddAvatar(Shop.Instance.ShopItemsList[i].Image, Shop.Instance.ShopItemsList[i].Prefab);
            }
        }

        SelectAvatar(newSelectedIndex);
    }

    public void AddAvatar(Sprite img, GameObject prefab)
    {
        if (AvatarsList == null)
            AvatarsList = new List<Avatar>();

        Avatar av = new Avatar() { Image = img, Prefab = prefab };
        AvatarsList.Add(av);

        g = Instantiate(AvatarUITemplate, AvatarsScrollView);
        g.transform.GetChild(0).GetComponent<Image>().sprite = av.Image;

        g.transform.GetComponent<Button>().AddEventListener(AvatarsList.Count - 1, OnAvatarClick);
    }

    void OnAvatarClick(int AvatarIndex)
    {
        SelectAvatar(AvatarIndex);
    }

    void SelectAvatar(int AvatarIndex)
    {
        previousSelectedIndex = newSelectedIndex;
        newSelectedIndex = AvatarIndex;
        AvatarsScrollView.GetChild(previousSelectedIndex).GetComponent<Image>().color = DefaultAvatarColor;
        AvatarsScrollView.GetChild(newSelectedIndex).GetComponent<Image>().color = ActiveAvatarColor;

        CurrentAvatar.sprite = AvatarsList[newSelectedIndex].Image;

        // Save the selected avatar index to PlayerPrefs
        PlayerPrefs.SetInt("SelectedAvatarIndex", newSelectedIndex);
        PlayerPrefs.Save();
    }

    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
