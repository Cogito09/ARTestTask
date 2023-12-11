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

    private bool _visibilityState;
    public void ChangeVisibility(bool b)
    {
        if (_visibilityState == b)
        {
            return;
        }
 
        _visibilityState = b;
        Debug.Log($"Hand visibility Changed to {b}, launching puff");
        GameMaster.Spawner.SpawnAtPosition(MainConfig.GameplayConfig.PoofEffetPrefab,  transform.position);

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