using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
public class DiceBehaviour : MonoBehaviour
{
    [PropertyOrder(2)][InfoBox("Guide How to setup Dice")] 
    [InfoBox("1. First add mesh, and generate new collider. If collider doesnt get updated exit->enter prefab mode or edit in scene view")]
    [InfoBox("2. Put all needed Dice_Face.prefab 's for each face under root of this object")]
    [InfoBox("3. Reference all the faces in Faces list underneith")]
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

    private void Awake()
    {
        _faceToPlacePosition = null;
    }
    
    public void Setup(DiceConfig diceConfig)
    {
        if (diceConfig == null)
        {
            Debug.LogError($"DiceConfig is null");
            return;
        }
        
        if (_faces == null)
        {
            Debug.LogError($"Faces list is null");
            return;
        }

        var diceFacesList = diceConfig.Faces;
        if (_faces.Count != diceFacesList.Count)
        {
            Debug.LogError($"Faces list count is not equal to configs faces count. Please check DicePrefab and DiceConfig Faces of CofngiId: {diceConfig}");
            return;
        }
        
        for (var i = 0; i < diceFacesList.Count; i++)
        {
            var diceFaceCfg = diceFacesList[i];
            if (_faces.Count <= i)
            {
                Debug.LogError($"_faces.Count is to low");
                continue;
            }

            var faceBehaviour = _faces[i];
            if (faceBehaviour == null)
            {
                Debug.LogError($"faceBehaviour on index: {i} is null.");
                continue;
            }
            
            _faces[i].Setup(diceFaceCfg);
        }
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
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            RegisterEditorHotkey();
        }
#endif

        
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
    
    public void ResetVelocities()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

#if UNITY_EDITOR


    [PropertyOrder(3)][InfoBox("4. Reference Face that you want to setup here, hold CTRL and hoover over Dice for automatic placement")]
    [SerializeField] private DiceFaceBehaviour _faceToPlacePosition;
    
    [PropertyOrder(4)]
    [InfoBox("5. Adjust edited face rotation")]
    [PropertyRange(0, 360)]
    public float FaceRotation = 0f;
    
    [PropertyOrder(6)]
    [InfoBox("6. Adjust face position")]
    [PropertyRange(0, 0.25f)]
    public float FacePositionShift = 0.1f;
    
    [PropertyOrder(7)]
    [InfoBox("7. Adjust sizes")]
    [PropertyRange(0, "FaceMaxSizeRange")]
    public float FaceSize = 1;
    [PropertyOrder(10)]
    public float FaceMaxSizeRange = 5;

    private void OnValidate()
    {
        _wasRegistered = false;
        if (_faceToPlacePosition != null)
        {
            _faceToPlacePosition.SetupVisualRotation(FaceRotation);
        }
        
        for (var i = 0; i < _faces.Count; i++)
        {
            var face = _faces[i];
            if (face == null)
            {
                continue;
            }
            
            face.SetupVisualSize(FaceSize);
            face.SetupFacePostionShift(FacePositionShift);
        }
    }
    
    private bool _wasRegistered = false;
    public void RegisterEditorHotkey()
    {
        if (_wasRegistered)
        {
            return;
        }

        _wasRegistered = true;
        SceneView.onSceneGUIDelegate -= ONSceneGUIDelegate;
        SceneView.onSceneGUIDelegate += ONSceneGUIDelegate;
    }

    private void ONSceneGUIDelegate(SceneView sceneview)
    {
        var e = Event.current;
        if (e == null)
        {
            return;
        }

        if (e.control == true)
        {
            UpdatePointer();
        }
    }

    private void UpdatePointer()
    {
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
#endif
}