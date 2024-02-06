using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dice;

public class GetDiceButton : MonoBehaviour
{
    public DiceBlueprints diceBlueprints;
    public void OnClick()
    {
        // 將骰子藍圖添加到背包
        FindObjectOfType<DiceBackpack>().AddToBackpack(diceBlueprints);
        // 關閉GetDice場景
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("GetDiceScene");
    }
    
}
