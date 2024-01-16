using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Unity.VisualScripting;

public class MyGoogleSignIn : MonoBehaviour
{
    public TMP_Text userInfoText;

    private FirebaseAuth fauth;
    private FirebaseApp app;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                app = FirebaseApp.DefaultInstance;
                fauth = FirebaseAuth.DefaultInstance;

                if (fauth.CurrentUser != null)
                {
                    DisplayUserInfo(fauth.CurrentUser);
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
        });
    }

    public void SignInWithGoogle()
    {
        if (fauth == null)
        {
            Debug.LogError("Firebase not initialized.");
            return;
        }

        
        Firebase.Auth.Credential credential =
            Firebase.Auth.GoogleAuthProvider.GetCredential("1046390475413-4a7kp5sj1v9ihg56hj1mn41nbq06q9nm.apps.googleusercontent.com", null);

        Debug.Log("Provider is "+credential.Provider);
        

        fauth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            //DisplayUserInfo(newUser);
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
        }
        );
    }

    public void SignOut()
    {
        fauth.SignOut();
        ClearUserInfo();
    }

    private void DisplayUserInfo(Firebase.Auth.FirebaseUser user)
    {
        userInfoText.text = $"Welcome, {user.DisplayName}\nEmail: {user.Email}\nUID: {user.UserId}";
    }

    private void ClearUserInfo()
    {
        userInfoText.text = "Not signed in.";
    }
    public void GoogleSignInButton2()
    {
        Firebase.Auth.Credential credential =
        Firebase.Auth.GoogleAuthProvider.GetCredential("1046390475413-4a7kp5sj1v9ihg56hj1mn41nbq06q9nm.apps.googleusercontent.com", null);
        Debug.Log("Credential: " + credential + " :: " + credential.Provider);
        Debug.Log("App: " + app + " \t App Name: " + app.Name);
        fauth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
}
