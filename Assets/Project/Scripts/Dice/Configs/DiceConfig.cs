using System;
using System.Collections.Generic;
using Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DiceConfig
{
    public int Id;
    public string DevName;
    public int NumberOfFaces;
    [Prefab] public int PrefabId;
    [ReadOnly][ShowInInspector][PreviewField][System.NonSerialized] public GameObject PrefabRefForEasyAccess;
    [ValidateInput("IsShowingRightNumberOfFaces", "Faces configs count doesnt match NumberOfFaces!", InfoMessageType.Error)]
    public List<DiceFaceConfig> Faces;

    private bool IsShowingRightNumberOfFaces(List<DiceFaceConfig> Faces)
    {
        return Faces.Count == NumberOfFaces;
    }
    

    public void OnValidate()
    {
        for (var i = 0; i < Faces.Count; i++)
        {
            Faces[i].FaceIndex = i + 1;
        }

        PrefabRefForEasyAccess = MainConfig.Prefabs.GetPrefab(PrefabId)?.Object;
    }
}