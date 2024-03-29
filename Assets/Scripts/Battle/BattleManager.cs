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
    private DiceBackpack diceBackpack;
    public GameObject player;
    public GameObject enemy;
    public TMPro.TextMeshProUGUI TurnText;
    public List<DiceBlueprints> DicePool;
    public List<DiceBlueprints> DiscardDicePool;
    public List<DiceBlueprints> Hand;
    private int ContainsDamage = 0;
    private Vector3 startPos = new Vector3(0f, 2f, 0f);
    private bool isPlayerTurn = false;
    private bool hasUpdate = false;
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
    void ResetUpdateFlag()
    {
        hasUpdate = false;
    }
    
    // 進入場景時初始化
    void Start()
    {
        diceBackpack = FindObjectOfType<DiceBackpack>();
        player = GameObject.Find("Player");
    }
    //進入戰鬥時初始化
    public IEnumerator StartInitlizeBattle()
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
            yield return new WaitForSeconds(0.01f);
            ResetUpdateFlag();
        }
        
    }
    // 開始階段
    public IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.Standby);
        ResetUpdateFlag();
    }
    // 等待階段
    public IEnumerator StandbyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.Draw);
        ResetUpdateFlag();
    }
    // 抽牌階段
    public IEnumerator DrawTurn()
    {
        Hand.AddRange(DicePool.Take(player.GetComponent<EntityContainer>().DiceDraw));
        DicePool.RemoveAll(dice => Hand.Contains(dice));

        
        if (Hand.Count < player.GetComponent<EntityContainer>().DiceDraw) // 檢查是否還需要補滿骰子
        {
            ReplenishDicePool();

            
            Hand.AddRange(DicePool.Take(player.GetComponent<EntityContainer>().DiceDraw - Hand.Count)); // 補滿骰子
            DicePool.RemoveAll(dice => Hand.Contains(dice));
        }

        startPos = new Vector3(0f, 2f, 0f);
        UpdateHandDicePrefabs();
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.Battle);
        ResetUpdateFlag();
    }
    // 玩家回合階段
    public IEnumerator BattleTurn()
    {
        isPlayerTurn = true;
        yield return new WaitForSeconds(0.1f);
        // 等待玩家操作
        while (isPlayerTurn)
        {
            yield return null;
        }
    }

    // 丟棄骰子階段
    public IEnumerator DiscordTurn()
    {
        DiscardDicePool.AddRange(Hand);
        Hand.Clear();
        UpdateHandDicePrefabs();
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.End);
        ResetUpdateFlag();
    }
    // 回合結束階段
    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.Enemy);
        ResetUpdateFlag();
    }
    // 怪物回合階段
    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchPhase(GamePhase.Start);
        ResetUpdateFlag();
    }

//    public void PerformAttack(int damage, GameObject attacker, GameObject target, List<DiceTarget> diceTargets)
//     {
//         EntityContainer attackerContainer = attacker.GetComponent<EntityContainer>();
//         EntityContainer targetContainer = target.GetComponent<EntityContainer>();

//         if (attackerContainer != null && targetContainer != null)
//         {
//             int totalDamage = damage + attackerContainer.Attack;
//             targetContainer.AttackEntity(totalDamage);

//             // 在这里处理 diceTargets 相关的逻辑
//             foreach (var diceTarget in diceTargets)
//             {
//                 switch (diceTarget)
//                 {
//                     case DiceTarget.Self:
//                         // 对自己的处理
//                         break;
//                     case DiceTarget.Enemy:
//                         // 对敌人的处理
//                         break;
//                     case DiceTarget.All:
//                         // 对所有的处理
//                         break;
//                 }
//             }
//         }
//         else
//         {
//             //Debug.LogError("EntityContainer not found on attacker or target GameObject.");
//         }
//     }

//     public void PlayerAttack(int damage, List<DiceTarget> diceTargets)
//     {
        
//         PerformAttack(damage, player, enemy, diceTargets);
//     }

//     public void EnemyAttack(int damage, List<DiceTarget> diceTargets)
//     {
//         PerformAttack(damage, enemy, player, diceTargets);
//     }
    // 將Hand列表中的骰子實例化
    public void UpdateHandDicePrefabs()
    {
        // 清空所有子物件
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 初始化水平偏移量
        float offsetX = 4f;

        // 搜尋Hand列表中的所有骰子
        foreach (DiceBlueprints blueprint in Hand)
        {
            // 生成預置體
            GameObject dicePrefabInstance = Instantiate(Resources.Load<GameObject>("DicePrefab"));

            // 獲取Dice組件
            Dice.Dice diceComponent = dicePrefabInstance.GetComponent<Dice.Dice>();

            // 初始化骰子
            diceComponent.InitializeFromBlueprint(blueprint);

            // 設定骰子位置
            Vector3 position = new Vector3(offsetX, -3f, 0f);
            dicePrefabInstance.transform.position = position;

            // 設定父物件
            dicePrefabInstance.transform.SetParent(transform);

            // 更新水平偏移量
            offsetX -= 2.0f;
        }
    }
    // 玩家勝利
    public void Win()
    {
        UnityEngine.Debug.Log("Win");
    }
    // 玩家失敗
    public void GameOver()
    {
        UnityEngine.Debug.Log("GameOver");
    }
    // 戰鬥結束切換到丟棄骰子階段
    public void EndBattle()
    {
        isPlayerTurn = false;
        SwitchPhase(GamePhase.Discard);
        ResetUpdateFlag();
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
        UpdateHandDicePrefabs();
    }
    // 切換遊戲階段
    void SwitchPhase(GamePhase newPhase)
    {
        currentPhase = newPhase;
    }

    void Update()
    {
        if (!hasUpdate)
        {
            UnityEngine.Debug.Log(currentPhase);
            TurnText.text = currentPhase.ToString();
            switch (currentPhase) // 回合階段更新
            {
                case GamePhase.StartInit:
                    StartCoroutine(StartInitlizeBattle());
                    //SwitchPhase(GamePhase.Start);
                    break;
                case GamePhase.Start:
                    StartCoroutine(StartTurn());
                    //SwitchPhase(GamePhase.Standby);
                    break;
                case GamePhase.Standby:
                    StartCoroutine(StandbyTurn());
                    //SwitchPhase(GamePhase.Draw);
                    break;
                case GamePhase.Draw:
                    StartCoroutine(DrawTurn());
                    //SwitchPhase(GamePhase.Battle);
                    break;
                case GamePhase.Battle:
                    StartCoroutine(BattleTurn());
                    //SwitchPhase(GamePhase.Discard);
                    break;
                case GamePhase.Discard:
                    StartCoroutine(DiscordTurn());
                    //SwitchPhase(GamePhase.Enemy);
                    break;
                case GamePhase.End:
                    StartCoroutine(EndTurn());
                    //SwitchPhase(GamePhase.Draw);
                    break;
                case GamePhase.Enemy:
                    StartCoroutine(EnemyTurn());
                    //SwitchPhase(GamePhase.End);
                    break;
            }
            if (currentPhase != GamePhase.StartInit)
            {
                hasUpdate = true;
            }
        }
    }
}