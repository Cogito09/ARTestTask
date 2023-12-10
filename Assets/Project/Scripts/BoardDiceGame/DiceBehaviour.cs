using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DiceBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public Action<int> OnDiceResultCaptured;
    public Action OnDiceFailedToCaptureResult;

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

    }

    public void StartListenForResult()
    {

    }

    public void HighlightResult(float pauseDurationAfterResultAnnounced)
    {

    }
}