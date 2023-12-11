using Project.Scripts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public T Spawn<T>(int prefabId,Transform parent = null) where T : MonoBehaviour
    {
        var prefab = MainConfig.Prefabs.GetPrefab(prefabId);
        if (prefab?.Object == null)
        {
            Debug.LogError($"prefab of id{prefabId} is null.");
            return null;
        }
        
        var spawnedObject = Instantiate(prefab.Object, parent);
        if (spawnedObject == null)
        {
            Debug.LogError($"Somehow spawned prefab of id{prefabId} is null.");
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
    
    public T Spawn<T>(GameObject template,Transform parent = null) where T : MonoBehaviour
    {
        if (template == null)
        {
            Debug.LogError($"given template is null.");
            return null;
        }
        
        var spawnedObject = Instantiate(template, parent);
        if (spawnedObject == null)
        {
            Debug.LogError($"Somehow spawned prefab of from template {template.name} is null.");
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
    
    public GameObject Spawn(int prefabId,Transform parent = null)
    {
        var prefab = MainConfig.Prefabs.GetPrefab(prefabId);
        if (prefab?.Object == null)
        {
            Debug.LogError($"prefab of id{prefabId} is null.");
            return null;
        }
        
        var spawnedObject = Instantiate(prefab.Object, parent);
        if (spawnedObject == null)
        {
            Debug.LogError($"Somehow spawned prefab of id{prefabId} is null.");
        }
        
        return spawnedObject;
    }

    public void SpawnAtPosition(int prefabId, Vector3 position)
    {
        var obj = Spawn(prefabId);
        if (obj == null)
        {
            return;
        }

        obj.transform.position = position;
    }
}