using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    private bool _hold;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _hold = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _hold = false;
        }
        
    }
}
