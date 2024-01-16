using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SocialPlatforms;
using System;

public class FireBaseData : MonoBehaviour
{
    
    public DatabaseReference reference;


    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            // Assuming you have a Firebase user signed in
            Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;

            if (user != null)
            {
                // Write a string to the database for the user
                //WriteStringData(user.UserId, "Hello, this is my data!");
            }
            Debug.Log("Firebase Database Initialized successfully" + reference + "  key:" + reference.Key + "  root: " + reference.Root);
        });
    }

    public void WriteStringData(string userId, object data, string dataTitle)
    {
        Debug.Log("Inside database WriteStringData");
        // Set the data at the specified path
        reference.Child("users").Child(userId).Child(dataTitle).SetValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data written to database!");
               
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error writing data: " + task.Exception);
                
            }
        });
    }

    public object ReadDirectDatabase(string userId, string dataTitle)
    {
        object result = null;

        reference.Child("users").Child(userId).Child(dataTitle).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                object rawData = snapshot.Value;

                Type dataType = snapshot.Value.GetType();

                if (dataType == typeof(string))
                {
                    string data = (string)rawData;
                    result = data;
                }
                else if(dataType == typeof(long))
                {
                    long data = (long)rawData;
                    result = data;
                }
                else
                {
                    object data = (object)rawData;
                    result = data;
                }
                
                //Debug.Log("Read data from database:: Title: " + dataTitle + " Data: " + result + " Type: " + dataType);
                
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error reading data: " + task.Exception);
                result = null;
            }
        });

        if (result == null)
        {
            return null;
        }
        else
        {
            return result;
        }
    }

    /*
    public void ReadDirectDatabase2<T>(string userId, string dataTitle, Action<T> onComplete)
    {
        reference.Child("users").Child(userId).Child(dataTitle).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    object rawData = snapshot.Value;

                    if (rawData != null)
                    {
                        Type dataType = rawData.GetType();

                        if (dataType == typeof(long))
                        {
                            // Firebase stores numeric values as long by default
                            long longData = (long)rawData;
                            onComplete((T)Convert.ChangeType(longData, typeof(T)));
                        }
                        else if (dataType == typeof(string))
                        {
                            string stringData = (string)rawData;
                            onComplete((T)Convert.ChangeType(stringData, typeof(T)));
                        }
                        // Add more type checks as needed
                        else
                        {
                            Debug.LogWarning("Unhandled data type: " + dataType);
                            onComplete(default);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Data is null");
                        onComplete(default);
                    }
                }
                else
                {
                    Debug.LogWarning("Snapshot does not exist");
                    onComplete(default);
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error reading data: " + task.Exception);
                onComplete(default);
            }
        });
    }
    */



}
