using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectable_X", menuName = "ANT/Collectable/New Collectable")]
public class Collectable : ScriptableObject
{
    [Header("Info")]
    public Sprite Icon;
    public string Description;
    public int Priority;

    [Header("Stats")]
    public CollectableStats[] Stats; // 0- bad 1 -medium 2 -good  less bad more good
    public CollectableStats CurrentStats;
}

[Serializable]
public class CollectableStats
{
    public int Population;
    public int Food;
    public int Price; // you pay
    public int Money; // you win
    public int Workers;
    public CollectableType Type = CollectableType.MEIDUM;
}

public enum CollectableType { GOOD, MEIDUM, BAD };
