using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;
    public static GameMaster Instance => _instance;
    
    private bool _isGameLoaded;
    public static bool IsGameLoaded => Instance == null ? false : Instance._isGameLoaded;
    
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private MainConfig _mainConfig;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private RectTransform _canvasRoot;
    
    public static MainConfig MainConfig => _instance._mainConfig;
    public static Spawner Spawner => _instance._spawner;
    public static Camera MainCamera => Instance._mainCamera;
    public static CinemachineBrain MainCameraBrain => _instance._cinemachineBrain;
    public static BoardDiceGameBehaviour CurrentActiveBoardDiceGameBehaviour => Instance._boardDiceGameBehaviour;
    public static BoardDiceGame CurrentBoardDiceGameLogic => Instance._boardDiceGame;
    
    private UIBoardGameBehaviour _boardGameBehaviour;
    private BoardDiceGameBehaviour _boardDiceGameBehaviour;
    private BoardDiceGameSave _save;
    private BoardDiceGame _boardDiceGame;

    private void Awake()
    {
        _loadingScreen.gameObject.SetActive(true);
        _instance = this;
    }

    private IEnumerator Start()
    {
        yield return StartLoading();
    }

    private IEnumerator StartLoading()
    {
        yield return Load();
        _loadingScreen.gameObject.SetActive(false);    
    }

    private IEnumerator Load()
    {
        yield return LoadSave();
        yield return LoadBoardGame();

        _isGameLoaded = true;
    }

    private IEnumerator LoadSave()
    {
        yield return null;
        _save = GetSave();
    }

    private IEnumerator LoadBoardGame()
    {
        var boardConfigId = MainConfig.GameplayConfig.BoardToBePlayed;
        var boardConfig = MainConfig.BoardsConfig.GetBoardConfig(boardConfigId);
        
        _boardGameBehaviour = Spawner.Spawn<UIBoardGameBehaviour>(boardConfig.UIBoardGamePrefab,_canvasRoot);
        _boardDiceGame = new BoardDiceGame(boardConfig, _save);
        _boardDiceGameBehaviour = Spawner.Spawn<BoardDiceGameBehaviour>(boardConfig.BoardPrefab);
        yield return _boardDiceGameBehaviour.Setup(_boardDiceGame);
    }

    [Button]
    public void Unload()
    {
        _boardDiceGame?.Dispose();
        _boardDiceGame = null;

        if (_boardGameBehaviour != null)
        {
            _boardGameBehaviour.Unload();
            Destroy(_boardGameBehaviour.gameObject);
            _boardGameBehaviour = null;
        }
        
        _boardDiceGameBehaviour.Unload();
        Destroy(_boardDiceGameBehaviour.gameObject);
        _boardDiceGameBehaviour = null;
    }

    [Button]
    public void LoadAgain()
    {
        StartCoroutine(StartLoading());
    }
    
    private BoardDiceGameSave GetSave()
    {
        var save = new BoardDiceGameSave();
        return save;
    }
}
