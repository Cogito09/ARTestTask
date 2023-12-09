using Project.Scripts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public T Spawn<T>(int boardConfigDicePrefab,Transform parent = null) where T : MonoBehaviour
    {
        var prefab = MainConfig.Prefabs.GetPrefab(boardConfigDicePrefab);
        if (prefab?.Object == null)
        {
            Debug.LogError($"prefab of id{boardConfigDicePrefab} is null.");
            return null;
        }
        
        var spawnedObject = Instantiate(prefab.Object, parent);
        if (spawnedObject == null)
        {
            Debug.LogError($"Somehow spawned prefab of id{boardConfigDicePrefab} is null.");
            return null;
        }

        var script = spawnedObject.GetComponent<T>();
        if (script == null)
        {
            Debug.LogError($"Could not find attached Monobehaviour script!");
            return null;
        }
        
        return script;
    }
    
    public GameObject Spawn(int boardConfigDicePrefab,Transform parent = null)
    {
        var prefab = MainConfig.Prefabs.GetPrefab(boardConfigDicePrefab);
        if (prefab?.Object == null)
        {
            Debug.LogError($"prefab of id{boardConfigDicePrefab} is null.");
            return null;
        }
        
        var spawnedObject = Instantiate(prefab.Object, parent);
        if (spawnedObject == null)
        {
            Debug.LogError($"Somehow spawned prefab of id{boardConfigDicePrefab} is null.");
        }
        
        return spawnedObject;
    }
}