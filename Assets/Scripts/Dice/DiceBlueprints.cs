using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public enum DiceRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    public enum DiceType
    {
        Attack,
        Defense,
        Heal,
        Increace,
        Decrease,
        Poison,
        Burn,
        Dizzy,
        Utility
    }
}

namespace Dice
{
    [CreateAssetMenu]
    public class DiceBlueprints : ScriptableObject
    {
        public DiceRarity diceRarity;
        public int ID;
        public Sprite sprite;
        public List<int> diceValues;
        public List<DiceType> diceTypes;
    }
}