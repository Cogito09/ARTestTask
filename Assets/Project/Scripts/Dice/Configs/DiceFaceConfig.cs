using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DiceFaceConfig
{
    [ReadOnly] public int FaceIndex;
    public int Score;
    [ShowIf("IsShowingSymbolTextInEditor")]public string SymbolText;
    [ShowIf("IsShowingSymbolTextSameAsScore")]public bool IsSymbolTextSameAsScore = true;
    [ShowIf("IsShowingIsUsingSymbolSprite")]public bool IsUsingSymbolSprite;
    [ShowIf("IsShowingSymbolSpritee")]public Sprite SymbolSprite;
    
    private bool IsShowingSymbolTextInEditor => IsUsingSymbolSprite == false && IsSymbolTextSameAsScore == false;
    private bool IsShowingSymbolTextSameAsScore => IsUsingSymbolSprite == false;
    private bool IsShowingIsUsingSymbolSprite => IsSymbolTextSameAsScore == false;
    private bool IsShowingSymbolSpritee => IsUsingSymbolSprite;
}