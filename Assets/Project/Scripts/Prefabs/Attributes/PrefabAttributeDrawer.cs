using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

using UnityEngine;

public class PrefabAttributeDrawer : OdinAttributeDrawer<PrefabAttribute,int>
{
    private Dictionary<int, string> _content = new Dictionary<int, string>();
    private List<int> _ids;

    protected override void DrawPropertyLayout(IPropertyValueEntry<int> entry, PrefabAttribute attribute, GUIContent label)
    {
        Debug.Log($"Try to draw property");
        
        
        base.DrawPropertyLayout(entry, attribute, label);
    }

    // public override bool CanDrawTypeFilter(Type type)
    // {
    //     return type == typeof(int);
    // }
    //
    // protected override void DrawPropertyLayout(IPropertyValueEntry<int> entry, PrefabAttribute attribute, GUIContent label)
    // {
    //     var prefabs = MainConfig.Prefabs.Prefabs;
    //     if (prefabs == null)
    //     {
    //         Debug.LogError($" Prefabs list is null.");
    //         return;
    //     }
    //     
    //     _content.Clear();
    //     _ids = new List<int>();
    //     
    //     for (var i = 0; i < prefabs.Count; i++)
    //     {
    //         var contentEntry = prefabs[i];
    //         _content.Add(contentEntry.Id, $"{contentEntry.Id}.{contentEntry.DevName}");
    //     }
    //     
    //     var values = _content.Values.ToArray();
    //     var indexOfCurrentItem = _ids.IndexOf(entry.SmartValue);
    //     var index = EditorGUILayout.Popup(label, indexOfCurrentItem, values);
    //     entry.SmartValue = _ids[index];
    // }
}