using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Dice;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public DiceBackpack diceBackpack;
    public List<DiceBlueprints> DicePool;
    public List<DiceBlueprints> DiscardDicePool;
    public List<DiceBlueprints> Hand;

    // Start is called before the first frame update
    void Start()
    {
        diceBackpack = FindObjectOfType<DiceBackpack>();
        
    }
    //進入戰鬥時初始化
    public void StartInitlizeBattle()
    {
        if (diceBackpack == null)
        {
            Debug.LogError("DiceBackpack not found");
        }
        else
        {
            DicePool = new List<DiceBlueprints>(diceBackpack.GetAllDiceBlueprints());
            
            ShuffleDicePool();
        }
    }

    //Shuffle the dice pool
    void ShuffleDicePool()
    {
        System.Random rng = new System.Random();

        // Fisher-Yates shuffle
        int n = DicePool.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            DiceBlueprints value = DicePool[k];
            DicePool[k] = DicePool[n];
            DicePool[n] = value;
        }
    }
    void ReplenishDicePool()
    {
        DicePool.AddRange(DiscardDicePool);
        DiscardDicePool.Clear();
        ShuffleDicePool();
    }
    
    DiceBlueprints GetNextDice()
    {
        if (Hand.Count > 0)
        {
            DiceBlueprints nextDice = Hand[0];
            Hand.RemoveAt(0);

            // 将使用过的骰子放入废弃区
            DiscardDicePool.Add(nextDice);

            return nextDice;
        }
        else
        {
            Debug.LogWarning("Hand is empty!");
            return null;
        }
    }

    public void StartTurn()
    {
        // 抽滿 5 顆骰子
        Hand.AddRange(DicePool.Take(5));
        DicePool.RemoveAll(dice => Hand.Contains(dice));

        // 檢查是否還需要補滿骰子
        if (Hand.Count < 5)
        {
            ReplenishDicePool();

            // 再次抽滿手上的骰子
            Hand.AddRange(DicePool.Take(5 - Hand.Count));
            DicePool.RemoveAll(dice => Hand.Contains(dice));
        }
    }


    public void DiscordTurn()
    {
        DiscardDicePool.AddRange(Hand);
        Hand.Clear();
    }
    public void EndTurn()
    {
        
    }
    public void DrawOneDice()
    {
        if (DicePool.Count < 1)
        {
            ReplenishDicePool();
        }
        Hand.AddRange(DicePool.Take(1));
        DicePool.RemoveAll(dice => Hand.Contains(dice));
    }
}
