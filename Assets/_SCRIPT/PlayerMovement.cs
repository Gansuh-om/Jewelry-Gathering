using System.Collections;
using System.Collections.Generic;
using CnControls;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Transform _player;

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

        if (_hold)
        {
            var horizontalInput = CnInputManager.GetAxis("Horizontal");
            var verticalInput = CnInputManager.GetAxis("Vertical");
            var value = Mathf.Clamp(_player.position.x - target.position.x + horizontalInput, -1, 1);
            var value2 = Mathf.Clamp(_player.position.z - target.position.z + verticalInput, -1, 1);
            target.position = new Vector3(target.position.x+value, target.localPosition.y, target.position.z+value2);
        }
    }
    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // First, normalize the value within the source range (fromMin to fromMax) to a 0-1 range.
        float normalizedValue = Mathf.InverseLerp(fromMin, fromMax, value);

        // Then, map the normalized value to the target range (toMin to toMax).
        return Mathf.Lerp(toMin, toMax, normalizedValue);
    }

    public void ChangePlayer(GameObject value)
    {
        _player = value.transform;
    }
}
