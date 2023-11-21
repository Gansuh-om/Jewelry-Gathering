using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelStatus : MonoBehaviour
{
    public int id;
    public Material initialMat;
    public MeshRenderer meshRenderer;
    public Material materialz;
    public int GetId()
    {
        return id;
    }

    public void SetColor(bool value=false)
    {
        Debug.Log(value);
        if (value)
        {
            meshRenderer.material = initialMat;
        }
        else
        {
            meshRenderer.material = materialz;
        }
    }
}
