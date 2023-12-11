using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DicesConfig", menuName = "Configs/DicesConfig", order = 0)]
public class DicesConfig : ScriptableObject
{
    [InfoBox("Guide")]
    [InfoBox("To create new dice, please create new Dice prefab, you can use Dice_Template.prefab for reference. Add mesh, generate collider, and follow steps for easy face placement")]
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

    public void OnValidate()
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