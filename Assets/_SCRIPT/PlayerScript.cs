using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent player;
    [SerializeField] private List<Transform> parts;
    private Transform _target;
    private void Update()
    {
        player.SetDestination(_target.position);
        
    }

    public void SetTarget(Transform value)
    {
        _target = value;
    }

    public void ChangeSpeed(float value)
    {
        player.speed = value;
        Debug.Log($"happen? {value}");
    }
    public void LevelUp()
    {
        foreach (var value in parts)
        {
            DOTween.Rewind(player);
            value.DOScale(1, 0.25f).SetEase(Ease.OutElastic).From(0.75f);
        }
    }
}
