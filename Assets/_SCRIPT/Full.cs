using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Full : MonoBehaviour
{
    public static Full Instance;
    [SerializeField] private Image image;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        image.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
    }
}
