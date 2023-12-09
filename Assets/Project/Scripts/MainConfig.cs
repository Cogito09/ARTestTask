using Project.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [FormerlySerializedAs("PrefabsConfig")] [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private BoardsConfig _boardsConfig;
        [SerializeField] private GameplayConfig _gameplayConfig;
        public static PrefabsConfig Prefabs => Instance._prefabsConfig;
        public static BoardsConfig BoardsConfig => Instance._boardsConfig;
        public static GameplayConfig GameplayConfig => Instance._gameplayConfig;
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