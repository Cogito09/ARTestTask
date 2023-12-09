using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BoardsConfig", menuName = "Configs/BoardsConfig", order = 0)]
public class BoardsConfig : ScriptableObject
{
    public List<BoardConfig> Boards;

    public BoardConfig GetBoardConfig(int id)
    {
        for (var i = 0; i < Boards.Count; i++)
        {
            if (Boards[i].Id != id)
            {
                continue;
            }

            return Boards[i];
        }

        Debug.LogError($"Could not find board config of Id {id}!");
        return null;
    }
}