using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[Serializable]
struct CarsParts
{
    public Crane crane;
    public Attractor attractor;
    public PlayerScript playerScript;
    public GameObject car;
}
public class CarMain : MonoBehaviour
{
    public static CarMain Instance;
    
    [SerializeField] private CarsParts[] carPartsArray = new CarsParts[5];
    [SerializeField] private Transform offloadSpot;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Offloading offloading;

    [SerializeField] private GameObject text;

    private int _id;
    private float _radius=4;
    private int _count=5;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CarInitializer();
    }


    private void CarInitializer()
    {
        for (int i = 0; i < carPartsArray.Length; i++)
        {
            carPartsArray[i].car.SetActive(false);
        }
        virtualCamera.m_Follow = carPartsArray[_id].playerScript.transform;
        playerMovement.ChangePlayer(carPartsArray[_id].playerScript.gameObject);
        carPartsArray[_id].car.SetActive(true);
        carPartsArray[_id].attractor.ChangeRadius(_radius);
        carPartsArray[_id].attractor.ChangeCount(_count);
        carPartsArray[_id].playerScript.SetTarget(target);
        carPartsArray[_id].attractor.StartSet(offloadSpot,upgradeUI);
        if(_id!=0){
            carPartsArray[_id].car.transform.GetChild(0).transform.position = carPartsArray[_id - 1].car.transform.GetChild(0).transform.position;
            carPartsArray[_id].car.transform.GetChild(0).transform.rotation = carPartsArray[_id - 1].car.transform.GetChild(0).transform.rotation;
        }
    }

    public void SetInt(int value)
    {
        if (_id.Equals(value)) return;
        _id = value;
        CarInitializer();
    }

    public void ChangeRadius(float value)
    {
        _radius = value;
        carPartsArray[_id].attractor.ChangeRadius(value);
    }

    public void ChangeCount(int value)
    {
        _count = value;
        carPartsArray[_id].attractor.ChangeCount(value);
    }
    public void ShowText(Transform value){
        var position = new Vector3(UnityEngine.Random.Range(-1f, 1), 3.32f, UnityEngine.Random.Range(-1, 1));
        var temp = Instantiate(text);
        temp.transform.position = value.position+position;
        temp.SetActive(true);
    }
}
