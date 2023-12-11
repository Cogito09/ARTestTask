using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

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
    [ReadOnly] public bool IsAbleToGrabDice;
    [ReadOnly] public bool Grabbed;
    
    [SerializeField] private float _maxVelocityForRandomThrow;
    [SerializeField] private float _minVelocityForRandomThrow;
    [SerializeField] private float _minVelocityForThrow;
    [SerializeField] private float _throwModificator;
    [SerializeField] private float _torqueForceRangeMin;
    [SerializeField] private float _torqueForceRangeMax;
    [SerializeField] private Vector3 _torqueBase;
    [SerializeField] private int _numberOfLastRegisteredVelocities;
    [SerializeField] private float MaxRaycastDistance = 5000;
    [SerializeField] private float _downForce = 0.3f;
    private readonly LinkedList<float> _lastVelocities = new LinkedList<float>();
    private Vector3 _lastPosition;
    private Vector3 _currentMoveVector;
    private int _inputLayer;

    private DiceBehaviour DiceBehaviour => GameMaster.CurrentActiveBoardDiceGameBehaviour.DiceBehaviour;
    private HandBehaviour Hand  => GameMaster.CurrentActiveBoardDiceGameBehaviour.Hand;
    private PointerBehaviour Pointer => GameMaster.CurrentActiveBoardDiceGameBehaviour.Pointer;
    private BoardDiceGameBehaviour BoardDiceGame => GameMaster.CurrentActiveBoardDiceGameBehaviour;
    
    
    public void Initialize()
    {
        _inputLayer =  LayerMask.GetMask("BoardInput");
        ResetVelocity();
    }

    private void OnValidate()
    {
        if (_numberOfLastRegisteredVelocities <= 0)
        {
            Debug.LogError($"_numberOfLastRegisteredVelocities is setup ${_numberOfLastRegisteredVelocities}. " +
                           $"This value must be greater than 0 to calculate average velocity.");
        }
    }

    private void Update()
    {
        StreamInputPosition();
        if (Grabbed)
        {
            RegisterVelocity();
        }
    }
    

    private void RegisterVelocity()
    {
        var currentPosition = Hand.transform.position;
        var currentVelocity = Vector3.Distance(_lastPosition, currentPosition);
        _currentMoveVector = currentPosition - _lastPosition;
        _lastPosition = Hand.transform.position;

        AddVelocity(currentVelocity);
    }

    private void AddVelocity(float currentVelocity)
    {
        _lastVelocities.AddFirst(currentVelocity);
        if (_lastVelocities.Count <= _numberOfLastRegisteredVelocities)
        {
            return;
        }
        
        _lastVelocities.RemoveLast();
    }
    
    private float GetAverageVelocity()
    {
         if (_lastVelocities.Count <= 0)
        {
            return 0;
        }
        
        var allVelocitiies = _lastVelocities.Sum();
        var average = allVelocitiies / _lastVelocities.Count;
        return average;
    }
    
    private void ResetVelocity()
    {
        _lastVelocities.Clear();
        _lastPosition = Hand.transform.position;
    }
    
    
    private void TryThrow()
    {
        var averageVelocity = GetAverageVelocity();
        Debug.Log($"Tried throw with velocity {averageVelocity}");
        if (averageVelocity < _minVelocityForThrow)
        {
            ResetDice();
            Debug.Log($"Velocity too slow, reseting.");
            return;
        }

        var downwardForce = new Vector3(0, (-1 )* -_downForce, 0);
        var forceVector = _currentMoveVector + downwardForce;
        Throw(averageVelocity,forceVector);
    }
    
    private void Throw(float velocity,Vector3 forceVector,bool ignorePointerUpdate = false)
    {
        DiceBehaviour.transform.SetParent(null);
        DiceBehaviour.ChangeKinematic(false);
        
        var throwForce = forceVector.normalized * velocity * _throwModificator;
        var torqueXRandom = Random.Range(_torqueForceRangeMin, _torqueForceRangeMax);
        var torqueYRandom = Random.Range(_torqueForceRangeMin, _torqueForceRangeMax);
        var torqueZRandom = Random.Range(_torqueForceRangeMin, _torqueForceRangeMax);
        
        var torqueForce =  new Vector3(
            _torqueBase.x *torqueXRandom,
            _torqueBase.y * torqueYRandom,
            _torqueBase.z * torqueZRandom
            );
        
        DiceBehaviour.Throw(throwForce,torqueForce);
        if (ignorePointerUpdate == false)
        {
            ChangePointerAndHandState(HandAndPointerState.Visible);
        }

        
        Debug.Log($"Velocity too slow, reseting.");
        BoardDiceGame.OnDiceRolledForScore();
    }
    
    public void ResetDice()
    {
        GameMaster.Spawner.SpawnAtPosition(MainConfig.GameplayConfig.PoofEffetPrefab,  DiceBehaviour.transform.position);
        GameMaster.Spawner.SpawnAtPosition(MainConfig.GameplayConfig.PoofEffetPrefab,  BoardDiceGame.DiceStartPosition.position);
        DiceBehaviour.transform.SetParent(null);
        DiceBehaviour.transform.position = BoardDiceGame.DiceStartPosition.position;
        DiceBehaviour.ResetVelocities();
        DiceBehaviour.ChangeKinematic(false);
    }

    private void StreamInputPosition()
    {
        var isHolding = Input.GetMouseButton(0);
        var isReleased = Input.GetMouseButtonUp(0);
        var pointer = Input.mousePosition;
        var ray = GameMaster.MainCamera.ScreenPointToRay(new Vector3(pointer.x, pointer.y, 0));
        var raycastHits = Physics.RaycastAll(ray,MaxRaycastDistance,_inputLayer);
        
        if (Grabbed && isReleased)
        {
            TryThrow();
            Grabbed = false;
            return;
        }
        
        var isPointerAboveBoardInputPlane = raycastHits != null && raycastHits.Length > 0;
        if (isPointerAboveBoardInputPlane == false)
        {
            if (Grabbed)
            {
                ResetDice();
            }

            ChangePointerAndHandState(HandAndPointerState.NonVisible);
            return;
        }

        var isAbleToInteractWithDice = BoardDiceGame.State == DiceGameState.PlayerInput;
        var boardInputPlaneRaycastHit = raycastHits[0];
        if (IsAbleToGrabDice && isHolding == false)
        {
            ChangePointerAndHandState(HandAndPointerState.AbleToGrab);
        }
        else if (IsAbleToGrabDice && isHolding && isAbleToInteractWithDice)
        {
            Grabbed = true;
            GrabDice();
            ResetVelocity();
            ChangePointerAndHandState(HandAndPointerState.Grab);
        }
        else if (Grabbed && isHolding)
        {
            // GrabMode , just Updating Pointer position
        }
        else
        {
            ChangePointerAndHandState(HandAndPointerState.Visible);
        }
        
        SetPointerOnRaycastHit(boardInputPlaneRaycastHit);
    }

    private void ReleaseDice()
    {
        ResetDice();
    }

    private void GrabDice()
    {
        DiceBehaviour.ChangeKinematic(true);
        DiceBehaviour.transform.SetParent(Hand.DiceHolder);
        DiceBehaviour.transform.localPosition = Vector3.zero;
    }

    private void ChangePointerAndHandState(HandAndPointerState state)
    {
        Debug.Log($"state id {state}");
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

                Hand.FreeVisualState();
                Pointer.FreeVisualState();
                break;
            case HandAndPointerState.AbleToGrab:
                Hand.ChangeVisibility(true);
                Pointer.ChangeVisibility(true);
                
                Hand.AbleToGrabVisualState();
                Pointer.AbleToGrabVisualState();
                break;
            case HandAndPointerState.Grab:
                Hand.ChangeVisibility(true);
                Pointer.ChangeVisibility(true);
                
                Hand.GrabbedVisualState();
                Pointer.GrabbedVisualState();
                break;
            case HandAndPointerState.Throw:
                Hand.ChangeVisibility(true);
                Pointer.ChangeVisibility(false);
                
                Hand.ThrowVisualState();
                break;
        }
    }

    private void SetPointerOnRaycastHit(RaycastHit hit)
    {
        var pos = hit.point;
        Pointer.transform.position = pos;
        Hand.transform.position = pos;
    }

    public void RandomThrow()
    {
        var randomizedVelocity = Random.Range(_minVelocityForRandomThrow,_maxVelocityForRandomThrow);
        var randomixedVector =  new Vector3(
            Random.Range(-1f,1f),
            Random.Range(-1f,0.1f),
            Random.Range(-1f,1f)
            );
        
        Throw(randomizedVelocity, randomixedVector.normalized,true);
    }
}