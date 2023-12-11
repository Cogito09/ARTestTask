using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

using Vector3 = UnityEngine.Vector3;

public class DiceBehaviour : MonoBehaviour
{
    [SerializeField] private List<DiceFaceBehaviour> _faces;
    
    [SerializeField] private Rigidbody _rigidbody;
    public Action<int> OnDiceResultCaptured;
    public Action OnDiceFailedToCaptureResult;
    public Action OnDiceUnableToGetClearResult;
    private bool _isListeningForResult;

    [ReadOnly][SerializeField] private int _motionlessCounter;
    [SerializeField] private float _minMotionlessFramesToProceedResult;
    [FormerlySerializedAs("_maxDistanceToRegisterPositonsAsEqual")] [SerializeField] private float _maxPositionsEqualityDistance;
    [SerializeField] private float _maxRotationEqualityEulerAngleDifference;

    public void Setup(DiceConfig diceConfig)
    {
        
    }
    
    public void ChangeKinematic(bool b)
    {
        _rigidbody.isKinematic = b;
    }

    public void Throw(Vector3 throwForce,Vector3 throwTorque)
    {
        _rigidbody.AddForce(throwForce);
        _rigidbody.AddTorque(throwTorque);
    }

    public void StopListenForResult()
    {
        _isListeningForResult = false;
        Reset();
    }

    public void StartListenForResult()
    {
        _isListeningForResult = true;
        Reset();
    }

    private void Reset()
    {
        _motionlessCounter = 0;
    }

    private void FixedUpdate()
    {
        if (_isListeningForResult == false)
        {
            return;
        }

        WatchMotion();
        VerifyMontionless();
    }

    private void VerifyMontionless()
    {
        if (_motionlessCounter < _minMotionlessFramesToProceedResult)
        {
            return;
        }

        TryProceedResult();
    }

    private void TryProceedResult()
    {
        
    }
    
    private void WatchMotion()
    {
        var areRotationsAlmostEqual = AreVectorsAlmostEqual(_rigidbody.angularVelocity,Vector3.zero);
        var arePositionsAlmostEqual = AreVectorsAlmostEqual(_rigidbody.velocity, Vector3.zero);
        if (areRotationsAlmostEqual && arePositionsAlmostEqual)
        {
            _motionlessCounter += 1;
        }
        else
        {
            _motionlessCounter = 0;
        }
    }

    private bool AreVectorsAlmostEqual(Vector3 a, Vector3 b)
    {
        var delta = _maxRotationEqualityEulerAngleDifference;
        var value = IsAlmostEqual(a.x, b.x, delta) && 
                    IsAlmostEqual(a.y, b.y, delta) && 
                    IsAlmostEqual(a.z, b.z, delta);
        
        return value;
    }

    private bool IsAlmostEqual(float a, float b, float delta)
    {
        return Mathf.Abs(a-b) < delta; 
    }

    private bool ArePositionsAlmostEqual(Vector3 currentPosition, Vector3 lastPostion)
    {
        return Vector3.Distance(currentPosition, lastPostion) < _maxPositionsEqualityDistance;
    }

    public void HighlightResult(float pauseDurationAfterResultAnnounced)
    {

    }
}