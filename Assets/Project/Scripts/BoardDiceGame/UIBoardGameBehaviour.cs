using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIBoardGameBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("_totalResult")] [SerializeField] private TextMeshProUGUI _totalScore;
    [FormerlySerializedAs("_result")] [SerializeField] private TextMeshProUGUI _lastResult;

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
    }

    private void OnRoll()
    {
        _lastResult.text = "?";
    }

    public void OnClickRoll()
    {
        GameMaster.CurrentActiveBoardDiceGameBehaviour.RandomRoll();
    }
}