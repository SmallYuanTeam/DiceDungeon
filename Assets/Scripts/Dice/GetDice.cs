using System.Collections.Generic;
using Dice;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetDice : MonoBehaviour
{
    public TextMeshProUGUI DiceInfoTextA;
    public TextMeshProUGUI DiceInfoTextB;
    public TextMeshProUGUI DiceInfoTextC;

    public void GetDice3Weighted()
    {
        DiceBlueprints[] allDiceBlueprints = Resources.LoadAll<DiceBlueprints>("DiceBlueprints");
        List<DiceBlueprints> selectedDices = SelectRandomWeightedDice(allDiceBlueprints, 3);

        // 更新 TextMeshPro 文本
        UpdateTextMeshPro(DiceInfoTextA, selectedDices.Count > 0 ? selectedDices[0] : null);
        UpdateTextMeshPro(DiceInfoTextB, selectedDices.Count > 1 ? selectedDices[1] : null);
        UpdateTextMeshPro(DiceInfoTextC, selectedDices.Count > 2 ? selectedDices[2] : null);

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
// weighted dice
    void UpdateTextMeshPro(TextMeshProUGUI tmpText, DiceBlueprints blueprint)
    {
        if (tmpText != null && blueprint != null)
        {
            string info = $"名稱：{blueprint.diceName}\n" +
                          $"描述：{blueprint.diceDescription}";
            tmpText.text = info;
        }
    }
}
