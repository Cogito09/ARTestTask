using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Configs/GameplayConfig", order = 0)]
public class GameplayConfig : ScriptableObject
{
    [Board] public int BoardToBePlayed;
}
