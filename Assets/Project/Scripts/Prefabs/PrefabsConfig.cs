using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Configs
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Configs/PrefabsConfig", order = 0)]
    public class PrefabsConfig : ScriptableObject
    {
        public List<Prefab> Prefabs;

        public Prefab GetPrefab(int id)
        {
            for (var i = 0; i < Prefabs.Count; i++)
            {
                if (Prefabs[i].Id != id)
                {
                    continue;
                }

                return Prefabs[i];
            }

            Debug.LogError($"Could not find prefab of id {id}!");
            return null;
        }
    }

    [Serializable]
    public class Prefab
    {
        public int Id;
        public string DevName;
        public GameObject Object;
    }
}