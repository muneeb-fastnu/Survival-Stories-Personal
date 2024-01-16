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


public class FirebaseSignIn : MonoBehaviour
{
    public string GoogleWebAPI = "1046390475413-4a7kp5sj1v9ihg56hj1mn41nbq06q9nm.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public TMP_Text UsernameTXT, UserEmailTXT;
    public Image UserProfilePicture;
    public string imageURL;
    public GameObject LoginScreen, ProfileScreen;

    [SerializeField] private FireBaseData firebaseData;
    private void Awake()
    {
        InitFirebase();
        firebaseData.Initialize();
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true,
        };
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InvokeRepeating(nameof(SaveDataOnFireBase), 20f, 10f);
    }
    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void GoogleSignInClick()
    {
        Debug.Log("Signin Button Clicked");
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }
    void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        Debug.Log("Continue with OnGoogleAuthentication STARTED");
        if(task.IsFaulted)
        {
            Debug.Log("Faulted:  " + task.Exception.ToString());
            Debug.LogError("Faulted:  " + task.Exception.ToString());
        }
        else if(task.IsCanceled) 
        {
            Debug.Log("Cancelled:  ");
            Debug.LogError("Cancelled:  ");
        }
        else
        {
            Debug.Log("Not faulted and Not cancelled");
            Debug.Log("task.Result.IdToken1: " + task.Result.IdToken);
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            Debug.Log("task.Result.IdToken2: " + task.Result.IdToken);
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Trying to sign in with credential Asunc");
                if (task.IsCanceled)
                {
                    Debug.Log("SignInWithCredentialAsync was canceled.");
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.Log("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }
                user = auth.CurrentUser;

                string[] nameParts = user.DisplayName.Split(' ');

                string firstName = (nameParts.Length > 0) ? nameParts[0] : "";
                PlayerPrefs.SetString("Username", firstName);

                UsernameTXT.text = "Player ID: " + user.Email;

                //Testing*****************************************************************************************************************
                WriteDatabase("testData", "Test");

                Debug.Log("Attempting to read from database.");



                Debug.Log("Title: Test \t| Data: " + ReadDatabase("Test"));
                Debug.Log("Title: inventorySize \t| Data: " + ReadDatabase("inventorySize"));
                Debug.Log("Title: inventory \t| Data: " + ReadDatabase("inventory"));
                //Testing*****************************************************************************************************************


                if (SceneManager.GetActiveScene().name != "Survival Stories")
                {
                    // Load "Scene1"
                    SceneManager.LoadScene(1);
                }

            });
            
        }
    }

    void LoadScene()
    {

    }

    private string CheckImageUrl(string url)
    {
        if(!string.IsNullOrEmpty(url))
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
        if (auth != null)
        {
            auth.SignOut();
            user = null;

            // Add any additional actions you want to perform after signing out, such as UI changes.
            UsernameTXT.text = "Not signed in";
            UserEmailTXT.text = "";
            UserProfilePicture.sprite = null;

            if (LoginScreen && ProfileScreen)
            {
                LoginScreen.SetActive(true);
                ProfileScreen.SetActive(false);
            }

            Debug.Log("User signed out.");
        }
        else
        {
            Debug.LogError("Firebase authentication is not initialized.");
        }
    }

    public void WriteInDatabase()
    {
        if (user != null)
        {
            if(firebaseData == null)
            {
            }
            firebaseData.WriteStringData(user.UserId, "String text for backend 11", "Title");
        }
        else
        {
            Debug.LogError("User is not signed in. Unable to write to the database.");
        }
    }
    public void WriteDatabase(object data, string dataTitle)
    {
        if (user != null)
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
            firebaseData.WriteStringData(user.UserId, data, dataTitle);
        }
        else
        {
            Debug.LogError("User is not signed in. Unable to write to the database.");
        }
    }
    public object ReadDatabase(string dataTitle)
    {
        object result = null;

        if (user != null)
        {
            result = firebaseData.ReadDirectDatabase(user.UserId, dataTitle);
        }
        else
        {
            Debug.LogError("User is not signed in. Unable to read from the database.");
        }
        return result;
    }
    public void SaveDataOnFireBase()
    {
        Debug.Log("inside SaveDataOnFirebase");
        if(user != null)
        {
            string id = user.UserId;
            Debug.Log("user is not null and id is: " + user.UserId);
            if(PlayerPrefs.HasKey("inventorySize"))
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

            if(PlayerPrefs.HasKey("constructionsystem"))
            {
                string construction = PlayerPrefs.GetString("constructionsystem");
                Debug.Log("Attempting to write on database constructionsystem: " + construction);
                WriteDatabase(construction, "constructionsystem");
            }

            if (PlayerPrefs.HasKey("Player Attribute System"))
            {
                string pas = PlayerPrefs.GetString("Player Attribute System");
                Debug.Log("Attempting to write on database Player Attribute System: " + pas);
                WriteDatabase(pas, "Player Attribute System");
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
        }
    }
    public void LoadDataFromDatabase()
    {
        //  -> Call ReadDatabase for all values
        //  -> Check all value types, if already present then override, else create new (Do this for all necessary required values)

        //  -> Set the maching prefabs according to the values
        //  -> Logs for Debuggig

        //  -> this will run after the authentication has been completed and loading from firebase will not be done in the next scene
    }

    public Firebase.Auth.FirebaseUser GetUser()
    {
        if (user != null)
            return user;
        else
            return null;
    }
}
