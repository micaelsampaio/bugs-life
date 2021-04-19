using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom_item", menuName = "ANT/Custom Item/New Item")]

public class CustomizeItem : ScriptableObject
{
    public GameObject Prefab;
    private List<CustomizeItemClone> SpawnedPrefabs = new List<CustomizeItemClone>();
    public int Priority = 100;
    public int PreLoadSize = 0;

    public CustomizeItemClone GetClone(GameObject parent)
    {
        var index = SpawnedPrefabs.FindIndex(s => !s.gameObject.activeSelf);

        if (index != -1)
        {
            SpawnedPrefabs[index].gameObject.SetActive(true);
            return SpawnedPrefabs[index];
        }

        var clone = Instantiate(Prefab);
        clone.SetActive(true);

        var scripts = clone.gameObject.GetComponentsInChildren<CustomizeItemPrefab>();
        foreach (var script in scripts)
        {
            script.OnCreate(parent);
        }

        var item = new CustomizeItemClone()
        {
            gameObject = clone,
            parent = this
        };
        return item;
    }

    public void ResetClose(CustomizeItemClone clone)
    {
        clone.gameObject.transform.SetParent(null);
        clone.gameObject.SetActive(false);
    }
}

public class CustomizeItemClone
{
    public GameObject gameObject;
    public CustomizeItem parent;

    public void Reset()
    {
        parent.ResetClose(this);
    }
}