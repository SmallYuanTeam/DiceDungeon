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

    // public void GetDice3()
    // {
    //     DiceBlueprints[] allDiceBlueprints = Resources.LoadAll<DiceBlueprints>("DiceBlueprints");
    //     List<DiceBlueprints> selectedDiceBlueprints = SelectRandomDiceBlueprints(allDiceBlueprints, 3);
        
    //     if (selectedDiceBlueprints.Count >= 3)
    //     {
    //         DiceInfoTextA.text = selectedDiceBlueprints[0].diceName;
    //         DiceInfoTextB.text = selectedDiceBlueprints[1].diceName;
    //         DiceInfoTextC.text = selectedDiceBlueprints[2].diceName;
    //     }
    // }

    // List<DiceBlueprints> SelectRandomDiceBlueprints(DiceBlueprints[] Blueprints, int numberOfDice)
    // {
    //     List<DiceBlueprints> selected = new List<DiceBlueprints>();
    //     List<int> alreadySelected = new List<int>();

    //     for (int i = 0; i < numberOfDice; i++)
    //     {
    //         int index;
    //         do
    //         {
    //             index = Random.Range(0, Blueprints.Length);
    //         } while (alreadySelected.Contains(index));
    //         {
    //             alreadySelected.Add(index);
    //             selected.Add(Blueprints[index]);                
    //         }
    //     }
    //     return selected;
    // }

// weighted dice


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
            // 假设 DiceBlueprint 有一个名为 'name' 的属性
            tmpText.text = blueprint.diceName;
        }
    }
}
