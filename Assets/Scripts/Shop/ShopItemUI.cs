using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TMP_Text Description;
    public TMP_Text Info;
    public Button ShopButton;
    private ShopItem ShopItem;

    public void SetItem(ShopItem shopItem, Action<ShopItem> cb)
    {
        ShopItem = shopItem;
        ShopButton.onClick.RemoveAllListeners();
        ShopButton.onClick.AddListener(() =>
        {
            int level = shopItem.Level;
            cb?.Invoke(shopItem);

            if (level != shopItem.Level && shopItem.Level >= shopItem.Upgrades.Count && shopItem.StopOnMaxLevel)
            {
                ShopButton.interactable = false;
            }

            if (level != shopItem.Level && shopItem.Level >= shopItem.Upgrades.Count)
            {
                shopItem.Level = shopItem.Upgrades.Count - 1;
            }

            UpdateView();
        });

        UpdateView();
    }

    public void UpdateView()
    {
        Description.text = ShopItem.Description;

        string infoText = "level: " + (ShopItem.Level + 1).ToString() + ", ";
        var info = ShopItem.Upgrades[ShopItem.Level];

        foreach (var item in info.Items)
        {
            infoText += item.Type + " +" + item.Amount.ToString();
        }

        infoText += " // " + info.Price + "$";
        Info.text = infoText;
    }
}
