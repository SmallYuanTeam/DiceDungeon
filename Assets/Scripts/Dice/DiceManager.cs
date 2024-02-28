using UnityEngine;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    public GameObject dicePrefab; // 骰子预制体
    public GameObject player; // 玩家
    public int numberOfDice = 0; // 每次需要生成的骰子数量

    private List<GameObject> diceInstances = new List<GameObject>(); // 存储当前场景中的骰子实例

    void Start()
    {
        player = GameObject.Find("Player");
    }
    // 生成新的骰子实例
    public void GenerateDice()
    {
        numberOfDice = player.GetComponent<EntityContainer>().DiceDraw;
        // 清空当前场景中的所有骰子实例
        ClearDice();

        // 生成新的骰子实例
        for (int i = 0; i < numberOfDice; i++)
        {
            GameObject diceInstance = Instantiate(dicePrefab, GetSpawnPosition(i), Quaternion.identity);
            // 设置骰子的其他属性，例如材质、尺寸等
            // 这里根据需要设置骰子的属性
            diceInstances.Add(diceInstance);
        }
    }

    // 获取骰子的生成位置
    private Vector3 GetSpawnPosition(int index)
    {
        // 根据需要设置生成位置，这里简单地以索引为基础生成位置
        return new Vector3(index * 2, 0, 0);
    }

    // 清空当前场景中的所有骰子实例
    private void ClearDice()
    {
        foreach (var diceInstance in diceInstances)
        {
            Destroy(diceInstance);
        }
        diceInstances.Clear();
    }

    // 在抽取完骰子或将骰子打出去后调用此方法重新生成骰子
    public void ReGenerateDice()
    {
        GenerateDice();
    }
}
