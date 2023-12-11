using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DiceFaceBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private GameObject _textHolder;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _spriteHolder;
    [SerializeField] private GameObject _gameplayViewHolder;

    [ReadOnly]public DiceFaceConfig DiceConfig;
    public int Score => DiceConfig.Score;

    public void Setup(DiceFaceConfig diceFaceConfig)
    {
        DiceConfig = diceFaceConfig;
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
    public void SetupVisualSize(float faceSize)
    {
        var scale = new Vector3(faceSize, faceSize, faceSize);;
        transform.localScale = scale;
        transform.localScale = scale;
    }

    public void SetupVisualRotation(float rotationZ)
    {
        var rotation = Quaternion.Euler(new Vector3(0, 0, rotationZ));
        _gameplayViewHolder.transform.localRotation = rotation;
    }
    
    public void SetupFacePostionShift(float facePositionShift)
    {
        _gameplayViewHolder.transform.localPosition = new Vector3(0, 0, facePositionShift);
    }
#endif
}