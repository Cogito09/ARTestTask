using UnityEngine;

public class DiceBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public void ChangeKinematic(bool b)
    {
        _rigidbody.isKinematic = b;
    }
}