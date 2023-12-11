using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DiceConfig
{
    public int Id;
    public string DevName;
    public int NumberOfFaces;
    [PreviewField]public GameObject DicePrefab;
    [ValidateInput("IsShowingRightNumberOfFaces", "Faces configs count doesnt match NumberOfFaces!", InfoMessageType.Error)]
    [ValidateInput("IsShowingRightNumberOfFacesGreenLight", "Faces configs count is OK!", InfoMessageType.Info)]
    public List<DiceFaceConfig> Faces;

    private bool IsShowingRightNumberOfFaces(List<DiceFaceConfig> Faces)
    {
        return Faces.Count == NumberOfFaces;
    }
    
    private bool IsShowingRightNumberOfFacesGreenLight(List<DiceFaceConfig> Faces)
    {
        return Faces.Count != NumberOfFaces;
    }
    
    public void OnValidate()
    {
        for (var i = 0; i < Faces.Count; i++)
        {
            var face = Faces[i];
            face.FaceIndex = i + 1;
            Faces[i] = face;
        }
    }
}