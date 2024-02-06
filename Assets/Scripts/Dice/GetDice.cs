using System.Collections.Generic;
using Dice;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetDice : MonoBehaviour
{
    //get LangController component on self
    public LangController langController;
    //get three Button component
    public Button GetDiceButton;
    public Button GetDiceButton2;
    public Button GetDiceButton3;

    public void GetDice3Weighted()
    {
        DiceBlueprints[] allDiceBlueprints = Resources.LoadAll<DiceBlueprints>("DiceBlueprints");
        List<DiceBlueprints> selectedDices = SelectRandomWeightedDice(allDiceBlueprints, 3);
        //get Button TextMeshPro component with "Name" tag
        TextMeshProUGUI DiceNameTextA = GetDiceButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI DiceNameTextB = GetDiceButton2.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI DiceNameTextC = GetDiceButton3.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        //get Button TextMeshPro component with "Description" tag
        TextMeshProUGUI DiceDescriptionTextA = GetDiceButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI DiceDescriptionTextB = GetDiceButton2.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI DiceDescriptionTextC = GetDiceButton3.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        // 更新 TextMeshPro 文本
        UpdateNameTMP(DiceNameTextA, selectedDices.Count > 0 ? selectedDices[0] : null);
        UpdateNameTMP(DiceNameTextB, selectedDices.Count > 1 ? selectedDices[1] : null);
        UpdateNameTMP(DiceNameTextC, selectedDices.Count > 2 ? selectedDices[2] : null);

        UpdateDescriptionTMP(DiceDescriptionTextA, selectedDices.Count > 0 ? selectedDices[0] : null);
        UpdateDescriptionTMP(DiceDescriptionTextB, selectedDices.Count > 1 ? selectedDices[1] : null);
        UpdateDescriptionTMP(DiceDescriptionTextC, selectedDices.Count > 2 ? selectedDices[2] : null);
        // 使用 selectedDices 做些什么
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
// UpdateTextMeshPro 函数
    void UpdateNameTMP(TextMeshProUGUI tmpText, DiceBlueprints blueprint)
    {
        if (tmpText != null && blueprint != null)
        {
            if (langController.lang == "zh_TW")
            {
                string info = $"{blueprint.diceCNName}\n";
                tmpText.text = info;
            }
            else if (langController.lang == "en_US")
            {
                string info = $"{blueprint.diceName}\n";
                tmpText.text = info;
            }
        }
    }
    void UpdateDescriptionTMP(TextMeshProUGUI tmpText, DiceBlueprints blueprint)
    {
        if (tmpText != null && blueprint != null)
        {
            if (langController.lang == "zh_TW")
            {
                string info = $"{blueprint.diceCNDescription}";
                tmpText.text = info;
            }
            else if (langController.lang == "en_US")
            {
                string info = $"{blueprint.diceDescription}";
                tmpText.text = info;
            }
        }
    }
}
