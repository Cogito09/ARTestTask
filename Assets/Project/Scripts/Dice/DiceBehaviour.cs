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
    [PropertyOrder(2)][InfoBox("Guide How to setup Dice")] 
    [InfoBox("!!Please Edit on Scene. Editing in prefab sub-scene cause unexpected behaviour!!",InfoMessageType.Warning)] 
    [InfoBox("1. First add mesh, and generate new collider. If collider doesnt get updated exit->enter prefab mode or edit in scene view")]
    [InfoBox("2. Add Dice_Face.prefab for each face and place it under root of this object")]
    [InfoBox("3. Reference all the Dice_Face.prefab's in Faces list underneith")]
    [SerializeField] private List<DiceFaceBehaviour> _faces;
    [SerializeField] private Rigidbody _rigidbody;
    public Action<int> OnDiceResultCaptured;
    public Action OnDiceFailedToCaptureResult;
    private bool _isListeningForResult;

    [ReadOnly][SerializeField] private int _motionlessCounter;
    [SerializeField] private float _minMotionlessFramesToProceedResult;
    [SerializeField] private float _maxPositionsEqualityDistance;
    [SerializeField] private float _maxRotationEqualityEulerAngleDifference;

    [SerializeField] private GameObject _highlightEffect;
    private float _highlightEffectFinishTimestamp;
    
    private void Awake()
    {
        _faceToEdit = null;
        if (_highlightEffect != null)
        {
            _highlightEffect.ChangeActive(false);
        }
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
    
    private void TryProceedResult()
    {
        var highestPointingFacePosition = 0f;
        DiceFaceBehaviour highestPointingFace = null;
        for (var i = 0; i < _faces.Count; i++)
        {
            if (_faces[i].transform.position.y < highestPointingFacePosition)
            {
                continue;
            }

            highestPointingFacePosition = _faces[i].transform.position.y;
            highestPointingFace = _faces[i];
        }

        if (highestPointingFace == null)
        {
            OnDiceFailedToCaptureResult?.Invoke();
            return;
        }
        
        OnDiceResultCaptured?.Invoke(highestPointingFace.Score);
    }
    
    public void ChangeKinematic(bool b)
    {
        _rigidbody.isKinematic = b;
    }

    public void Throw(Vector3 throwForce,Vector3 throwTorque)
    {
        _rigidbody.AddForce(throwForce,ForceMode.Impulse);
        //_rigidbody.AddTorque(throwTorque, ForceMode.Impulse);
        _rigidbody.angularVelocity = throwTorque;
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

        TrySwtichOffHighlight();
    }

    private void TrySwtichOffHighlight()
    {
        if (Time.time < _highlightEffectFinishTimestamp)
        {
            return;
        }

        if (_highlightEffect != null)
        {
            _highlightEffect.ChangeActive(false);
        }
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
        if (_highlightEffect == null)
        {
            Debug.LogError($"_highlightEffect is not assinged in Dice named : {gameObject.name}");
            return;
        }
        
        _highlightEffect.ChangeActive(true);
        _highlightEffectFinishTimestamp = Time.time + pauseDurationAfterResultAnnounced;
    }
    
    public void ResetVelocities()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

#if UNITY_EDITOR
    [FormerlySerializedAs("_faceEdited")]
    [FormerlySerializedAs("_faceToPlacePosition")]
    [PropertyOrder(3)][InfoBox("4. Reference Dice_Face under FaceToEdit that you want to place on Dice")]
    [SerializeField] private DiceFaceBehaviour _faceToEdit;
    
    [PropertyOrder(4)]
    [InfoBox("5. Hold  [CTRL] and hoover over Dice for automatic placement")]
    [InfoBox("6. Adjust edited face rotation")]
    [PropertyRange(0, 360)]
    public float FaceRotation = 0f;
    
    [PropertyOrder(6)]
    [InfoBox("6. Adjust face position")]
    [PropertyRange(0, 0.15f)]
    public float FacePositionShift = 0.01f;
    
    [PropertyOrder(7)]
    [InfoBox("7. Adjust sizes")]
    [PropertyRange(0, "FaceMaxSizeRange")]
    public float FaceSize = 1;
    [PropertyOrder(10)]
    public float FaceMaxSizeRange = 5;

    private bool _wasRegistered = false;


    private void OnValidate()
    {
        _wasRegistered = false;
        if (_faceToEdit != null)
        {
            _faceToEdit.SetupVisualRotation(FaceRotation);
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
    
    public void RegisterEditorHotkey()
    {
        if (_wasRegistered)
        {
            return;
        }

        _wasRegistered = true;
        SceneView.onSceneGUIDelegate -= OnSceneGUIDelegate;
        SceneView.onSceneGUIDelegate += OnSceneGUIDelegate;
    }

    private void OnSceneGUIDelegate(SceneView sceneview)
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
        if (_faceToEdit == null)
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

        _faceToEdit.transform.position = hitPoint;
        
        var faceRotation =  Quaternion.LookRotation(_faceToEdit.transform.localPosition, Vector3.up);
        var vectorFromCenter = transform.position - _faceToEdit.transform.position;
        _faceToEdit.transform.SetPositionAndRotation(_faceToEdit.transform.position,faceRotation);
    }
#endif
}