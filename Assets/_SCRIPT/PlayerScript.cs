using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent player;
    [SerializeField] private Transform target;
    private void Update()
    {
        player.SetDestination(target.position);
        if (Input.GetKeyDown("a"))
        {
            MoveIt();
        }
    }

    private void MoveIt()
    {
        
    }
}
