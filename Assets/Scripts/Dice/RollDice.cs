using System.Collections;
using System.Collections.Generic;
using Dice;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public List<DiceBlueprints> diceBlueprints;
    
    
    // Press Button to Roll Dice
    public void Roll()
    {
        
        // Roll Dice
        foreach (var diceBlueprint in diceBlueprints)
        {
            // Get Dice ListCount
            int _DiceListCount = diceBlueprint.diceValues.Count;
            // Get Random Dice Value
            int _DiceValue = Random.Range(0, _DiceListCount);
            // Get Random Dice Value
            var diceID = diceBlueprint.diceID; // Get Which Dice to Roll
            var diceName = diceBlueprint.diceName;
            var randomDiceValue = diceBlueprint.diceValues[_DiceValue];
            var randomDiceType = diceBlueprint.diceAbilities[_DiceValue];
            var randomDiceTarget = diceBlueprint.diceTargets[_DiceValue];
            Debug.Log(diceID + " " + randomDiceType + " " + randomDiceValue + " " + randomDiceTarget);
        }
    }
    
}
