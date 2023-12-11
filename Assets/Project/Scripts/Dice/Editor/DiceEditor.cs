#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiceBehaviour))]
public class MyCustomEditor : UnityEditor.Editor 
{
    
    void OnSceneGUI() {
        Event e = Event.current;
        switch (e.type) {
            case EventType.KeyDown:
                if (e.alt)
                {
                        
                }
                    
                break;
        }
    }
}

#endif