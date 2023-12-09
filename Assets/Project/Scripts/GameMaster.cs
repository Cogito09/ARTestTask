using System.Collections;
using Project.Scripts;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;
    public static GameMaster Instance => _instance;

    private bool _isGameLoaded;
    private bool IsGameLoaded => Instance == null ? false : Instance._isGameLoaded;
    [SerializeField] private MainConfig _mainConfig;
    public static MainConfig MainConfig => _instance._mainConfig;
    

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {

        yield return LoadBoardGame();
        _isGameLoaded = true;
    }

    private IEnumerator LoadBoardGame()
    {
        //var boardGameConfig = MainConfig.BoardGameConfig;
        yield return null;
    }
}
