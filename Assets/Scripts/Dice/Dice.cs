using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class Dice : MonoBehaviour
    {
        public Image spriteRenderer; // 骰子的图像渲染器
        public Text valueText; // 显示骰子值的文本

        // 设置骰子的属性
        public void SetDiceAttributes(DiceBlueprints blueprints)
        {
            spriteRenderer.sprite = blueprints.sprite; // 设置图像
        }
    }
}
