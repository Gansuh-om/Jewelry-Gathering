using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    public static DebugMode Instance;
    
    [SerializeField] private Slider power;
    [SerializeField] private Slider range;
    [SerializeField] private Toggle wall;
    [SerializeField] private Toggle upgradeCamera;
    [SerializeField] private TMP_InputField capacity;
    [SerializeField] private TMP_InputField jewelCount;
    [SerializeField] private TMP_InputField speed;
    
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI rangeText;

    [SerializeField] private CarMain car;
    [SerializeField] private GameObject cliff;
    [SerializeField] private GameObject playerCam;
    
    [SerializeField] private GameObject one;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject three;
    private int _power;
    private float _range;
    private int _wall;
    private int _upgrade;
    private int _capacity;
    private int _jewelCount;
    private int _speed;

    private bool _on;
    private bool _upgradeCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("_power"))
        {
            _power = PlayerPrefs.GetInt("_power");
            power.value = _power;
            PowerLevel();
        }
        if (PlayerPrefs.HasKey("_range"))
        {
            _range = PlayerPrefs.GetFloat("_range");
            range.value = _power;
            Range();
        }
        if (PlayerPrefs.HasKey("_wall"))
        {
            _wall = PlayerPrefs.GetInt("_wall");
            if (_wall != 0)
            {
                wall.isOn = true;
            }
            else
            {
                wall.isOn = false;
            }
            Wall();
        }
        if (PlayerPrefs.HasKey("_upgrade"))
        {
            _upgrade = PlayerPrefs.GetInt("_upgrade");
            if (_upgrade != 0)
            {
                upgradeCamera.isOn = true;
            }
            else
            {
                upgradeCamera.isOn = false;
            }
            Wall();
        }
        if (PlayerPrefs.HasKey("_capacity"))
        {
            _capacity = PlayerPrefs.GetInt("_capacity");
            capacity.text = _capacity.ToString();
            Capacity();
        }
        if (PlayerPrefs.HasKey("_jewelCount"))
        {
            _jewelCount = PlayerPrefs.GetInt("_jewelCount");
            jewelCount.text = _jewelCount.ToString();
            JewelCount();
        }
        if (PlayerPrefs.HasKey("_speed"))
        {
            _speed = PlayerPrefs.GetInt("_speed");
            speed.text = _speed.ToString();
            Speed();
        }
    }

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
        PlayerPrefs.SetInt("_power",(int)power.value);
    }
    public void Capacity()
    {
        Debug.Log($"Capacity-debug {capacity.text}");
        if (int.TryParse(capacity.text, out int result))
        {
            car.ChangeCount(result);
            PlayerPrefs.SetInt("_capacity",result);
        }
        
    }
    public void Speed()
    {
        // Debug.Log($"Speed-debug {speed.value}");
        // speedText.text = speed.value.ToString();
        // car.Speed((int)speed.value);
        Debug.Log($"Capacity-debug {speed.text}");
        if (int.TryParse(speed.text, out int result))
        {
            // car.ChangeCount(result);
            car.Speed(result);
            PlayerPrefs.SetInt("_speed",result);
        }
    }
    public void Range()
    {
        Debug.Log($"Range-debug {range.value}");
        rangeText.text = range.value.ToString();
        car.ChangeRadius(range.value);
        PlayerPrefs.SetInt("_range",(int)range.value);
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
            PlayerPrefs.SetInt("_jewelCount",result);
        }
    }
    public void Wall()
    {
        Debug.Log($"Wall-debug {wall.isOn}");
        cliff.SetActive(wall.isOn);
        if (wall.isOn)
        {
            PlayerPrefs.SetInt("_wall",1);
        }
        else
        {
            PlayerPrefs.SetInt("_wall",0);
        }
    }
    public void UpgradeCamera()
    {
        Debug.Log($"UpgradeCamera-debug {upgradeCamera.isOn}");
        // _upgradeCam = upgradeCamera.isOn;
        if (upgradeCamera.isOn)
        {
            PlayerPrefs.SetInt("_upgrade",1);
        }
        else
        {
            PlayerPrefs.SetInt("_upgrade",0);
        }
    }

    public bool GetBool()
    {
        return upgradeCamera.isOn;
    }
}
