using System.Collections;
using Project.Scripts;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;
    public static GameMaster Instance => _instance;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private MainConfig _mainConfig;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private RectTransform _canvasRoot;
    
    public static Camera MainCamera => Instance._mainCamera;
    public static MainConfig MainConfig => _instance._mainConfig;
    public static Spawner Spawner => _instance._spawner;
    private bool _isGameLoaded;
    public static bool IsGameLoaded => Instance == null ? false : Instance._isGameLoaded;
    
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
        _save = ReadSaveFromPlayerPrefs();
    }

    private IEnumerator LoadBoardGame()
    {
        var currentLevel = MainConfig.GameplayConfig.DemoLevel;
        var boardConfigId = currentLevel.BoardTemplate;
        var boardConfig = MainConfig.BoardsConfig.GetBoardConfig(boardConfigId);

        var uiBoardGame = Spawner.Spawn<UIBoardGameBehaviour>(boardConfig.UIBoardGamePrefab,_canvasRoot);
        _boardGameBehaviour = uiBoardGame;
        
        _boardDiceGame = new BoardDiceGame(boardConfig, _save);
        _boardDiceGame.Initialize();
        
        _boardDiceGameBehaviour = Spawner.Spawn<BoardDiceGameBehaviour>(boardConfig.BoardPrefab);
        yield return _boardDiceGameBehaviour.Setup(_boardDiceGame);
    }

    private void Unload()
    {
        WriteSaveToPlayerPrefs(_save);
        
        _boardDiceGame.Dispose();
        _boardDiceGame = null;
        
        _boardDiceGameBehaviour.Unload();
        Destroy(_boardDiceGameBehaviour);
        _boardDiceGameBehaviour = null;
    }

    private void WriteSaveToPlayerPrefs(BoardDiceGameSave save)
    {
        //only works in editor
        PlayerPrefs.SetString("Save",JsonUtility.ToJson(save));
        PlayerPrefs.Save();
    }

    private BoardDiceGameSave ReadSaveFromPlayerPrefs()
    {
        var save = JsonUtility.FromJson<BoardDiceGameSave>(PlayerPrefs.GetString("Save",JsonUtility.ToJson(new BoardDiceGameSave())));
        return save;
    }
}
