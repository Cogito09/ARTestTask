using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DiceFaceBehaviour : MonoBehaviour
{
    [InfoBox("Remember to setup transform of this object as clos as possible to center of dice face. " +
                                   "Position is used for result calculation!")]
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private GameObject _textHolder;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _spriteHolder;
    
    [SerializeField] private GameObject _debugPostionMarker;
    [SerializeField] private GameObject _gameplayViewHolder;

    public void Setup(DiceFaceConfig diceFaceConfig)
    {
        SetupDebugMode(false);
        
        _spriteHolder.gameObject.SetActive(diceFaceConfig.IsUsingSymbolSprite);
        if (diceFaceConfig.IsUsingSymbolSprite)
        {
            _sprite.sprite = diceFaceConfig.SymbolSprite;
        }

        var usingText = diceFaceConfig.IsUsingSymbolSprite == false;
        _textHolder.gameObject.SetActive(usingText);
        if (usingText)
        {
            _text.text = diceFaceConfig.IsSymbolTextSameAsScore ? diceFaceConfig.Score.ToString() : diceFaceConfig.SymbolText;
        }
    }
    
#if UNITY_EDITOR
    [Button]
    public void TurnDebugModeOn()
    {
        SetupDebugMode(true);
    }
    
    [Button]
    public void TurnDebugModeOff()
    {
        SetupDebugMode(false);
    }
    
    public void SetupDebugMode(bool b)
    {
        _debugPostionMarker.SetActive(b);
        _gameplayViewHolder.SetActive(!b);
    }

    public void SetupVisualSize(float faceSize)
    {
        var scale = new Vector3(faceSize, faceSize, faceSize);;
        _gameplayViewHolder.transform.localScale = scale;
        _debugPostionMarker.transform.localScale = scale;
    }
#endif
  
}