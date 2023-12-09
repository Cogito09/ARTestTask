using Project.Configs;
using UnityEngine;

namespace Project.Scripts
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [SerializeField] private PrefabsConfig PrefabsConfig;
        
        private static MainConfig _mainConfig;
        public static MainConfig Instance =>
#if !UNITY_EDITOR
             _mainConfig ?? (_mainConfig = GameMaster.MainConfig);
#endif
#if UNITY_EDITOR
            _mainConfig ? _mainConfig : Application.isEditor ? (_mainConfig = GetConfigAsset()) : GameMaster.MainConfig;
        
        private static MainConfig GetConfigAsset()
        {
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<MainConfig>("Assets/Project/Configs/MainConfig.asset");
            return asset;
        }
#endif
    }
}