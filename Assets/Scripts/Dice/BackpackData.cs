using Dice;
using System.Collections.Generic;

[System.Serializable]
public class BackpackData
{
    public List<DiceBlueprints> diceBlueprintsList;

    public BackpackData(List<DiceBlueprints> diceBlueprints)
    {
        diceBlueprintsList = diceBlueprints;
    }
}