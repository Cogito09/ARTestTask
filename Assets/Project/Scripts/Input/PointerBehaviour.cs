using System;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject _ableToGrabLook;
    [SerializeField] private GameObject _freeHandLook;
    [SerializeField] private GameObject _grabbedHandLook;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dice") == false)
        {
            return;
        }

        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = true;
        //Debug.Log($"IsAbleToGrabDice is true");
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

    public void AbleToGrabVisualState()
    {
        _freeHandLook.gameObject.SetActive(false);
        _grabbedHandLook.gameObject.SetActive(false);
        _ableToGrabLook.gameObject.SetActive(true);
    }

    public void FreeVisualState()
    {
        _freeHandLook.gameObject.SetActive(true);
        _grabbedHandLook.gameObject.SetActive(false);
        _ableToGrabLook.gameObject.SetActive(false);
    }
}