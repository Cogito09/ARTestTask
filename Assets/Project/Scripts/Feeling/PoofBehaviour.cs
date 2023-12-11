using System.Collections;
using UnityEngine;

public class PoofBehaviour : MonoBehaviour
{
    [SerializeField] private float _duration;
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }
}