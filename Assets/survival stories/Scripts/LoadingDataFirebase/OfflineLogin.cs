using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OfflineLogin : MonoBehaviour
{
    public string userID = "896dGp2direZew6A1kwP1DG4zzv2";
    [SerializeField] FireBaseData firebaseData;
    void Awake()
    {
        //firebaseData.Initialize();
        
    }

    private void Start()
    {
        
    }

    public async Task<object> ReadDatabaseAsync(string dataTitle)
    {
        object result = null;

        if (userID != null)
        {
            result = await firebaseData.ReadDirectDatabaseAsync(userID, dataTitle);
        }
        else
        {
            Debug.LogError("User is not signed in. Unable to read from the database.");
        }

        return result;
    }

    public async void LoadDataFromDatabase()
    {
        //  -> Call ReadDatabaseAsync for all values
        //  -> Check all value types, if already present then override, else create new (Do this for all necessary required values)

        //  -> Set the matching prefabs according to the values
        //  -> Logs for Debugging

        //  -> this will run after the authentication has been completed and loading from Firebase will not be done in the next scene
        Debug.Log("inside LoadDataOnFirebase");

        //if (PlayerPrefs.HasKey("inventorySize"))
        {
            object inventorySizeObject = await ReadDatabaseAsync("inventorySize");
            if (TryConvertToInt(inventorySizeObject, out int inventorySize))
            {
                Debug.Log("inventorySize: " + inventorySize);
                PlayerPrefs.SetInt("inventorySize", inventorySize);
            }
            else
            {
                Debug.LogError("Failed to convert 'inventorySize' to int.");
            }
        }

        //if (PlayerPrefs.HasKey("inventory"))
        {
            object inventoryObject = await ReadDatabaseAsync("inventory");
            if (TryConvertToString(inventoryObject, out string inventoryData))
            {
                Debug.Log("inventoryData: " + inventoryData);
                PlayerPrefs.SetString("inventory", inventoryData);
            }
            else
            {
                Debug.LogError("Failed to convert 'inventory' to string.");
            }
        }

        //if (PlayerPrefs.HasKey("constructionsystem"))
        {
            object constructionObject = await ReadDatabaseAsync("constructionsystem");
            if (TryConvertToString(constructionObject, out string constructionData))
            {
                Debug.Log("constructionData: " + constructionData);
                PlayerPrefs.SetString("constructionsystem", constructionData);
            }
            else
            {
                Debug.LogError("Failed to convert 'constructionsystem' to string.");
            }
        }

        

        //if (PlayerPrefs.HasKey("Player PRofile Manager"))
        {
            object profileManagerObject = await ReadDatabaseAsync("Player PRofile Manager");
            if (TryConvertToString(profileManagerObject, out string profileManagerData))
            {
                Debug.Log("profileManagerData: " + profileManagerData);
                PlayerPrefs.SetString("Player PRofile Manager", profileManagerData);
            }
            else
            {
                Debug.LogError("Failed to convert 'Player PRofile Manager' to string.");
            }
        }

        

        {
            object goldData = await ReadDatabaseAsync("gold");
            if (TryConvertToInt(goldData, out int gold))
            {
                Debug.Log("gold: " + gold);
                if (gold > 0 && gold < 1000)
                {

                    PlayerPrefs.SetInt("gold", gold);
                }
                else
                {
                    PlayerPrefs.SetInt("gold", 0);
                }
            }
            else
            {
                Debug.LogError("Failed to convert 'gold' to int.");
            }

        }
        {
            object hasPlayedTutorialData = await ReadDatabaseAsync("HasPlayedTutorial");
            if (TryConvertToInt(hasPlayedTutorialData, out int hasPlayedTutorial))
            {
                Debug.Log("hasPlayedTutorial: " + hasPlayedTutorial);
                if (hasPlayedTutorial == 1)
                {
                    PlayerPrefs.SetInt("HasPlayedTutorial", hasPlayedTutorial);
                }
                else
                {
                    PlayerPrefs.SetInt("HasPlayedTutorial", 0);
                }
            }
            else
            {
                Debug.LogError("Failed to convert 'hasPlayedTutorial' to int.");
            }

        }
        //if (PlayerPrefs.HasKey("Player Attributes System"))
        {
            object attributeSystemObject = await ReadDatabaseAsync("Player Attributes System");
            if (TryConvertToString(attributeSystemObject, out string attributeSystemData))
            {
                Debug.Log("attributeSystemData: " + attributeSystemData);
                PlayerPrefs.SetString("Player Attributes System", attributeSystemData);
            }
            else
            {
                Debug.LogError("Failed to convert 'Player Attributes System' to string.");
            }
        }
        //if (PlayerPrefs.HasKey("achievSkill"))
        {
            object achievSkillObject = await ReadDatabaseAsync("achievSkill");
            if (TryConvertToString(achievSkillObject, out string achievSkillData))
            {
                Debug.Log("achievSkillData: " + achievSkillData);
                PlayerPrefs.SetString("achievSkill", achievSkillData);
            }
            else
            {
                Debug.LogError("Failed to convert 'achievSkill' to string.");
            }

        }
        // Similar conversion for 'gold'
        // object goldObject = await ReadDatabaseAsync("gold");
        // if (TryConvertToInt(goldObject, out int goldValue))
        // {
        //     PlayerPrefs.SetInt("gold", goldValue);
        // }
        // else
        // {
        //     Debug.LogError("Failed to convert 'gold' to int.");
        // }
        Debug.Log("Retreived the following Data: \ninventorySize:" + PlayerPrefs.GetInt("inventorySize")
                + ", \ninventory:" + PlayerPrefs.GetString("inventory")
                + ", \nconstructionsystem:" + PlayerPrefs.GetString("constructionsystem")
                + ", \nPlayer Attributes System:" + PlayerPrefs.GetString("Player Attributes System")
                + ", \nPlayer PRofile Manager" + PlayerPrefs.GetString("Player PRofile Manager")
                + ", \nachievSkill" + PlayerPrefs.GetString("achievSkill"));
    }

    private bool TryConvertToInt(object value, out int result)
    {
        if (value is int intValue)
        {
            result = intValue;
            return true;
        }
        else if (value is long longValue)
        {
            result = (int)longValue;
            return true;
        }
        else if (value is string stringValue && int.TryParse(stringValue, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    private bool TryConvertToString(object value, out string result)
    {
        result = value?.ToString();
        return result != null;
    }
    public void PrintM()
    {
        Debug.Log("double tapped");
    }
}
