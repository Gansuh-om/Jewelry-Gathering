using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent player;
    private Transform _target;
    private void Update()
    {
        player.SetDestination(_target.position);
    }

    public void SetTarget(Transform value)
    {
        _target = value;
    }
}
