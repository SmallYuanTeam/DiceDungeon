using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Dice;
using UnityEngine;
using System.Diagnostics;
public enum GamePhase
{
    Start,
    Standby,
    Draw,
    Battle,
    Discard,
    Enemy,
    End
}
public class BattleManager : MonoBehaviour
{
    public DiceBackpack diceBackpack;
    public List<DiceBlueprints> DicePool;
    public List<DiceBlueprints> DiscardDicePool;
    public List<DiceBlueprints> Hand;
    private GamePhase currentPhase;

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
            UnityEngine.Debug.LogWarning("Hand is empty!");
            return null;
        }
    }
    
    
    // 進入場景時初始化
    void Start()
    {
        diceBackpack = FindObjectOfType<DiceBackpack>();
        SwitchPhase(GamePhase.Start);
    }
    //進入戰鬥時初始化
    public void StartInitlizeBattle()
    {
        if (diceBackpack == null)
        {
            UnityEngine.Debug.LogError("DiceBackpack not found");
        }
        else
        {
            DicePool = new List<DiceBlueprints>(diceBackpack.GetAllDiceBlueprints());
            
            ShuffleDicePool();
        }
    }
    public IEnumerator StartTurn()
    {
        if (currentPhase == GamePhase.Draw)
        {
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
            yield return new WaitForSeconds(3f);
            SwitchPhase(GamePhase.Battle);
        }
        // 抽滿 5 顆骰子
    }

    // 丟棄骰子階段
    public IEnumerator DiscordTurn()
    {
        DiscardDicePool.AddRange(Hand);
        Hand.Clear();
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.End);
    }
    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Enemy);
    }
    // 抽一顆骰子(調試功能)
    public void DrawOneDice()
    {
        if (DicePool.Count < 1)
        {
            ReplenishDicePool();
        }
        Hand.AddRange(DicePool.Take(1));
        DicePool.RemoveAll(dice => Hand.Contains(dice));
    }
    // 切換遊戲階段
    void SwitchPhase(GamePhase newPhase)
    {
        currentPhase = newPhase;
    }
    void Update()
    {
        switch (currentPhase) // 回合階段更新
        {
            case GamePhase.Start:
                StartInitlizeBattle();
                //SwitchPhase(GamePhase.Standby);
                break;
            case GamePhase.Standby:
                //SwitchPhase(GamePhase.Draw);
                break;
            case GamePhase.Draw:
                StartTurn();
                //SwitchPhase(GamePhase.Battle);
                break;
            case GamePhase.Battle:
                //SwitchPhase(GamePhase.Discard);
                break;
            case GamePhase.Discard:
                DiscordTurn();
                //SwitchPhase(GamePhase.Enemy);
                break;
            case GamePhase.End:
                EndTurn();
                //SwitchPhase(GamePhase.Draw);
                break;
            case GamePhase.Enemy:
                //SwitchPhase(GamePhase.End);
                break;
        }
    }
    
}
