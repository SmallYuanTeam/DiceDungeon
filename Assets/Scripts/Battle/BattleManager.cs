using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Dice;
using UnityEngine;
using System.Diagnostics;
public enum GamePhase
{
    StartInit,
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
        Hand.AddRange(DicePool.Take(5));
        DicePool.RemoveAll(dice => Hand.Contains(dice));
        if (DicePool.Count == 0)
        {
            UnityEngine.Debug.LogWarning("Pool is empty!");
            return null;
        }
    }
    
    
    // 進入場景時初始化
    void Start()
    {
        diceBackpack = FindObjectOfType<DiceBackpack>();
        SwitchPhase(GamePhase.StartInit);
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

            SwitchPhase(GamePhase.Start);
        }
    }

    public IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Standby);
    }

    public IEnumerator StandbyTurn()
    {
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Draw);
    }

    public IEnumerator DrawTurn()
    {
        Hand.AddRange(DicePool.Take(5));
        DicePool.RemoveAll(dice => Hand.Contains(dice));

        
        if (Hand.Count < 5) // 檢查是否還需要補滿骰子
        {
            ReplenishDicePool();

            
            Hand.AddRange(DicePool.Take(5 - Hand.Count)); // 補滿骰子
            DicePool.RemoveAll(dice => Hand.Contains(dice));
        }

        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Battle);
    }

    public IEnumerator BattleTurn()
    {
        
    }

    // 丟棄骰子階段
    public IEnumerator DiscordTurn()
    {
        DiscardDicePool.AddRange(Hand);
        Hand.Clear();
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.End);
    }
    // 回合結束階段
    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Enemy);
    }
    // 怪物回合階段
    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(3f);
        SwitchPhase(GamePhase.Start);
    }


    // 戰鬥結束切換到丟棄骰子階段
    public void EndBattle()
    {
        SwitchPhase(GamePhase.Discard);
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
            case GamePhase.StartInit:
                StartInitlizeBattle();
                //SwitchPhase(GamePhase.Start);
                break;
            case GamePhase.Start:
                StartTurn();
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
