using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoardDiceGameBehaviour : MonoBehaviour
{
    [ReadOnly] public DesktopBehaviour Desktop;
    [ReadOnly] public DiceBehaviour DiceBehaviour;
    [ReadOnly] public InputBehaviour Input;
    [ReadOnly] public HandBehaviour Hand;
    [ReadOnly] public PointerBehaviour Pointer;
    
    public Transform DiceStartPosition => Desktop.DiceResetPosition;
    
    public IEnumerator Setup(BoardDiceGame boardDiceGame)
    {
        if (boardDiceGame == null)
        {
            Debug.LogError($"BoardDiceGame object is null");
            yield break;
        }

        if (boardDiceGame.BoardConfig == null)
        {
            Debug.LogError($"BoardDiceGameConfig is null");
            yield break;
        }

        yield return SpawnObjects(boardDiceGame);
        InitGame();
    }
    
    private IEnumerator SpawnObjects(BoardDiceGame boardDiceGame)
    {
        DiceBehaviour = GameMaster.Spawner.Spawn<DiceBehaviour>(boardDiceGame.BoardConfig.DicePrefab);
        Input = GameMaster.Spawner.Spawn<InputBehaviour>(boardDiceGame.BoardConfig.InputPrefab);
        Hand = GameMaster.Spawner.Spawn<HandBehaviour>(boardDiceGame.BoardConfig.HandPrefab);
        Pointer = GameMaster.Spawner.Spawn<PointerBehaviour>(boardDiceGame.BoardConfig.PointerPrefab);
        Desktop = GameMaster.Spawner.Spawn<DesktopBehaviour>(boardDiceGame.BoardConfig.DesktopPrefab);

        Input.Initialize();
        yield return null;
    }
    
    private void InitGame()
    {

    }
}