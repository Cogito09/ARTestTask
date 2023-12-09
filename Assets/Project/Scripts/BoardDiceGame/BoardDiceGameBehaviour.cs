using UnityEngine;

public class BoardDiceGameBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _desktop;
    [SerializeField] private DiceBehaviour _diceBehaviour;
    [SerializeField] private InputBehaviour _hand;
    

    public void Setup(BoardDiceGame boardDiceGame)
    {
        if (boardDiceGame == null)
        {
            Debug.LogError($"BoardDiceGame object is null");
            return;
        }

        if (boardDiceGame.BoardConfig == null)
        {
            Debug.LogError($"BoardDiceGameConfig is null");
            return;
        }

        SpawnObjects(boardDiceGame);
        InitGame();
    }
    
    private void SpawnObjects(BoardDiceGame boardDiceGame)
    {
        _diceBehaviour = GameMaster.Spawner.Spawn<DiceBehaviour>(boardDiceGame.BoardConfig.DicePrefab);
        _hand = GameMaster.Spawner.Spawn<InputBehaviour>(boardDiceGame.BoardConfig.PointerPrefab);
        _desktop = GameMaster.Spawner.Spawn(boardDiceGame.BoardConfig.DesktopPrefab);
    }
    
    private void InitGame()
    {

    }
}