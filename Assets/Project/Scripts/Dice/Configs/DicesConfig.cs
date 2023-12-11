using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DicesConfig", menuName = "Configs/DicesConfig", order = 0)]
public class DicesConfig : ScriptableObject
{
    public List<DiceConfig> Dices;

    public DiceConfig GetConfig(int diceConfigId)
    {
        for (var i = 0; i < Dices.Count; i++)
        {
            if (Dices[i].Id != diceConfigId)
            {
                continue;
            }

            return Dices[i];
        }
            
        Debug.LogError($"Dice cofnig of id {diceConfigId} is not present!");
        return null;
    }

    private void OnValidate()
    {
        for (var i = 0; i < Dices.Count; i++)
        {
            var diceConfig = Dices[i];
            if (diceConfig == null)
            {
                continue;
            }

            diceConfig.OnValidate();
        }
    }
}