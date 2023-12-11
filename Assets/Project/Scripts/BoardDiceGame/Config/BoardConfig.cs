using System;
using UnityEngine.Serialization;

[Serializable]
public class BoardConfig
{
    public int Id;
    public string DevName;
    
    [Prefab] public int BoardPrefab;
    [FormerlySerializedAs("DicePrefab")] [Dice] public int Dice;
    [Prefab] public int DesktopPrefab;
    [Prefab] public int PointerPrefab;
    [Prefab] public int HandPrefab;
    [Prefab] public int InputPrefab;
    [Prefab] public int UIBoardGamePrefab;
    [Prefab] public int CamerasSetupPrefab;
}