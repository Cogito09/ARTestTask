using System;

public class BoardDiceGame : IDisposable
{
    private BoardDiceGameSave _save;
    public BoardConfig BoardConfig;
    
    public BoardDiceGame(BoardConfig boardConfig, BoardDiceGameSave boardDiceGameSave)
    {
        _save = boardDiceGameSave;
        BoardConfig = boardConfig;
    }

    public int LastResult
    {
        get => _save.LastResult;
        set => _save.LastResult = value;
    }

    public int TotalScore
    {
        get => _save.TotalScore;
        set => _save.TotalScore = value;
    }

    public void AddScore(int score)
    {
        LastResult = score;
        TotalScore += score;
        
        EventManager.OnScoreUpdated?.Invoke();
    }
    
    public void Dispose()
    {
        BoardConfig = null;
        _save = null;
    }
}