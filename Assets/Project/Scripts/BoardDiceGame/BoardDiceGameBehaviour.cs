using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DiceGameState
{
    Unknown = 0,
    PlayerInput = 1,
    DiceRoll = 2,
    Result = 3,
}

public class BoardDiceGameBehaviour : MonoBehaviour
{
    [ReadOnly] public DesktopBehaviour Desktop;
    [ReadOnly] public DiceBehaviour DiceBehaviour;
    [ReadOnly] public InputBehaviour Input;
    [ReadOnly] public HandBehaviour Hand;
    [ReadOnly] public PointerBehaviour Pointer;
    
    public Transform DiceStartPosition => Desktop.DiceResetPosition;
    private BoardBarrier BoardBarrier => Desktop.BoardBarrier;
    private DiceTriggerBoardBarrier DiceTriggerBoardBarrier => Desktop.DiceTriggerBoardBarrier;
    
    [ReadOnly] public DiceGameState State;
    [SerializeField] private double _rollTimeLimit;
    [SerializeField] private float _pauseDurationAfterResultAnnounced;
    private float _starDiceRollTimestmap;
    private float _diceResultCapturedTimestamp;
    
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
        Initialize();
    }

    private IEnumerator SpawnObjects(BoardDiceGame boardDiceGame)
    {
        DiceBehaviour = GameMaster.Spawner.Spawn<DiceBehaviour>(boardDiceGame.BoardConfig.DicePrefab);
        Input = GameMaster.Spawner.Spawn<InputBehaviour>(boardDiceGame.BoardConfig.InputPrefab);
        Hand = GameMaster.Spawner.Spawn<HandBehaviour>(boardDiceGame.BoardConfig.HandPrefab);
        Pointer = GameMaster.Spawner.Spawn<PointerBehaviour>(boardDiceGame.BoardConfig.PointerPrefab);
        Desktop = GameMaster.Spawner.Spawn<DesktopBehaviour>(boardDiceGame.BoardConfig.DesktopPrefab);
        yield return null;
    }
    
    private void Initialize()
    {
        Input.Initialize();
        
        DiceTriggerBoardBarrier.OnDiceInsidePlayground += OnDiceInsidePlayground;
        DiceBehaviour.OnDiceResultCaptured += OnDiceResultCaptured;
        DiceBehaviour.OnDiceFailedToCaptureResult += OnDiceFailedToCaptureResult;

        ResetDice();
    }
    
    public void Unload()
    {
        DiceTriggerBoardBarrier.OnDiceInsidePlayground -= OnDiceInsidePlayground;
        DiceBehaviour.OnDiceResultCaptured -= OnDiceResultCaptured;
        DiceBehaviour.OnDiceFailedToCaptureResult -= OnDiceFailedToCaptureResult;
    }
    
    public void OnDiceInsidePlayground()
    {
        State = DiceGameState.DiceRoll;
        _starDiceRollTimestmap = Time.time;

        DiceBehaviour.StartListenForResult();
        BoardBarrier.ChangeBarrierState(true);
    }
    
    private void OnDiceFailedToCaptureResult()
    {
        DiceBehaviour.StopListenForResult();
        ResetDice();
    }

    private void OnDiceResultCaptured(int obj)
    {
        _diceResultCapturedTimestamp = Time.time;
        DiceBehaviour.StopListenForResult();
        DiceBehaviour.HighlightResult(_pauseDurationAfterResultAnnounced);

        State = DiceGameState.Result;
    }
    
    private void Update()
    {
        switch (State)
        {
            case DiceGameState.Unknown: break;
            case DiceGameState.PlayerInput: break;
            case DiceGameState.DiceRoll:
            {
                var isTimeLimitPassed = Time.time - _starDiceRollTimestmap > _rollTimeLimit;
                if (isTimeLimitPassed == false)
                {
                    return;
                }

                OnDiceFailedToCaptureResult();
                break;
            }
            case DiceGameState.Result:
                var isPasueTimestampPassed = Time.time - _diceResultCapturedTimestamp > _pauseDurationAfterResultAnnounced;
                if (isPasueTimestampPassed == false)
                {
                    return;
                }
                
                ResetDice();
                break;
        }
    }
    
    [Button]
    public void ResetDice()
    {
        Input.ResetDice();
        BoardBarrier.ChangeBarrierState(false);

        State = DiceGameState.PlayerInput;
    }
}