using System;
using UnityEngine;

public class BoardDiceGameBehaviour : MonoBehaviour
{
    [Prefab] public int BoardPrefab;


    public void Setup(BoardDiceGame boardDiceGame)
    {
        SetupDice(boardDiceGame);
    }

    private void SetupDice(BoardDiceGame boardDiceGame)
    {
    }
}