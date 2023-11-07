using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Offloading : MonoBehaviour
{
    [SerializeField] private GameObject money;
    [SerializeField] private Transform spot;
    [SerializeField] private Transform moneyFrom;
    [SerializeField] private Transform moneyTo;
    [SerializeField] private List<Transform> moneyDropLocations;
    private Vector3 _tempVec = new Vector3(0,1,0);
    private int _tempId;
    public static Offloading Instace;

    private void Awake(){
        Instace = this;
    }

    public Transform GetSpot(){
        return spot;
    }
    public void DispenseMoney(int count){
        for(int i=0;i<count;i++){
            if(_tempId>=20){
                _tempId=0;
            }
            var started = Instantiate(money,this.transform);
            started.transform.position = moneyFrom.position;
            started.transform.DOLocalRotate(new Vector3(0,90,0),0.1f);
            if(i!=0){
                started.transform.DOMove(moneyTo.transform.position,0.5f).SetDelay(_tempId*0.1f).OnComplete(()=>{
                    started.transform.DOMove(moneyDropLocations[_tempId].position+_tempVec,0.1f).OnComplete(()=>{
                        started.GetComponent<Rigidbody>().isKinematic=false;
                        // started.transform.DOMove(started.transform.position-_tempVec,0.1f).OnComplete(()=>{
                        // });
                    });
                    _tempId++;
                    if(_tempId>=20){
                        _tempId=0;
                    }
                });
            }else{
                
                started.transform.DOMove(moneyTo.transform.position,0.5f).OnComplete(()=>{
                    started.transform.DOMove(moneyDropLocations[_tempId].position+_tempVec,0.1f).OnComplete(()=>{
                        started.GetComponent<Rigidbody>().isKinematic=false;
                        // started.transform.DOMove(started.transform.position-_tempVec,0.1f).OnComplete(()=>{
                        // });
                    });
                    _tempId++;
                    if(_tempId>=20){
                        _tempId=0;
                    }
                });
            }
        }
    }
    private void Update(){
        if(Input.GetKeyDown("a")){
            DispenseMoney(20);
        }
    }
}