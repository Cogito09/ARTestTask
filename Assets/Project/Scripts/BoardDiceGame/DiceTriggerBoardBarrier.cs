using System;
using UnityEngine;

public class DiceTriggerBoardBarrier : MonoBehaviour
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