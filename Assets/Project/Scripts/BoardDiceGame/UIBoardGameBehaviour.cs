using System.Collections;
using TMPro;
using UnityEngine;

public class UIBoardGameBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalScore;
    [SerializeField] private TextMeshProUGUI _lastResult;
    [SerializeField] private MMUIShaker _totalScoreShaker; 
    [SerializeField] private MMUIShaker _lastResultShaker;
    [SerializeField] private MMUIShaker _buttonShaker;
    [SerializeField] private float _udpateShakeDuration = 0.4f;
    [SerializeField] private float _buttonClickShakeDuration = 0.2f;

    private BoardDiceGame BoardDiceGame => GameMaster.CurrentBoardDiceGameLogic;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameMaster.IsGameLoaded);
        EventManager.OnRoll += OnRoll;
        EventManager.OnScoreUpdated += OnScoreUpdated;
        OnScoreUpdated();
    }
    
    private void OnDestroy()
    {
        EventManager.OnRoll -= OnRoll;
        EventManager.OnScoreUpdated -= OnScoreUpdated;
    }

    private void OnScoreUpdated()
    {
        var lastResult = BoardDiceGame?.LastResult.ToString() ?? "0";
        _lastResult.text = $"Result: {lastResult}";
        var totalScore = BoardDiceGame?.TotalScore.ToString() ?? "0";;
        _totalScore.text = $"Total: {totalScore}";

        Shake();
    }

    private void Shake()
    {
        StartCoroutine(_totalScoreShaker.Shake(_udpateShakeDuration));
        StartCoroutine(_lastResultShaker.Shake(_udpateShakeDuration));
    }
    
    private void OnRoll()
    {
        _lastResult.text = "?";
    }

    public void OnClickRoll()
    {
        if (GameMaster.CurrentActiveBoardDiceGameBehaviour.State != DiceGameState.PlayerInput)
        {
            return;
        }
        
        GameMaster.CurrentActiveBoardDiceGameBehaviour.RandomRoll();
        StartCoroutine(_buttonShaker.Shake(_buttonClickShakeDuration));
    }

    public void Unload()
    {
    }
}