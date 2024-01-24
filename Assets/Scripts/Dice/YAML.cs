using System.Collections;
using System.Collections.Generic;
using Dice;
using UnityEngine;


public class YAML : MonoBehaviour
{
    // Get the YAML file from the Resources folder
    public TextAsset yamlFile;
    public DiceBlueprints dicePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Parse the YAML file
        var yaml = new YamlDotNet.Serialization.DeserializerBuilder().Build();
        var result = yaml.Deserialize<Dictionary<string, object>>(yamlFile.text);

        // Print the YAML file
        foreach (var entry in result)
        {
            Debug.Log(entry.Key + ": " + entry.Value);
            // Set the dice prefab
            dicePrefab = (DiceBlueprints)entry.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
