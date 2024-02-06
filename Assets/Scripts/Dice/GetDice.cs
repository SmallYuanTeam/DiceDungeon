using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dice;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetDice : MonoBehaviour
{
    //get three Button component
    public Button GetDiceButton1;
    public Button GetDiceButton2;
    public Button GetDiceButton3;

    public void GetDice3Weighted()
    {
        DiceBlueprints[] allDiceBlueprints = Resources.LoadAll<DiceBlueprints>("DiceBlueprints");
        List<DiceBlueprints> selectedDices = SelectRandomWeightedDice(allDiceBlueprints, 3);

        // 更新 TextMeshPro 文本
        UpdateTextMeshPro(GetDiceButton1.transform.Find("Name").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 0 ? selectedDices[0] : null, "Name");
        UpdateTextMeshPro(GetDiceButton2.transform.Find("Name").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 1 ? selectedDices[1] : null, "Name");
        UpdateTextMeshPro(GetDiceButton3.transform.Find("Name").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 2 ? selectedDices[2] : null, "Name");

        UpdateTextMeshPro(GetDiceButton1.transform.Find("Description").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 0 ? selectedDices[0] : null, "Description");
        UpdateTextMeshPro(GetDiceButton2.transform.Find("Description").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 1 ? selectedDices[1] : null, "Description");
        UpdateTextMeshPro(GetDiceButton3.transform.Find("Description").GetComponent<TextMeshProUGUI>(), selectedDices.Count > 2 ? selectedDices[2] : null, "Description");
        
        //transfer DiceBlueprints to GetDiceButton
        GetDiceButton1.GetComponent<GetDiceButton>().diceBlueprints = selectedDices.Count > 0 ? selectedDices[0] : null;
        GetDiceButton2.GetComponent<GetDiceButton>().diceBlueprints = selectedDices.Count > 1 ? selectedDices[1] : null;
        GetDiceButton3.GetComponent<GetDiceButton>().diceBlueprints = selectedDices.Count > 2 ? selectedDices[2] : null;
    }

    List<DiceBlueprints> SelectRandomWeightedDice(DiceBlueprints[] blueprints, int count)
    {
        List<DiceBlueprints> selected = new List<DiceBlueprints>();
        List<DiceBlueprints> available = new List<DiceBlueprints>(blueprints);

        for (int i = 0; i < count; i++)
        {
            int totalWeight = 0;
            foreach (var blueprint in available)
            {
                totalWeight += blueprint.weight;
            }

            int randomNumber = Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (var blueprint in available)
            {
                currentWeight += blueprint.weight;
                if (randomNumber < currentWeight)
                {
                    selected.Add(blueprint);
                    available.Remove(blueprint);
                    break;
                }
            }
        }

        return selected;
    }
    void Start()
    {
        GetDice3Weighted();
    }

    void UpdateTextMeshPro(TextMeshProUGUI tmpText, DiceBlueprints blueprint, string type)
    {
        if (tmpText != null && blueprint != null)
        {
            string fieldName = GetFieldName(type);
            string fieldValue = GetFieldValue(blueprint, fieldName);
            string info = $"{fieldValue}\n";
            tmpText.text = info;
        }
    }
    string GetFieldName(string type)
    {
        switch (type)
        {
            case "Name":
                return FindObjectOfType<DiceBackpack>().lang == "zh_TW" ? "diceCNName" : "diceName";
            case "Description":
                return FindObjectOfType<DiceBackpack>().lang == "zh_TW" ? "diceCNDescription" : "diceDescription";
            default:
                return "";
        }
    }
    
    string GetFieldValue(DiceBlueprints blueprint, string fieldName)
    {
        // 使用反射來獲取字段值
        return (string)typeof(DiceBlueprints).GetField(fieldName).GetValue(blueprint);
    }
}
