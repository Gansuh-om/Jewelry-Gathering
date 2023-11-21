using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelStatus : MonoBehaviour
{
    public int id;
    private Material _initialColor;
    public MeshRenderer meshRenderer;
    public Material material;

    private void Awake()
    {
        _initialColor = meshRenderer.material;
    }

    public int GetId()
    {
        return id;
    }

    public void SetColor(bool value=false)
    {
        if (value)
        {
            meshRenderer.material = _initialColor;
        }
        else
        {
            meshRenderer.material = material;
        }
    }
}
