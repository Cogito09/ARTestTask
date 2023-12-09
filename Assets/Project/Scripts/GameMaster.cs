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
    public static MainConfig MainConfig => _instance._mainConfig;
    public static Spawner Spawner => _instance._spawner;
    private bool _isGameLoaded;
    private bool IsGameLoaded => Instance == null ? false : Instance._isGameLoaded;

    private BoardDiceGameSave _save;
    private BoardDiceGame _boardDiceGame;
    private BoardDiceGameBehaviour _boardDiceGameBehaviour;
    
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
        _save = new BoardDiceGameSave();
    }

    private IEnumerator LoadBoardGame()
    {
        var currentLevel = MainConfig.GameplayConfig.DemoLevel;
        var boardConfigId = currentLevel.BoardTemplate;
        var boardConfig = MainConfig.BoardsConfig.GetBoardConfig(boardConfigId);

        _boardDiceGame = new BoardDiceGame(boardConfig, _save);
        _boardDiceGame.Initialize();
        
        _boardDiceGameBehaviour = Spawner.Spawn<BoardDiceGameBehaviour>(boardConfig.BoardPrefab);
        _boardDiceGameBehaviour.Setup(_boardDiceGame);
        yield return null;
    }
}
