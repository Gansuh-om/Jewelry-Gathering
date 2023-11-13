using System;
using System.Collections;
using System.Collections.Generic;
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
    private void Awake()
    {
        Instance = this;
        ShowUpgrade();
    }

    public void Upgrade(int index)
    {
        upgrade[index].index++;
        if (upgrade[index].index == 5&&upgrade[index].lvl<5)
        {
            upgrade[index].value += upgrade[index].increaseLvlValue;
            upgrade[index].lvl++;
            upgrade[index].index = 0;
            upgrade[index].sprite.fillAmount = 1;
            // if (index==0)
            // {
                
            // }
        }
        else
        {
            var currentUpgrade = upgrade[index];
            currentUpgrade.value += currentUpgrade.increaseValue;

            var remappedValue = PlayerMovement.Remap(currentUpgrade.index, 0, 5, 0, 1);
            currentUpgrade.sprite.fillAmount = remappedValue;
            Debug.Log(remappedValue);

            // currentUpgrade.sprite.DOFill(remappedValue, 0.5f);
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
        // attractor.SetPower(upgrade[index].lvl);
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
