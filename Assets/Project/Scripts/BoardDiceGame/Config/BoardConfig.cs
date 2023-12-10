using System;

[Serializable]
public class BoardConfig
{
    public int Id;
    public string DevName;
    
    [Prefab] public int BoardPrefab;
    [Prefab] public int DicePrefab;
    [Prefab] public int DesktopPrefab;
    [Prefab] public int PointerPrefab;
    [Prefab] public int HandPrefab;
    [Prefab] public int InputPrefab;
}