using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
   private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            CarMain.Instance.ShowText(transform);
            Upgrades.Instance.IncreaseMoney(10);
            this.transform.parent.gameObject.SetActive(false);
            Destroy(gameObject);
        }
   }
}
