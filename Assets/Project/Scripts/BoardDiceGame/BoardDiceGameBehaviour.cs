﻿using System.Collections;
using Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum DiceGameState
{
    Unknown = 0,
    PlayerInput = 1,
    DiceRoll = 2,
    Result = 3,
}

public class BoardDiceGameBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("Desktop")] [ReadOnly] public BoardDesktopBehaviour boardDesktop;
    [ReadOnly] public DiceBehaviour DiceBehaviour;
    [ReadOnly] public InputBehaviour Input;
    [ReadOnly] public HandBehaviour Hand;
    [ReadOnly] public PointerBehaviour Pointer;
    
    public Transform DiceStartPosition => boardDesktop.DiceResetPosition;
    //private BoardBarrier BoardBarrier => boardDesktop.BoardBarrier;
    //private BoardDiceTriggerAreaBehaviour BoardDiceTriggerAreaBehaviour => boardDesktop.boardDiceTriggerAreaBehaviour;
    
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

    private void SpawnDice(BoardDiceGame boardDiceGame)
    {
        var diceConfigId = boardDiceGame.BoardConfig.Dice;
        var diceConfig = MainConfig.DicesConfig.GetConfig(diceConfigId);
       DiceBehaviour = GameMaster.Spawner.Spawn<DiceBehaviour>(diceConfig.DicePrefab);
       DiceBehaviour.Setup(diceConfig);
    }

    private IEnumerator SpawnObjects(BoardDiceGame boardDiceGame)
    {
        Input = GameMaster.Spawner.Spawn<InputBehaviour>(boardDiceGame.BoardConfig.InputPrefab);
        Hand = GameMaster.Spawner.Spawn<HandBehaviour>(boardDiceGame.BoardConfig.HandPrefab);
        Pointer = GameMaster.Spawner.Spawn<PointerBehaviour>(boardDiceGame.BoardConfig.PointerPrefab);
        boardDesktop = GameMaster.Spawner.Spawn<BoardDesktopBehaviour>(boardDiceGame.BoardConfig.DesktopPrefab);
        SpawnDice(boardDiceGame);
        yield return null;
    }
    
    private void Initialize()
    {
        Input.Initialize();
        
        //BoardDiceTriggerAreaBehaviour.OnDiceInsidePlayground += OnDiceInsidePlayground;
        DiceBehaviour.OnDiceResultCaptured += OnDiceResultCaptured;
        DiceBehaviour.OnDiceFailedToCaptureResult += OnDiceFailedToCaptureResult;

        ResetDice();
    }
    
    public void Unload()
    {
        State = DiceGameState.Unknown;
        
        //BoardDiceTriggerAreaBehaviour.OnDiceInsidePlayground -= OnDiceInsidePlayground;
        DiceBehaviour.OnDiceResultCaptured -= OnDiceResultCaptured;
        DiceBehaviour.OnDiceFailedToCaptureResult -= OnDiceFailedToCaptureResult;

        Destroy(DiceBehaviour.gameObject);
        Destroy(Input.gameObject);
        Destroy(Hand.gameObject);
        Destroy(Pointer.gameObject);
        Destroy(boardDesktop.gameObject);
    }

    public void RandomRoll()
    {
        if (State != DiceGameState.PlayerInput)
        {
            return;
        }
        
        Input.RandomThrow();
        OnDiceInsidePlayground();
    }

    public void OnDiceInsidePlayground()
    {
        State = DiceGameState.DiceRoll;
        _starDiceRollTimestmap = Time.time;

        DiceBehaviour.StartListenForResult();
        //BoardBarrier.ChangeBarrierState(true);
    }
    
    private void OnDiceFailedToCaptureResult()
    {
        DiceBehaviour.StopListenForResult();
        ResetDice();
    }

    private void OnDiceResultCaptured(int score)
    {
        _diceResultCapturedTimestamp = Time.time;
        DiceBehaviour.StopListenForResult();
        DiceBehaviour.HighlightResult(_pauseDurationAfterResultAnnounced);
        GameMaster.CurrentBoardDiceGameLogic.AddScore(score); 
        
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
        //BoardBarrier.ChangeBarrierState(false);

        State = DiceGameState.PlayerInput;
    }
}