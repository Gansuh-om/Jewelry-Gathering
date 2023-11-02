using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
    public float attractionForce = 10.0f;
    public float attractionRadius = 5.0f;
    private Collider[] _collidersBuffer;
    private Rigidbody _rb;
    private int _counter = 0;
    private bool _endCheck;
    private List<Transform> _collected = new List<Transform>();
    [SerializeField] private Transform offloadSpot;

    void Awake()
    {
        _collidersBuffer = new Collider[20]; // Set the buffer size as needed
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, attractionRadius, _collidersBuffer);
        for (int i = 0; i < numColliders; i++)
        {
            Collider col = _collidersBuffer[i];
            if (col.CompareTag("Attractable")) // Avoid attracting itself
            {
                if (_counter < 5)
                {
                    _counter++;
                    col.transform.parent = transform;
                    col.tag = "Collecting";
                }
            }else if (col.CompareTag("Collecting"))
            {
                Vector3 attractionDirection = transform.position - col.transform.position;
                float distance = attractionDirection.magnitude;

                if (distance > 0.5f) // Avoid division by zero
                {
                    attractionDirection /= distance;
                    float force = attractionForce / distance;
                    col.transform.position += attractionDirection * force * Time.deltaTime;
                }
                else
                {
                    col.tag = "Collected";
                    _collected.Add(col.transform);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OffloadSpot"))
        {
            if(_collected.Count<=0)
                return;
            ReleaseCollected();
        }
    }

    public void ReleaseCollected()
    {
        _counter = 0;
        for (int i = 0; i < _collected.Count; i++)
        {
            _collected[i].parent = offloadSpot;
            _collected[i].DOMove(offloadSpot.position, 0.25f).SetDelay(i * 0.1f);
            if (i == _collected.Count - 1)
            {
                RemoveListItems();
            }
        }
    }

    private void RemoveListItems()
    {
        for (int i = 0; i < _collected.Count; i++)
        {
            _collected.RemoveAt(0);
        }
    }
}