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
    private List<Transform> _collecting = new List<Transform>();
    [SerializeField] private Transform offloadSpot;
    public int maxCount=5;
    public int power;

    private void Awake()
    {
        _collidersBuffer = new Collider[20];
        _rb = GetComponent<Rigidbody>();
    }

    public void SetPower(int value)
    {
        power = value;
    }

    private void FixedUpdate()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, attractionRadius, _collidersBuffer);
        for (int i = 0; i < numColliders; i++)
        {
            Collider col = _collidersBuffer[i];
            if (col.CompareTag("Attractable"))
            {
                if (_counter < maxCount&&power>=col.GetComponent<JewelStatus>().GetId())
                {
                    _counter++;
                    col.tag = "Collecting";
                    _collecting.Add(col.transform);
                }
            }
            // else if (col.CompareTag("Collecting"))
            // {
            //     Vector3 attractionDirection = transform.position - col.transform.position;
            //     float distance = attractionDirection.magnitude;
            //
            //     if (distance > 0.5f) // Avoid division by zero
            //     {
            //         attractionDirection /= distance;
            //         float force = attractionForce / distance;
            //         col.transform.position += attractionDirection * force * Time.deltaTime;
            //     }
            //     else
            //     {
            //         col.tag = "Collected";
            //         _collected.Add(col.transform);
            //     }
            // }
        }

        if (_collecting.Count > 0)
        {
            for (int i = 0; i < _collecting.Count; i++)
            {
                Vector3 attractionDirection = transform.position - _collecting[i].position;
                float distance = attractionDirection.magnitude;

                if (distance > 0.5f) // Avoid division by zero
                {
                    attractionDirection /= distance;
                    float force = attractionForce / distance;
                    _collecting[i].position += attractionDirection * force * Time.deltaTime;
                }
                else
                {
                    _collecting[i].transform.parent = transform;
                    _collecting[i].tag = "Collected";
                    _collected.Add(_collecting[i]);
                    _collecting.Remove(_collecting[i]);
                    break;
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
            _collected[i].DOMove(offloadSpot.position, 0.25f).SetDelay(i * 0.025f);
            if (i == _collected.Count - 1)
            {
                RemoveListItems();
            }
        }
    }

    private void RemoveListItems()
    {
        foreach (var col in _collected)
        {
            col.parent = offloadSpot;
        }
        for (int i = 0; i < _collected.Count; i++)
        {
            _collected.RemoveAt(0);
        }
    }

    public void ChangeCount(int value)
    {
        maxCount = value;
    }

    public void ChangeRadius(float value)
    {
        attractionRadius = value;
    }
}