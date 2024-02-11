using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Dice;

public class DiceBackpack : MonoBehaviour
{
    public string lang = "zh_TW";
    public string[] langList = new string[] { "zh_TW", "en_US" };
    public List<DiceBlueprints> diceBlueprintsList = new List<DiceBlueprints>();

    // 將骰子藍圖添加到背包
    public void AddToBackpack(DiceBlueprints diceBlueprint)
    {
        diceBlueprintsList.Add(diceBlueprint);
    }

    // 從背包中獲取所有骰子藍圖
    public List<DiceBlueprints> GetAllDiceBlueprints()
    {
        return diceBlueprintsList;
    }

    // 從背包中獲取特定骰子藍圖（根據索引）
    public DiceBlueprints GetDiceBlueprintAtIndex(int index)
    {
        if (index >= 0 && index < diceBlueprintsList.Count)
        {
            return diceBlueprintsList[index];
        }
        return null;
    }

    // 從背包中移除特定骰子藍圖
    public void RemoveFromBackpack(DiceBlueprints diceBlueprint)
    {
        diceBlueprintsList.Remove(diceBlueprint);
    }

    // 清空背包
    public void ClearBackpack()
    {
        diceBlueprintsList.Clear();
    }

    // 導入GetDice場景並疊加在當前場景上
    public void LoadGetDiceScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GetDiceScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void SaveBackpackData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "DiceDungeon/backpack.json");
        BackpackData backpackData = new BackpackData(diceBlueprintsList);
        string jsonData = JsonUtility.ToJson(backpackData);
        File.WriteAllText(filePath, jsonData);
    }

    // 从 JSON 文件加载背包數據
    public void LoadBackpackData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "DiceDungeon/backpack.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            BackpackData backpackData = JsonUtility.FromJson<BackpackData>(jsonData);

            if (backpackData != null)
            {
                diceBlueprintsList = backpackData.diceBlueprintsList;
            }
        }
        else
        {
            Debug.LogWarning("No backpack data found at: " + filePath);
        }
    }

    // 取得所有骰子藍圖並顯示在Log中
    public void ShowAllDiceBlueprints()
    {
        string log = "All Dice Blueprints: ";
        foreach (DiceBlueprints diceBlueprint in diceBlueprintsList)
        {
            log += diceBlueprint.diceName + ", ";
        }
        Debug.Log(log);
    }

    public void ChangeLang()
    {
        int index = System.Array.IndexOf(langList, lang);
        index = (index + 1) % langList.Length;
        lang = langList[index];
    }
}
