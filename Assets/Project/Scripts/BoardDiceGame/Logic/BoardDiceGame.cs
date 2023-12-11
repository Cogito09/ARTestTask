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
    
    public void Dispose()
    {
    }

    public void Initialize()
    {

    }
}