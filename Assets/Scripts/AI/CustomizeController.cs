using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * popcorn
sugar
dead mosquito
work -> ShopItem -> Wings
flower
pan
gum
rock

___

hat 
scarf
glasses
hair ???
wings if male no family
gravata
pisco
minocolo
chapeu de velho
beard



->eyes 
->mouths
___ 

badge
- chest
- head



.> head
half body



*/
[CreateAssetMenu(fileName = "Custom_item", menuName = "ANT/Custom Item/New Controller")]
public class CustomizeController : ScriptableObject
{
    public List<CustomizeItem> AntennaeItems;
    public List<CustomizeItem> HairItems;
    public List<CustomizeItem> EyesItems;
    public List<CustomizeItem> Badge;

    public List<Texture> Textures;


    public AI CustomizeAI(AI ai)
    {
        foreach (var item in ai.CustomObjects)
        {
            item.Reset();
        }
        ai.CustomObjects.Clear();
        // Clear old Elements
        CreateAiTextures(ai);
        CreateAiAntennae(ai);
        CreateAiEyes(ai);
        CreateAiHair(ai);
        return ai;
    }

    public void CreateAiAntennae(AI ai)
    {
        var clone = GetRandomAntennae(ai.gameObject);
        SetSocketParent(ai.AntennaeSocket, clone);

        ai.CustomObjects.Add(clone);
    }

    public void CreateAiEyes(AI ai)
    {
        var clone = GetRandomEyes(ai.gameObject);
        SetSocketParent(ai.AntennaeSocket, clone);

        ai.CustomObjects.Add(clone);
    }

    public void CreateAiHair(AI ai)
    {
        if (Random.value > 0.5)
        {
            var clone = GetRandomHair(ai.gameObject);
            SetSocketParent(ai.AntennaeSocket, clone);

            ai.CustomObjects.Add(clone);
        }
    }

    private void CreateAiTextures(AI ai)
    {
        // ai.Renderer.material.SetColor("_Color", new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

        ai.CurrentTexture = Textures[Random.Range(0, Textures.Count)];
        ai.Renderer.material.SetTexture("_MainTex", ai.CurrentTexture);
    }

    public void SetSocketParent(GameObject socket, CustomizeItemClone clone)
    {
        clone.gameObject.transform.SetParent(socket.transform);
        clone.gameObject.transform.localPosition = socket.transform.localPosition;
        clone.gameObject.transform.localRotation = socket.transform.localRotation;
    }

    public CustomizeItemClone GetRandomAntennae(GameObject parent)
    {
        return AntennaeItems[Random.Range(0, AntennaeItems.Count)].GetClone(parent);
    }

    public CustomizeItemClone GetRandomHair(GameObject parent)
    {
        return HairItems[Random.Range(0, HairItems.Count)].GetClone(parent);
    }

    public CustomizeItemClone GetRandomEyes(GameObject parent)
    {
        return EyesItems[Random.Range(0, EyesItems.Count)].GetClone(parent);
    }

    public CustomizeItemClone GetRandomBadge(GameObject parent)
    {
        return null;
    }
}
