using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class shopp : MonoBehaviour
{
    [System.Serializable]  class ShopItem
    
    {
        public Sprite Image;
        public int Price;
        public bool IsPurchased = false;
    }
   
    [SerializeField] List<ShopItem> ShopItemsList;
    GameObject ItemTemplate;
    GameObject g;
    [SerializeField] Transform ShopScrollView;

    void Start()
    {
        ItemTemplate = ShopScrollView.GetChild(0).gameObject;

        int Len = ShopItemsList.Count;
        for (int i = 0; i < Len; i++)
        {
            g = Instantiate(ItemTemplate, ShopScrollView);
            g.transform.GetChild(0).GetComponent<Image> ().sprite = ShopItemsList[i].Image;
            g.transform.GetChild(1).GetChild(0).GetComponent<Text> ().text = ShopItemsList[i].Price.ToString();
            g.transform.GetChild(2).GetComponent<Button> ().interactable = !ShopItemsList[i].IsPurchased;
        }

        Destroy(ItemTemplate);
    }
}
