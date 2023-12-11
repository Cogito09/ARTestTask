using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

public class PointerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject _ableToGrabLook;
    [SerializeField] private GameObject _freeHandLook;
    [SerializeField] private GameObject _grabbedHandLook;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dice") == false)
        {
            return;
        }

        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = true;
        //Debug.Log($"IsAbleToGrabDice is true");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dice") == false)
        {
            return;
        }

        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
        //Debug.Log($"IsAbleToGrabDice is true");
    }
    // private void LateUpdate()
    // {
    //     GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
    // }

    private void OnDisable()
    {
        GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
    }
    
    
    private bool _visibilityState;
    public void ChangeVisibility(bool b)
    {
        if (_visibilityState == b)
        {
            return;
        }

        _visibilityState = b;
        Debug.Log($"Pointer visibility Changed to {b}, launching puff");
        GameMaster.Spawner.SpawnAtPosition(MainConfig.GameplayConfig.PoofEffetPrefab,  transform.position);
        if (b == false)
        {
            GameMaster.CurrentActiveBoardDiceGameBehaviour.Input.IsAbleToGrabDice = false;
        }
        
        _holder.gameObject.SetActive(b);
    }

    public void AbleToGrabVisualState()
    {
        _freeHandLook.ChangeActive(false);
        _grabbedHandLook.ChangeActive(false);
        
        _ableToGrabLook.ChangeActive(true);
    }

    public void FreeVisualState()
    {
        _grabbedHandLook.ChangeActive(false);
        _ableToGrabLook.ChangeActive(false);
        
        _freeHandLook.ChangeActive(true);
    }
    
    public void GrabbedVisualState()
    {
        _freeHandLook.ChangeActive(false);
        _ableToGrabLook.ChangeActive(false);
        
        _grabbedHandLook.ChangeActive(true);
    }
}