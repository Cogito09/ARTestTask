using System;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _holder;
    //[SerializeField] private GameObject _ableToGrabHolder;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dice") == false)
        {
            return;
        }

        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = true;
        Debug.Log($"IsAbleToGrabDice is true");
    }
    
    private void LateUpdate()
    {
        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
    }

    private void OnDisable()
    {
        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
    }

    public void ChangeVisibility(bool b)
    {
        _holder.gameObject.SetActive(b);
    }

    public void AbleToGrab()
    {

    }
}