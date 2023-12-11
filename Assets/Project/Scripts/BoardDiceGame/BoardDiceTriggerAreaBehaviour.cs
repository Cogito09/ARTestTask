using System;
using UnityEngine;

public class BoardDiceTriggerAreaBehaviour : MonoBehaviour
{
    public Action OnDiceInsidePlayground;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dice") == false)
        {
            return;
        }
        
        OnDiceInsidePlayground?.Invoke();
    }
}