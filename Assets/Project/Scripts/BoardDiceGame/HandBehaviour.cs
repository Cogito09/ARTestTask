using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _grabbedGraphicsRepresentation;
    [SerializeField] private GameObject _freeGraphicsRepresentation;
    private bool _state;
    [SerializeField] private GameObject _holder;
    public Transform DiceHolder;

    private void Awake()
    {
        OnStateChanged();
    }
    
    public void ChangeGrabbedState(bool state)
    {
        _state = !state;
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        _grabbedGraphicsRepresentation.gameObject.SetActive(_state);
        _grabbedGraphicsRepresentation.gameObject.SetActive(!_state);
    }

    public void Throw()
    {

    }

    public void ChangeVisibility(bool b)
    {
        _holder.gameObject.SetActive(b);
    }

    public void AbleToGrab()
    {

    }
}