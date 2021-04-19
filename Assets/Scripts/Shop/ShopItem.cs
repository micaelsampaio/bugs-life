
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ANT/Shop/New Shop Item")]
public class ShopItem : ScriptableObject
{
    public string Name;
    public string Description;
    public int Level = 0;
    public bool StopOnMaxLevel = true;
    public List<ShopItemUpgrade> Upgrades = new List<ShopItemUpgrade>();
    public ShopItemUI UI;

    public void Init()
    {
        Level = 0;
    }
}
[Serializable]
public class ShopItemAmount
{
    public ShopItemType Type;
    public int Amount;
}

[Serializable]
public class ShopItemUpgrade
{
    public int Price = 10;
    public int Time = -1;
    public string TimeDescription;
    public ShopItemAmount[] Items;
}

public enum ShopItemType { POPULATION, WORKERS, BADGES, MAX_FOOD }
