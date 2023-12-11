using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DiceFaceBehaviour : MonoBehaviour
{
    [InfoBox("Remember to setup transform of this object as clos as possible to center of dice face. " +
                                   "Position is used for result calculation!")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _textHolder;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _spriteHolder;

    [ReadOnly] private int Score;

    public void Setup(DiceFaceConfig diceFaceConfig)
    {
        _spriteHolder.gameObject.SetActive(diceFaceConfig.IsUsingSymbolSprite);
        if (diceFaceConfig.IsUsingSymbolSprite)
        {
            _sprite = diceFaceConfig.SymbolSprite;
        }

        var usingText = diceFaceConfig.IsUsingSymbolSprite == false;
        _textHolder.gameObject.SetActive(usingText);
        if (usingText)
        {
            _text.text = diceFaceConfig.IsSymbolTextSameAsScore ? diceFaceConfig.Score.ToString() : diceFaceConfig.SymbolText;
        }
    }
}