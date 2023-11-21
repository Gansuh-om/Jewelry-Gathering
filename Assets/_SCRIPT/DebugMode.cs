using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    [SerializeField] private Slider power;
    [SerializeField] private Slider speed;
    [SerializeField] private Slider range;
    [SerializeField] private Toggle wall;
    [SerializeField] private TMP_InputField capacity;
    [SerializeField] private TMP_InputField jewelCount;
    
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI rangeText;

    [SerializeField] private CarMain car;
    [SerializeField] private GameObject cliff;
    [SerializeField] private GameObject playerCam;
    
    [SerializeField] private GameObject one;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject three;

    private bool _on;
    public void turnOnOff()
    {
        _on = !_on;
        playerCam.SetActive(_on);
    }

    public void PowerLevel()
    {
        Debug.Log($"PowerLevel-debug {power.value}");
        powerText.text = power.value.ToString();
        car.Power((int)power.value);
    }
    public void Capacity()
    {
        Debug.Log($"Capacity-debug {capacity.text}");
        if (int.TryParse(capacity.text, out int result))
        {
            car.ChangeCount(result);
        }
    }
    public void Speed()
    {
        Debug.Log($"Speed-debug {speed.value}");
        speedText.text = speed.value.ToString();
        car.Speed((int)speed.value);
    }
    public void Range()
    {
        Debug.Log($"Range-debug {range.value}");
        rangeText.text = range.value.ToString();
        car.ChangeRadius(range.value);
    }

    public void JewelCount()
    {
        Debug.Log($"JewelCount-debug {jewelCount.text}");
        if (int.TryParse(jewelCount.text, out int result))
        {
            switch (result)
            {
                case 1:
                    one.SetActive(true);
                    two.SetActive(false);
                    three.SetActive(false);
                    break;
                case 2:
                    one.SetActive(false);
                    two.SetActive(true);
                    three.SetActive(false);
                    break;
                default:
                    one.SetActive(false);
                    two.SetActive(false);
                    three.SetActive(true);
                    break;
            }
        }
    }
    public void Wall()
    {
        Debug.Log($"Wall-debug {wall.isOn}");
        cliff.SetActive(wall.isOn);
    }
}
