using System;
using Sirenix.OdinInspector;
using UnityEngine;

public enum HandAndPointerState
{
    Unknown = 0,
    NonVisible = 1,
    AbleToGrab = 2,
    Grab = 3,
    Throw = 4,
    Visible = 5
}
    
public class InputBehaviour : MonoBehaviour
{
    private int InputLayer;
    public float MaxDistance = 5000;
    private DiceBehaviour DiceBehaviour => GameMaster.CurrentActiveBoardDiceGameBehaviour.DiceBehaviour;
    private HandBehaviour Hand  => GameMaster.CurrentActiveBoardDiceGameBehaviour.Hand;
    private PointerBehaviour Pointer => GameMaster.CurrentActiveBoardDiceGameBehaviour.Pointer;
    private BoardDiceGameBehaviour BoardDiceGame => GameMaster.CurrentActiveBoardDiceGameBehaviour;
    
    [ReadOnly] public bool IsAbleToGrabDice;
    [ReadOnly] public bool Grabbed;

    public void Initialize()
    {
        InputLayer =  LayerMask.GetMask("BoardInput");
        ResetVelocity();
    }
    
    private void FixedUpdate()
    {
        if (Grabbed)
        {
            RegisterVelocity();
        }
    }

    private void Update()
    {
        StreamInputPosition();

    }

    private float _currentVelocity;
    private Vector3 _lastPosition;
    private void RegisterVelocity()
    {
        var currentPosition = Hand.transform.position;
        _currentVelocity = Vector3.Distance(_lastPosition, currentPosition);
        _lastPosition = Hand.transform.position;
    }

    private void ResetVelocity()
    {
        _currentVelocity = 0;
        _lastPosition = Hand.transform.position;
    }
    
    private void TryThrow()
    {
        Debug.Log($"Tried throw with velocity {_currentVelocity}");
    }

    private void StreamInputPosition()
    {
        var isHolding = Input.GetMouseButton(0);
        var isReleased = Input.GetMouseButtonUp(0);
        var pointer = Input.mousePosition;
        var ray = GameMaster.MainCamera.ScreenPointToRay(new Vector3(pointer.x, pointer.y, 0));
        var raycastHits = Physics.RaycastAll(ray,MaxDistance,InputLayer);
        
        if (Grabbed && isReleased)
        {
            TryThrow();
            Grabbed = false;
        }

        var ableToThrow = Grabbed && isHolding;
        if (raycastHits == null || raycastHits.Length <= 0)
        {
            if (ableToThrow)
            {
                TryThrow();
                Grabbed = false;
                return;
            }
            
            ChangePointerAndHandState(HandAndPointerState.NonVisible);
            return;
        }

        var hit = raycastHits[0];
        if (IsAbleToGrabDice && isHolding == false)
        {
            //Debug.Log($"Able To grab");
            ChangePointerAndHandState(HandAndPointerState.AbleToGrab);
        }
        else if (IsAbleToGrabDice && isHolding)
        {
            //Debug.Log($"Grabbed");
            Grabbed = true;
            GrabDice();
            ResetVelocity();
            ChangePointerAndHandState(HandAndPointerState.Grab);
        }
        else
        {
            ChangePointerAndHandState(HandAndPointerState.Visible);
        }
        
        UpdateRaycastHit(hit);
    }

    private void GrabDice()
    {
        DiceBehaviour.ChangeKinematic(true);
        DiceBehaviour.transform.SetParent(Hand.DiceHolder);
        DiceBehaviour.transform.localPosition = Vector3.zero;
    }

    private void ChangePointerAndHandState(HandAndPointerState state)
    {
        switch (state)
        {
            case HandAndPointerState.Unknown:
                break;
            case HandAndPointerState.NonVisible:
                Hand.ChangeVisibility(false);
                Pointer.ChangeVisibility(false);
                break;
            case HandAndPointerState.Visible:
                Hand.ChangeVisibility(true);
                Pointer.ChangeVisibility(true);
                break;
            case HandAndPointerState.AbleToGrab:
                Hand.ChangeVisibility(true);
                Hand.AbleToGrab();
                Pointer.ChangeVisibility(true);
                Pointer.AbleToGrab();
                break;
            case HandAndPointerState.Grab:
                Hand.ChangeGrabbedState(true);
                Pointer.ChangeVisibility(false);
                break;
            case HandAndPointerState.Throw:
                Hand.Throw();
                Pointer.ChangeVisibility(false);
                break;
        }
    }

    private void UpdateRaycastHit(RaycastHit hit)
    {
        var pos = hit.point;
        Pointer.transform.position = pos;
        Hand.transform.position = pos;
    }
}