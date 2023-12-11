using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class DiceConfig
{
    public int Id;
    public int NumberOfFaces;
    [Prefab] public int PrefabId;
    public List<DiceFaceConfig> Faces;

    private bool IsRightNumberOfFaces;

    [ShowIf("IsRightNumberOfFaces")] 
    public string NotRightNumberOfFaces = $"Not right number of faces";

    
    public void OnValidate()
    {
        CheckForRightNumberOfFaces();

    }

    private bool CheckForRightNumberOfFaces()
    {
        if (Faces == null && NumberOfFaces == 0)
        {
            return true;
        }

        return NumberOfFaces == Faces.Count;
    }
}