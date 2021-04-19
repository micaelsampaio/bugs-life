using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeItemRandomTexture : CustomizeItemPrefab
{
    public Texture[] Texture;
    public Renderer Renderer;

    public override void OnCreate(GameObject parent)
    {
        base.OnCreate(parent);

        Renderer.material.SetTexture("_MainTex", Texture[Random.Range(0, Texture.Length)]);

        // GET PARENT TEXTURE
        // SET TEXTURE

    }
}
