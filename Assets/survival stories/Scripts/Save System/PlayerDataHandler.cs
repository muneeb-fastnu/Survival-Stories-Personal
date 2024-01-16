using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class PlayerDataHandler : MonoBehaviour
{
    private string jsonFilePath;

    private void Start()
    {
        // Set the path to your JSON file
        jsonFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");

        // Example: Save data
        SavePlayerData();

        // Example: Load data
        LoadPlayerData();
    }

    private void SavePlayerData()
    {
        PlayerData playerData = new PlayerData
        {
            PlayerAttributesSystem = new PlayerAttributes
            {
                Health = 100,
                Hunger = 75,
                Thirst = 50
                // Add other attributes as needed
            },
            PlayerProfileManager = new PlayerProfile
            {
                PlayerName = "John Doe",
                PlayerLevel = 5
                // Add other profile manager data as needed
            },
            Inventory = new Inventory
            {
                Item1 = 10,
                Item2 = 5
                // Add other inventory items and quantities as needed
            },
            InventorySize = 20,
            SkillPointAchievements = new SkillPointAchievements
            {
                Achievement8 = true,
                Achievement15 = false,
                Achievement19 = true,
                Achievement21 = false,
                // Add other achievements as needed
            },
            ConstructionSystem = new ConstructionSystem2
            {
                Building1 = true,
                Building2 = false
                // Add other construction system data as needed
            },
            LoadOrNot = true
        };

        // Convert the data to JSON format
        string jsonData = JsonConvert.SerializeObject(playerData, Formatting.Indented);

        // Write the JSON data to the file
        File.WriteAllText(jsonFilePath, jsonData);

        Debug.Log("Data written to: " + jsonFilePath);
    }

    private void LoadPlayerData()
    {
        // Check if the file exists
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(jsonFilePath);

            // Convert the JSON data to PlayerData object
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(jsonData);

            // Example: Accessing loaded data
            Debug.Log("Loaded health: " + playerData.PlayerAttributesSystem.Health);
            Debug.Log("Loaded playerName: " + playerData.PlayerProfileManager.PlayerName);
            Debug.Log("Loaded inventory size: " + playerData.InventorySize);
            // Add other access statements as needed
        }
        else
        {
            Debug.LogWarning("Player data file not found.");
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public PlayerAttributes PlayerAttributesSystem;
    public PlayerProfile PlayerProfileManager;
    public Inventory Inventory;
    public int InventorySize;
    public SkillPointAchievements SkillPointAchievements;
    public ConstructionSystem2 ConstructionSystem;
    public bool LoadOrNot;
}

[System.Serializable]
public class PlayerAttributes
{
    public float Health;
    public float Hunger;
    public float Thirst;
    // Add other attributes as needed
}

[System.Serializable]
public class PlayerProfile
{
    public string PlayerName;
    public int PlayerLevel;
    // Add other profile manager data as needed
}

[System.Serializable]
public class Inventory
{
    public int Item1;
    public int Item2;
    // Add other inventory items and quantities as needed
}

[System.Serializable]
public class SkillPointAchievements
{
    public bool Achievement8;
    public bool Achievement15;
    public bool Achievement19;
    public bool Achievement21;
    // Add other achievements as needed
}

[System.Serializable]
public class ConstructionSystem2
{
    public bool Building1;
    public bool Building2;
    // Add other construction system data as needed
}
