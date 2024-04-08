using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.UI;
using Google;
using System.Net.Http;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Data;
using Newtonsoft.Json;

public class FirebaseSignInScriptO : MonoBehaviour
{
    public string GoogleWebAPI = "1046390475413-4a7kp5sj1v9ihg56hj1mn41nbq06q9nm.apps.googleusercontent.com";

    public myMUser myUser;

    

    public TMP_Text UsernameTXT, UserEmailTXT;
    public Image UserProfilePicture;
    public string imageURL;
    public GameObject LoginScreen, ProfileScreen;

    [SerializeField] private FireBaseData firebaseData;

    SkillNames2 allSkillNamesWrapper = new SkillNames2();
    private void Awake()
    {
        firebaseData.Initialize();

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //InitFirebase();
        firebaseData.Initialize();

        DontDestroyOnLoad(gameObject);
        InvokeRepeating(nameof(SaveDataOnFireBase), 30f, 10f);
        myUser.UserId = "896dGp2direZew6A1kwP1DG4zzv2";
        myUser.DisplayName = "Muneeb Javed";
        myUser.Email = "watchingsafe21@gmail.com";
    }
    void InitFirebase()
    {
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void GoogleSignInClick()
    {
        Debug.Log("Signin Button Clicked");
        

        //GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }
    public void OnGoogleAuthenticationFinished()
    { 
        
        
        Debug.Log("Continue with OnGoogleAuthentication STARTED");
        

        string[] nameParts = myUser.DisplayName.Split(' ');

        string firstName = (nameParts.Length > 0) ? nameParts[0] : "";
        PlayerPrefs.SetString("Username", firstName);

        UsernameTXT.text = "Player ID: " + myUser.Email;

        //Testing*****************************************************************************************************************
        WriteDatabase("testData", "Test");

        Debug.Log("Attempting to read from database.");



        Debug.Log("Title: Test \t| Data: " + ReadDatabaseAsync("Test"));
        Debug.Log("Title: inventorySize \t| Data: " + ReadDatabaseAsync("inventorySize"));
        Debug.Log("Title: inventory \t| Data: " + ReadDatabaseAsync("inventory"));
        PlayerPrefs.SetString("LoadorNot", "true");
        LoadDataFromDatabase();

        Debug.Log("exiting");
    }

    void LoadScene()
    {
        Debug.Log("loading scene");
        SceneManager.LoadScene(1);
    }

    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageURL;
    }
    IEnumerator LoadImage(string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;

        UserProfilePicture.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
    public void SignOut()
    {
        
    }

    public void WriteInDatabase()
    {
        if (myUser != null)
        {
            if (firebaseData == null)
            {
            }
            firebaseData.WriteStringData(myUser.UserId, "String text for backend 11", "Title");
        }
        else
        {
            Debug.Log("User is not signed in. Unable to write to the database.");
        }
    }
    public void WriteDatabase(object data, string dataTitle)
    {
        if (myUser != null)
        {
            Debug.Log("Second last writeDatabase");
            //if (firebaseData == null)
            //{
            //    Debug.Log("firebaseData Component was null");
            //    firebaseData = GetComponent<FireBaseData>();
            //}
            //if (firebaseData.reference != FirebaseDatabase.DefaultInstance.RootReference)
            //{
            //    Debug.Log("FireBase Database wasn't initialized.");

            //    firebaseData.Initialize();
            //}
            firebaseData.WriteStringData(myUser.UserId, data, dataTitle);
        }
        else
        {
            Debug.Log("User is not signed in. Unable to write to the database.");
        }
    }
    public async Task<object> ReadDatabaseAsync(string dataTitle)
    {
        object result = null;

        if (myUser != null)
        {
            result = await firebaseData.ReadDirectDatabaseAsync(myUser.UserId, dataTitle);
        }
        else
        {
            Debug.Log("User is not signed in. Unable to read from the database.");
        }

        return result;
    }

    public void SaveDataOnFireBase()
    {
        Debug.Log("inside SaveDataOnFirebase");
        if (myUser != null)
        {
            string id = myUser.UserId;
            Debug.Log("user is not null and id is: " + myUser.UserId);
            if (PlayerPrefs.HasKey("inventorySize"))
            {
                int inventorySize = PlayerPrefs.GetInt("inventorySize");
                if (PlayerPrefs.HasKey("inventory"))
                {
                    string inventoryData = PlayerPrefs.GetString("inventory");
                    Debug.Log("Attempting to write on database: " + inventoryData + " and size: " + inventorySize);
                    WriteDatabase(inventoryData, "inventory");
                    WriteDatabase(inventorySize, "inventorySize");
                }
            }
            if (PlayerPrefs.HasKey("PortalQuantity"))
            {
                int PortalQuantity = PlayerPrefs.GetInt("PortalQuantity");
                Debug.Log("Attempting to write on database PortalQuantity: " + PortalQuantity);
                WriteDatabase(PortalQuantity, "PortalQuantity");
            }
            for (int i = -1; i < 23; i++)
            {
                string key = "constructionsystem_" + i;
                if (PlayerPrefs.HasKey(key))
                {
                    string construction = PlayerPrefs.GetString(key);
                    Debug.Log("Attempting to write on database constructionsystem: " + construction);
                    WriteDatabase(construction, key);
                }
            }
            if (PlayerPrefs.HasKey("Username"))
            {
                string username1 = PlayerPrefs.GetString("Username");
                Debug.Log("Attempting to write on database Username: " + username1);
                WriteDatabase(username1, "Username");
            }
            if (PlayerPrefs.HasKey("Player Attributes System"))
            {
                string pas = PlayerPrefs.GetString("Player Attributes System");
                Debug.Log("Attempting to write on database Player Attributes System: " + pas);
                WriteDatabase(pas, "Player Attributes System");
            }
            if (PlayerPrefs.HasKey("Player PRofile Manager"))
            {
                string ppm = PlayerPrefs.GetString("Player PRofile Manager");
                Debug.Log("Attempting to write on database Player PRofile Manager: " + ppm);
                WriteDatabase(ppm, "Player PRofile Manager");
            }

            if (PlayerPrefs.HasKey("achievSkill"))
            {
                string skill = PlayerPrefs.GetString("achievSkill");
                Debug.Log("Attempting to write on database achievSkill: " + skill);
                WriteDatabase(skill, "achievSkill");
            }

            if (PlayerPrefs.HasKey("gold"))
            {
                int goldAmount = PlayerPrefs.GetInt("gold");
                Debug.Log("Attempting to write on database achievSkill: " + goldAmount);
                WriteDatabase(goldAmount, "gold");
            }

            if (PlayerPrefs.HasKey("HasPlayedTutorial"))
            {
                int hasPLayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial");
                Debug.Log("Attempting to write on database HasPlayedTutorial: " + hasPLayedTutorial);
                WriteDatabase(hasPLayedTutorial, "HasPlayedTutorial");
            }
            if (PlayerPrefs.HasKey("PlayerPosition"))
            {
                string playerPosStr = PlayerPrefs.GetString("PlayerPosition");
                Debug.Log("Attempting to write on database PlayerPosition: " + playerPosStr);
                WriteDatabase(playerPosStr, "PlayerPosition");
            }
            if (PlayerPrefs.HasKey("SkillNames"))
            {

                string sn = PlayerPrefs.GetString("SkillNames");
                /*
                allSkillNamesWrapper = JsonConvert.DeserializeObject<SkillNames2>(sn);

                int skillItemsAmount = allSkillNamesWrapper.allNames.Count;
                for(int i=0; i<skillItemsAmount; i++)
                {
                    WriteDatabase(allSkillNamesWrapper.allSkillTiers[i], allSkillNamesWrapper.allNames[i]);
                    Debug.Log("Attempting to write on database SkillNames: " + allSkillNamesWrapper.allNames[i] + allSkillNamesWrapper.allSkillTiers[i]);
                }
                */
                Debug.Log("Attempting to write on database SkillNames: " + sn);
                WriteDatabase(sn, "SkillNames");
            }
            if (PlayerPrefs.HasKey("level"))
            {
                int level = PlayerPrefs.GetInt("level");
                Debug.Log("Attempting to write on database: level: " + level);
                WriteDatabase(level, "level");
            }
        }
    }
    public async void LoadDataFromDatabase()
    {
        //  -> Call ReadDatabaseAsync for all values
        //  -> Check all value types, if already present then override, else create new (Do this for all necessary required values)

        //  -> Set the matching prefabs according to the values
        //  -> Logs for Debugging

        //  -> this will run after the authentication has been completed and loading from Firebase will not be done in the next scene
        Debug.Log("inside LoadDataOnFirebase");
        //if (PlayerPrefs.HasKey("Username"))
        {
            object username1 = await ReadDatabaseAsync("Username");
            if (TryConvertToString(username1, out string username2))
            {
                PlayerPrefs.SetString("Username", username2);
            }
            else
            {
                Debug.Log("Failed to convert 'username2' to string.");
            }
        }
        //if (PlayerPrefs.HasKey("inventorySize"))
        {
            object inventorySizeObject = await ReadDatabaseAsync("inventorySize");
            if (TryConvertToInt(inventorySizeObject, out int inventorySize))
            {
                PlayerPrefs.SetInt("inventorySize", inventorySize);
            }
            else
            {
                Debug.Log("Failed to convert 'inventorySize' to int.");
            }
        }

        //if (PlayerPrefs.HasKey("inventory"))
        {
            object inventoryObject = await ReadDatabaseAsync("inventory");
            if (TryConvertToString(inventoryObject, out string inventoryData))
            {
                PlayerPrefs.SetString("inventory", inventoryData);
            }
            else
            {
                Debug.Log("Failed to convert 'inventory' to string.");
            }
        }

        {
            object PortalQuantityData = await ReadDatabaseAsync("PortalQuantity");
            if (TryConvertToInt(PortalQuantityData, out int PortalQuantity))
            {
                if (PortalQuantity > 0 && PortalQuantity < 1000)
                {

                    PlayerPrefs.SetInt("PortalQuantity", PortalQuantity);
                }
                else
                {
                    PlayerPrefs.SetInt("PortalQuantity", 0);
                }
            }
            else
            {
                Debug.Log("Failed to convert 'PortalQuantity' to int.");
            }

        }
        //if (PlayerPrefs.HasKey("constructionsystem"))
        for (int i = -1; i < 23; i++)
        {
            string key = "constructionsystem_" + i;
            object constructionObject = await ReadDatabaseAsync(key);
            if (TryConvertToString(constructionObject, out string constructionData))
            {
                PlayerPrefs.SetString(key, constructionData);
            }
            else
            {
                Debug.Log("Failed to convert " + key + " to string.");
            }
        }

        //if (PlayerPrefs.HasKey("SkillNames"))
        {
            object skillsObject = await ReadDatabaseAsync("SkillNames");
            if (TryConvertToString(skillsObject, out string skillsData))
            {
                PlayerPrefs.SetString("SkillNames", skillsData);
            }
            else
            {
                Debug.Log("Failed to convert 'SkillNames' to string.");
            }
        }

        //if (PlayerPrefs.HasKey("Player PRofile Manager"))
        {
            object profileManagerObject = await ReadDatabaseAsync("Player PRofile Manager");
            if (TryConvertToString(profileManagerObject, out string profileManagerData))
            {
                PlayerPrefs.SetString("Player PRofile Manager", profileManagerData);
            }
            else
            {
                Debug.Log("Failed to convert 'Player PRofile Manager' to string.");
            }
        }



        {
            object goldData = await ReadDatabaseAsync("gold");
            if (TryConvertToInt(goldData, out int gold))
            {
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
                Debug.Log("Failed to convert 'gold' to int.");
            }

        }
        {
            object levelData = await ReadDatabaseAsync("level");
            if (TryConvertToInt(levelData, out int level))
            {
                if (level >= -1 && level < 100)
                {

                    PlayerPrefs.SetInt("level", level);
                }
                else
                {
                    PlayerPrefs.SetInt("level", -1);
                }
            }
            else
            {
                Debug.Log("Failed to convert 'level' to int.");
            }

        }
        {
            object hasPlayedTutorialData = await ReadDatabaseAsync("HasPlayedTutorial");
            if (TryConvertToInt(hasPlayedTutorialData, out int hasPlayedTutorial))
            {
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
                Debug.Log("Failed to convert 'hasPlayedTutorial' to int.");
            }

        }
        //if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            object playerPosObj = await ReadDatabaseAsync("PlayerPosition");
            if (TryConvertToString(playerPosObj, out string playerPosStr))
            {
                PlayerPrefs.SetString("PlayerPosition", playerPosStr);
            }
            else
            {
                Debug.Log("Failed to convert 'PlayerPosition' to string.");
            }
        }
        //if (PlayerPrefs.HasKey("Player Attributes System"))
        {
            object attributeSystemObject = await ReadDatabaseAsync("Player Attributes System");
            if (TryConvertToString(attributeSystemObject, out string attributeSystemData))
            {
                PlayerPrefs.SetString("Player Attributes System", attributeSystemData);
            }
            else
            {
                Debug.Log("Failed to convert 'Player Attributes System' to string.");
            }
        }
        //if (PlayerPrefs.HasKey("achievSkill"))
        {
            object achievSkillObject = await ReadDatabaseAsync("achievSkill");
            if (TryConvertToString(achievSkillObject, out string achievSkillData))
            {
                PlayerPrefs.SetString("achievSkill", achievSkillData);
            }
            else
            {
                Debug.Log("Failed to convert 'achievSkill' to string.");
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
        //     Debug.Log("Failed to convert 'gold' to int.");
        // }
        Debug.Log("Retreived the following Data: \ninventorySize:" + PlayerPrefs.GetInt("inventorySize")
                + ", \ninventory:" + PlayerPrefs.GetString("inventory")
                + ", \nconstructionsystem:" + PlayerPrefs.GetString("constructionsystem")
                + ", \nPlayer Attributes System:" + PlayerPrefs.GetString("Player Attributes System")
                + ", \nPlayer PRofile Manager" + PlayerPrefs.GetString("Player PRofile Manager")
                + ", \nachievSkill" + PlayerPrefs.GetString("achievSkill"));


        //Only proceed to the next scene once all the data has loaded
        if (SceneManager.GetActiveScene().name != "Survival Stories")
        {

            // Load "Scene1"
            LoadScene();

        }
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


    
}
[System.Serializable]
public class myMUser
{
    public string UserId;
    public string Email;
    public string DisplayName;
}