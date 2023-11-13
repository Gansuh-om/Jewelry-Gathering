using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class Destroyer : MonoBehaviour
{
    private async void OnEnable() {
        await Task.Delay(500);
        this.gameObject.SetActive(false);
    }
}
