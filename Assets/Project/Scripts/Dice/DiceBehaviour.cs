using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
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
    [SerializeField] private float _maxPositionsEqualityDistance;
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
    
    private void Update()
    {
        RegisterEditorHotkey();
        //UpdatePointer();
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


#if UNITY_EDITOR
    [SerializeField] private DiceFaceBehaviour _faceToPlacePosition;
    private bool _wasRegistered = false;
    
    public void RegisterEditorHotkey()
    {
        if (_wasRegistered)
        {
            return;
        }

        _wasRegistered = true;
        SceneView.onSceneGUIDelegate += view =>
        {
            var e = Event.current;
            if (e == null)
            {
                return;
            }

            if (e.control == true)
            {
                Debug.Log($"control clicked");
                UpdatePointer();
            }
        };
    }
    
    private void UpdatePointer()
    {
        Debug.Log($"clicled space!");
        if (_faceToPlacePosition == null)
        {
            return;
        }
        
        if (SceneView.lastActiveSceneView == null)
        {
            return;
        }
        
        Vector3 mousePosition = Event.current.mousePosition;
        var mouseRay = HandleUtility.GUIPointToWorldRay(mousePosition);//SceneView.lastActiveSceneView.camera.ScreenPointToRay(mousePosition);// Camera.main.ScreenPointToRay();
        var allHits = Physics.RaycastAll(mouseRay, 1000000f);
        var isHit = false;
        var hitPoint = Vector3.zero;    
        allHits.ForEach(hit =>
        {
            if (hit.collider.gameObject.tag == "Dice")
            {
                isHit = true;
                hitPoint = hit.point;
            }
        });
       
        if (isHit == false)
        {
            return;
        }

        _faceToPlacePosition.transform.position = hitPoint;
        
        var faceRotation =  Quaternion.LookRotation(_faceToPlacePosition.transform.localPosition, Vector3.up);
        var vectorFromCenter = transform.position - _faceToPlacePosition.transform.position;
        _faceToPlacePosition.transform.SetPositionAndRotation(_faceToPlacePosition.transform.position,faceRotation);
    }
    
    [PropertyOrder(2)][Button]
    public void SetupFacesRoatations()
    {
        if (_faces == null)
        {
            return;
        }

        for (var i = 0; i < _faces.Count; i++)
        {
            var face =_faces[i];
            if (face != null)
            {
                var faceRotation =  Quaternion.LookRotation(face.transform.localPosition, Vector3.up);
                var vectorFromCenter = transform.position - face.transform.position;
                face.transform.SetPositionAndRotation(face.transform.position,faceRotation);
            }
        }
        
        PreviewFacesMode();
    }

    [PropertyOrder(2)][Button]
    public void EnableSetPostionMode()
    {
        for (var i = 0; i < _faces.Count; i++)
        {
            var face = _faces[i];
            face.SetupDebugMode(true);
        }
    }

    [PropertyOrder(2)][Button]
    public void PreviewFacesMode()
    {
        for (var i = 0; i < _faces.Count; i++)
        {
            var face = _faces[i];
            face.SetupDebugMode(false);
        }
    }

    [PropertyRange(0, "FaceMaxRange"), PropertyOrder(3)]
    public float FaceSize = 1;
    [PropertyOrder(4)]
    public float FaceMaxRange = 5;

    private void OnValidate()
    {
        _wasRegistered = false;
        for (var i = 0; i < _faces.Count; i++)
        {
            var face = _faces[i];
            if (face == null)
            {
                continue;
            }
            
            face.SetupVisualSize(FaceSize);
        }
    }
#endif
}