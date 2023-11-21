using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

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
    
    public int money;
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Instance = this;
        ShowUpgrade();
        moneyUi.text = money.ToString();
        for (int i = 0; i < upgrade.Length; i++)
        {
            upgrade[i].text.text = upgrade[i].increaseCost.ToString();
        }
    }

    public async void IncreaseMoney(int value)
    {
        money += value;
        moneyUi.color = Color.green;
        for (int i = value; i > 0; i--)
        {
            // money++;
            moneyUi.text = (money - i).ToString();
            await Task.Delay(50);
        }
        moneyUi.color = Color.white;
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
                break;
            case 1:
                carMain.ChangeCount((int)upgrade[index].value);
                break;
            default:
                upgrade[index].lvl++;
                PlayerLevelUp();
                carMain.SetInt(upgrade[index].lvl); 
                break;
        }
        ShowUpgrade();
    }

    private void ShowUpgrade()
    {
        radiusUpgrade.text = $"Level - {upgrade[0].lvl}\nIndex - {upgrade[0].index}\nValue - {upgrade[0].value}";
        capacityUpgrade.text = $"Level - {upgrade[1].lvl}\nIndex - {upgrade[1].index}\nValue - {upgrade[1].value}";
    }

    private void PlayerLevelUp()
    {
        foreach (var player in playerParts)
        {
            DOTween.Rewind(player);
            player.DOScale(1, 0.25f).SetEase(Ease.OutElastic).From(0.5f);
        }
    }
}
