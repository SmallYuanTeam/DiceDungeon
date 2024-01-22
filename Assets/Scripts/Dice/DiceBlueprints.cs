using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public enum DiceRarity
    {
        Basic,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    public enum DiceType
    {
        Attack,         // 進攻
        Defense,        // 格檔
        Rest,           // 休整
        Utility         // 特效
    }
    public enum DiceAbility
    {
        Attack,         // 攻擊
        Defense,        // 防禦
        Heal,           // 回血
        Increace,       // 亢奮,增傷
        Decrease,       // 堅強,減傷
        Poison,         // 中毒
        Dizzy,          // 暈眩
        Burn,           // 燃燒
        Pierce,         // 貫穿
        Bloodthirsty,   // 嗜血
        Restrict,       // 封印
        Paralysis,      // 麻痺
        Freeze,         // 冰凍
        Wound,          // 創傷
        Rebellion,      // 盾反
        Thorns,         // 荊棘
        Psychedelic,    // 迷幻
        Angry,          // 憤怒
        Purify,         // 淨化
        Pray,           // 祈禱
        Curse,          // 詛咒
        Utility
    }
    public enum DiceTarget
    {
        Self,
        Enemy,
        All
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
        public List<DiceAbility> diceAbilities;
        public List<DiceTarget> diceTargets;
    }
}