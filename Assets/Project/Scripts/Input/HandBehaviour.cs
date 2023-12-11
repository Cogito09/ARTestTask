using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject _ableToGrabLook;
    [SerializeField] private GameObject _freeHandLook;
    [SerializeField] private GameObject _grabbedHandLook;
    [SerializeField] private GameObject _throwLook;
    public Transform DiceHolder;

    private void Awake()
    {
        FreeVisualState();
    }
    
    public void ChangeVisibility(bool b)
    {
        _holder.ChangeActive(b);
    }

    public void AbleToGrabVisualState()
    {
        _freeHandLook.ChangeActive(false);
        _grabbedHandLook.ChangeActive(false);
        _ableToGrabLook.ChangeActive(true);
        _throwLook.ChangeActive(false);
    }

    public void FreeVisualState()
    {
        _freeHandLook.ChangeActive(true);
        _grabbedHandLook.ChangeActive(false);
        _ableToGrabLook.ChangeActive(false);
        _throwLook.ChangeActive(false);
    }

    public void GrabbedVisualState()
    {
        _freeHandLook.ChangeActive(false);
        _grabbedHandLook.ChangeActive(true);
        _ableToGrabLook.ChangeActive(false);
        _throwLook.ChangeActive(false);
    }

    public void ThrowVisualState()
    {
        _freeHandLook.ChangeActive(false);
        _grabbedHandLook.ChangeActive(false);
        _ableToGrabLook.ChangeActive(false);
        _throwLook.ChangeActive(true);
    }
}