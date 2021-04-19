using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeItemParentMaterial : CustomizeItemPrefab
{
    public Renderer Renderer;

    public override void OnCreate(GameObject parent)
    {
        base.OnCreate(parent);
        var ai = parent.GetComponent<AI>();
        Renderer.material.SetTexture("_MainTex", ai.CurrentTexture);

        // GET PARENT TEXTURE
        // SET TEXTURE

    }
}
