using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private Transform follow;
    public Transform targetTransform;
    public float rotationSpeed = 5.0f;
    private bool _once;

    // private void Awake()
    // {
    //     transform.position = follow.position;
    //     transform.rotation = follow.rotation;
    // }

    private void OnEnable()
    {
        transform.position = follow.position;
        transform.rotation = follow.rotation;
        _once = true;
        // rotationSpeed = 100;
    }

    private void Update()
    {
        if (!_once)
        {
            return;
        }
        // transform.rotation = follow.rotation;
        transform.position = follow.position;
        if (targetTransform != null)
        {
            Vector3 directionToTarget = targetTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            
            // Interpolate the rotation to smoothly "look at" the target
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
