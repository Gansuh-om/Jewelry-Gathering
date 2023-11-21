using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Upgrade
{
    public int index;
    public int lvl;
    public float value;
    public float increaseValue;
    public float increaseLvlValue;
    public Image sprite;
    public TextMeshProUGUI text;
    public int initialCost;
    public int increaseCost;
}

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance;
    public Upgrade[] upgrade = new Upgrade[3];
    // [SerializeField] private Attractor attractor;
    
    [SerializeField] private CarMain carMain;
    
    [SerializeField] private TextMeshProUGUI radiusUpgrade;
    [SerializeField] private TextMeshProUGUI capacityUpgrade;
    
    [SerializeField] private List<Transform> playerParts;
    
    [SerializeField] private TextMeshProUGUI moneyUi;
    
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private Transform debugButton;
    [SerializeField] private CinemachineVirtualCamera virtCam;
    
    public int money;
    public int totalCount;

    public int powerLevel;
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Instance = this;
        totalCount = totalObjects.Count;
        for (int i = 0; i < upgrade.Length; i++)
        {
            upgrade[i].text.text = upgrade[i].increaseCost.ToString();
        }
        Initializer();
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        moneyUi.text = money.ToString();
    }

    private void Start()
    {
        GrayScale();
    }

    public void GrayScale()
    {
        foreach (var value in totalObjects)
        {
            if (powerLevel < value.GetComponent<JewelStatus>().GetId())
            {
                // Debug.Log($"{powerLevel} -{value.GetComponent<JewelStatus>().GetId()}"); 
                value.GetComponent<JewelStatus>().SetColor();   
            }
            else
            {
                value.GetComponent<JewelStatus>().SetColor(true);   
                Debug.Log($"{powerLevel} -{value.GetComponent<JewelStatus>().GetId()}"); 
            }
        }
    }
    public async void IncreaseMoney(int value)
    {
        money += value;
        moneyUi.color = Color.green;
        for (int i = value; i >= 0; i--)
        {
            // money++;
            moneyUi.text = (money - i).ToString();
            await Task.Delay(50);
        }
        moneyUi.color = Color.white;
        PlayerPrefs.SetInt("Money",money);
    }

    public void DecreaseCount(int value)
    {
        totalCount -= value;
        if (totalCount <= 10)
        {
            nextUI.SetActive(true);
        }
    }
    public async void DecreaseMoney(int value)
    {
        money -= value;
        moneyUi.color = Color.red;
        for (int i = value; i > 0; i--)
        {
            // money--;
            moneyUi.text = (money + i).ToString();
            await Task.Delay(1);
        }
        moneyUi.color = Color.white;
    }

    private int _counter;
    public void CreativeUpgrade()
    {
        _counter++;
        DOTween.Rewind(debugButton);
        debugButton.DOScale(1 + (0.1f * _counter), 0.25f).SetEase(Ease.OutElastic);
        switch (_counter)
        {
            case 1:
                debugText.text = "LEVEL 5";
                break;
            case 2:
                debugText.text = "LEVEL 10";
                break;
            case 3:
                debugText.text = "LEVEL 50";
                break;
            case 4:
                debugText.text = "LEVEL 100";
                break;
            default:
                _counter = 4;
                break;
        }

        // CinemachineFramingTransposer transposer = virtCam.GetComponent<CinemachineFramingTransposer>();
        // transposer.m_CameraDistance += 5;
        CinemachineComponentBase componentBase = virtCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            (componentBase as CinemachineFramingTransposer).m_CameraDistance +=2; // your value
        }
        // virtCam.
        carMain.SetInt(_counter); 
        carMain.ChangeCount(1000);
    }
    public void Upgrade(int index)
    {

        if (money < upgrade[index].initialCost)
        {
            return;
        }
        Debug.Log(upgrade[index].lvl);
        if (upgrade[index].lvl >= 5)
        {
            upgrade[index].text.text = "MAXED";
            return;
        }

        if (index == 2)
        {
            if (upgrade[index].lvl >= 4)
            {
                upgrade[index].text.text = "MAXED";
                return;
            }
        }
        DecreaseMoney(upgrade[index].initialCost);
        upgrade[index].initialCost += upgrade[index].increaseCost;
        upgrade[index].text.text = upgrade[index].initialCost.ToString();
        upgrade[index].index++;
        if (upgrade[index].index == 5)
        {
            upgrade[index].value += upgrade[index].increaseLvlValue;
            upgrade[index].lvl++;
            upgrade[index].index = 0;
            upgrade[index].sprite.DOFillAmount(1, 0.25f).SetEase(Ease.InSine);
            if (upgrade[index].lvl == 5)
            {
                upgrade[index].text.text = "MAXED";
            }
            if (index == 2)
            {
                if (upgrade[index].lvl >= 4)
                {
                    upgrade[index].text.text = "MAXED";
                }
            }
        }
        else
        {
            // var currentUpgrade = upgrade[index];
            upgrade[index].value += upgrade[index].increaseValue;
            // currentUpgrade.value += currentUpgrade.increaseValue;
            var remappedValue=0f;
            if (index == 2)
            {
                remappedValue = PlayerMovement.Remap(upgrade[index].index, 0, 4, 0, 1);
            }
            else
            {
                remappedValue = PlayerMovement.Remap(upgrade[index].index, 0, 5, 0, 1);
            }
            upgrade[index].sprite.DOFillAmount(remappedValue, 0.25f).SetEase(Ease.InSine);
        }
        switch (index)
        {
            case 0:
                carMain.ChangeRadius(upgrade[index].value);
                PlayerPrefs.SetInt($"CarRadiusLevel",upgrade[index].lvl);
                PlayerPrefs.SetInt($"CarRadiusIndex",upgrade[index].index);
                PlayerPrefs.SetFloat($"CarRadiusValue",upgrade[index].value);
                PlayerPrefs.SetInt($"CarRadiusCost",upgrade[index].initialCost);
                break;
            case 1:
                carMain.ChangeCount((int)upgrade[index].value);
                PlayerPrefs.SetInt($"CarCountLevel",upgrade[index].lvl);
                PlayerPrefs.SetInt($"CarCountIndex",upgrade[index].index);
                PlayerPrefs.SetFloat($"CarCountValue",upgrade[index].value);
                PlayerPrefs.SetInt($"CarCountCost",upgrade[index].initialCost);
                break;
            default:
                upgrade[index].lvl++;
                PlayerPrefs.SetInt($"CarPowerLevel",upgrade[index].lvl);
                PlayerPrefs.SetInt($"CarPowerIndex",upgrade[index].index);
                PlayerPrefs.SetFloat($"CarPowerValue",upgrade[index].value);
                PlayerPrefs.SetInt($"CarPowerCost",upgrade[index].initialCost);
                PlayerLevelUp();
                carMain.SetInt(upgrade[index].lvl);
                powerLevel = upgrade[index].lvl;
                GrayScale();
                break;
        }
        if (upgrade[index].lvl == 5)
        {
            upgrade[index].text.text = "MAXED";
        }
        if (index == 2)
        {
            if (upgrade[index].lvl >= 4)
            {
                upgrade[index].text.text = "MAXED";
            }
        }
    }

    private void Initializer()
    {
        if (PlayerPrefs.HasKey("CarRadiusLevel"))
        {
            upgrade[0].lvl = PlayerPrefs.GetInt("CarRadiusLevel");
        }
        if (PlayerPrefs.HasKey("CarRadiusIndex"))
        {
            upgrade[0].index = PlayerPrefs.GetInt("CarRadiusIndex");
        }
        if (PlayerPrefs.HasKey("CarRadiusValue"))
        {
            upgrade[0].value = PlayerPrefs.GetFloat("CarRadiusValue");
        }
        if (PlayerPrefs.HasKey("CarRadiusCost"))
        {
            upgrade[0].initialCost = PlayerPrefs.GetInt("CarRadiusCost");
        }
        //////
        
        if (PlayerPrefs.HasKey("CarCountLevel"))
        {
            upgrade[1].lvl = PlayerPrefs.GetInt("CarCountLevel");
        }
        if (PlayerPrefs.HasKey("CarCountIndex"))
        {
            upgrade[1].index = PlayerPrefs.GetInt("CarCountIndex");
        }
        if (PlayerPrefs.HasKey("CarCountValue"))
        {
            upgrade[1].value = PlayerPrefs.GetFloat("CarCountValue");
        }
        if (PlayerPrefs.HasKey("CarCountCost"))
        {
            upgrade[1].initialCost = PlayerPrefs.GetInt("CarCountCost");
        }
        
        ////////
        
        if (PlayerPrefs.HasKey("CarPowerLevel"))
        {
            upgrade[2].lvl = PlayerPrefs.GetInt("CarPowerLevel");
        }
        if (PlayerPrefs.HasKey("CarPowerIndex"))
        {
            upgrade[2].index = PlayerPrefs.GetInt("CarPowerIndex");
        }
        if (PlayerPrefs.HasKey("CarPowerValue"))
        {
            upgrade[2].value = PlayerPrefs.GetFloat("CarPowerValue");
        }
        if (PlayerPrefs.HasKey("CarPowerCost"))
        {
            upgrade[2].initialCost = PlayerPrefs.GetInt("CarPowerCost");
        }

        for (int i = 0; i < 3; i++)
        {
            upgrade[i].text.text = upgrade[i].initialCost.ToString();
            var remappedValue=0f;
            if (i == 2)
            {
                remappedValue = PlayerMovement.Remap(upgrade[i].index, 0, 4, 0, 1);
            }
            else
            {
                remappedValue = PlayerMovement.Remap(upgrade[i].index, 0, 5, 0, 1);
            }
            upgrade[i].sprite.DOFillAmount(remappedValue, 0.25f).SetEase(Ease.InSine);
            
            if (upgrade[i].lvl == 5)
            {
                upgrade[i].text.text = "MAXED";
            }
            if (i == 2)
            {
                if (upgrade[i].lvl >= 4)
                {
                    upgrade[i].text.text = "MAXED";
                }
            }
        }
        carMain.ChangeRadius(upgrade[0].value);
        carMain.ChangeCount((int)upgrade[1].value);
        carMain.SetInt(upgrade[2].lvl); 
        powerLevel = upgrade[2].lvl;
        GrayScale();
    }

    private void PlayerLevelUp()
    {
        foreach (var player in playerParts)
        {
            DOTween.Rewind(player);
            player.DOScale(1, 0.25f).SetEase(Ease.OutElastic).From(0.5f);
        }
    }

    public void Next()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
