using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Configs/GameplayConfig", order = 0)]
public class GameplayConfig : ScriptableObject
{
    public LevelEntry DemoLevel;
}

[Serializable]
public class LevelEntry
{
    [Board] public int BoardTemplate;
}