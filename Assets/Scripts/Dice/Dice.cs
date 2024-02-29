using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Dice
{
    public class Dice : MonoBehaviour
    {
        public DiceBlueprints diceBlueprint;
        public DiceRarity diceRarity;
        public DiceType diceType;
        public string diceID;
        public string diceName;
        public string diceCNName;
        public string diceDescription;
        public string diceCNDescription;
        public int weight;
        public Sprite sprite;
        public List<int> diceValues;
        public List<DiceAbility> diceAbilities;
        public List<DiceTarget> diceTargets;
        
        private void InitBlueprint()
        {
            // 确保已分配骰子数据
            if (diceBlueprint != null)
            {
                diceRarity = diceBlueprint.diceRarity;
                diceType = diceBlueprint.diceType;
                diceID = diceBlueprint.diceID;
                diceName = diceBlueprint.diceName;
                diceCNName = diceBlueprint.diceCNName;
                diceDescription = diceBlueprint.diceDescription;
                diceCNDescription = diceBlueprint.diceCNDescription;
                weight = diceBlueprint.weight;
                sprite = diceBlueprint.sprite;
                diceValues = diceBlueprint.diceValues;
                diceAbilities = diceBlueprint.diceAbilities;
                diceTargets = diceBlueprint.diceTargets;

                //set dice  image
                Image diceImage = GetComponent<Image>();
                if (diceImage != null)
                {
                    diceImage.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Dice Image is not assigned to the Dice prefab.");
                }

            }
            else
            {
                Debug.LogWarning("DiceBlueprint is not assigned to the Dice prefab.");
            }
        }
        public void InitializeFromBlueprint(DiceBlueprints blueprint)
        {
            diceBlueprint = blueprint;
            InitBlueprint();
        }

    }
}
