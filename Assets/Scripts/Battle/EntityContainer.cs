using UnityEngine;

public class EntityContainer : MonoBehaviour 
{
    public enum EntityType
    {
        Player,
        Enemy
    }
    public EntityType entityType;
    public GameObject BattleManagers;
    // Get All effect to the entity
    public int Increace = 0; // 亢奮,增傷
    public int Decrease = 0; // 減傷
    public int Poison = 0; // 中毒
    public int Dizzy = 0; // 暈眩
    public int Burn = 0; // 燒傷
    public int Bloodthirsty = 0; // 嗜血
    public int Restrict = 0; // 限制
    public int Paralysis = 0; // 麻痺
    public int Freeze = 0; // 冰凍
    public int Wound = 0; // 傷口
    public int Rebellion = 0; // 盾反
    public int Thorns = 0; // 荊棘
    public int Psychedelic = 0; // 迷幻
    public int Angry = 0; // 憤怒
    public int Purify = 0; // 淨化
    public int Pray = 0; // 祈禱
    public int Curse = 0; // 詛咒
    public int Utility = 0; // 實用
    // Give the Entity Data

    public int HP = 0; // 血量
    public int Energy = 0; // 能量
    public int DiceDraw = 5; // 抽骰子數量
    public int Attack = 0; // 攻擊
    public int Shield = 0; // 護盾
    void Start()
    {
        BattleManagers = GameObject.Find("BattleManagers");
    }
    // 結算傷害
    public void DamageEntity(int damage)
    {
        if (Shield > 0)
        {
            Shield -= damage;
            if (Shield < 0)
            {
                HP += Shield;
                Shield = 0;
            }
        }
        else
        {
            HP -= damage;
            HPChange();
        }
    } 

    // HP結算
    public void HPChange()
    {
        if (HP <= 0)
        {
            if (entityType == EntityType.Player)
            {
                BattleManagers.GetComponent<BattleManager>().GameOver();
            }
            else
            {
                BattleManagers.GetComponent<BattleManager>().Win();
            }
        }
    }
    void GameInit()
    {
        if (entityType == EntityType.Player)
        {
            HP = 100;
            Energy = 3;
            Attack = 0;
            Shield = 0;
        }
    }

    public int AttackDamage(int damage)
    {
        Attack = Increace - Decrease;
        Attack += damage;
        return Attack;
    }

    public void PoisonDamage()
    {
        HP -= Poison;
        HPChange();
        Poison -=1;
    }

    public void BurnDamage()
    {
        HP -= Burn;
        HPChange();
        Burn -= Burn/3;
    }
}