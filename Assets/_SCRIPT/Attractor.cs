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
    private Transform _offloadSpot;
    private GameObject _upgradeUI;
    public int maxCount=5;
    public int power;
    public float pullDistance=0.5f;
    private float _initialDistance;
    [SerializeField] private GameObject arrow;

    private void Awake()
    {
        _collidersBuffer = new Collider[20];
        _rb = GetComponent<Rigidbody>();
        _initialDistance = pullDistance;
    }

    public void SetPower(int value)
    {
        power = value;
    }

    private void FixedUpdate()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, attractionRadius, _collidersBuffer);
        if (maxCount == _counter)
        {
            Full.Instance.Show();
            arrow.SetActive(true);
        }
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

                if (distance > pullDistance) 
                {
                    attractionDirection /= distance;
                    float force = attractionForce / distance;
                    _collecting[i].position += attractionDirection * force * Time.deltaTime;
                }
                else
                {
                    pullDistance += 0.01f;
                    _collecting[i].transform.parent = transform;
                    _collecting[i].tag = "Collected";
                    _collected.Add(_collecting[i]);
                    _collecting[i].GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
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

        if (other.CompareTag("Upgrade"))
        {
            _upgradeUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Upgrade"))
        {
            _upgradeUI.SetActive(false);
        }
    }
    private int _tempInd;
    private int _counterInside;
    public void ReleaseCollected()
    {
        Full.Instance.Hide();
        arrow.SetActive(false);
        _counter = 0;
        _counterInside = 0;
        pullDistance = _initialDistance;
        // for (int i = 0; i < _collected.Count; i++)
        // {
        //     _collected[i].DOMove(_offloadSpot.position, 0.25f).SetDelay(i * 0.025f).OnComplete(()=>{
        //         _collected[_tempInd].gameObject.SetActive(false);
        //         _tempInd++;
        //         _counterInside++;
        //         if (_counterInside == _collected.Count - 1)
        //         {
                    
        //             RemoveListItems();
        //             _tempInd = 0;
        //         }
        //     });
        // }
        int collectedCount = _collected.Count;
        for (int i = 0; i < collectedCount; i++)
        {
            Transform currentTransform = _collected[i];
            
            currentTransform.DOMove(_offloadSpot.position, 0.25f)
                .SetDelay(i * 0.025f)
                .OnComplete(() =>
                {
                    currentTransform.gameObject.SetActive(false);
                    _counterInside++;

                    if (_counterInside == collectedCount)
                    {
                        RemoveListItems();
                    }
                });
        }
    }

    private void RemoveListItems()
    {
        foreach (var col in _collected)
        {
            col.parent = _offloadSpot;
        }
        _offloadSpot.parent.parent.GetComponent<Offloading>().DispenseMoney(_collected.Count);
        // for (int i = 0; i < _collected.Count; i++)
        // {
        //     _collected.RemoveAt(0);
        // }
        _collected.Clear();
    }

    public void ChangeCount(int value)
    {
        maxCount = value;
    }

    public void ChangeRadius(float value)
    {
        attractionRadius = value;
    }

    public void StartSet(Transform offload, GameObject upgradeUi)
    {
        _offloadSpot = offload;
        _upgradeUI = upgradeUi;
    }
}