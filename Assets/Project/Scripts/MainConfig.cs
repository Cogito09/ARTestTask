using Project.Configs;
using UnityEngine;

namespace Project.Scripts
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [SerializeField] private PrefabsConfig PrefabsConfig;
        public static PrefabsConfig Prefabs => _instance.PrefabsConfig;
        private static MainConfig _instance;
        public static MainConfig Instance =>
#if !UNITY_EDITOR
             _instance ?? (_instance = GameMaster.MainConfig);
#endif
#if UNITY_EDITOR
            _instance ? _instance : Application.isEditor ? (_instance = GetConfigAsset()) : GameMaster.MainConfig;



        private static MainConfig GetConfigAsset()
        {
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<MainConfig>("Assets/Project/Configs/MainConfig.asset");
            return asset;
        }
#endif
    }
}