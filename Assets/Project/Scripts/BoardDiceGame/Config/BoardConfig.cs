using System;

[Serializable]
public class BoardConfig
{
    public int Id;
    public string DevName;
    
    [Prefab] public int BoardPrefab;
    [Prefab] public int DicePrefab;
    [Prefab] public int DesktopPrefab;
}