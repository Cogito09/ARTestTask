using UnityEngine;

public class BoardBarrier : MonoBehaviour
{
    [SerializeField] private Collider _barrierCollider;
    
    public void ChangeBarrierState(bool state)
    {
        _barrierCollider.enabled = state;
    }
}