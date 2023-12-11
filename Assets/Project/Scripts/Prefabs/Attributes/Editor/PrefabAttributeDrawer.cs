using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class PrefabAttributeDrawer : OdinAttributeDrawer<PrefabAttribute,int>
{
    private Dictionary<int, string> _content = new Dictionary<int, string>();
    private List<int> _ids;

    public override bool CanDrawTypeFilter(Type type)
    {
        return type == typeof(int);
    }
    
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var prefabs = MainConfig.Prefabs.Prefabs;
        if (prefabs == null)
        {
            Debug.LogError($" Prefabs list is null.");
            return;
        }
        
        _content.Clear();
        _content.Add(0, "Undefined");
         _ids = new List<int> {0};
        
        for (var i = 0; i < prefabs.Count; i++)
        {
            var contentEntry = prefabs[i];
            _content.Add(contentEntry.Id, $"{contentEntry.Id}.{contentEntry.DevName}");
            _ids.Add(contentEntry.Id);
        }
        
       
        var values = _content.Values.ToArray();
        var indexOfCurrentItem = _ids.IndexOf( ValueEntry.SmartValue);
        var index = EditorGUILayout.Popup(label, indexOfCurrentItem, values);

        // Debug.Log($"Index : {index}");
        if(index < 0)
        {
            return;
        }
        
        ValueEntry.SmartValue = _ids[index];
    }
}