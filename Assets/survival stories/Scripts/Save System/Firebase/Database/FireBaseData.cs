using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SocialPlatforms;
using System;
using System.Data;
using System.Threading.Tasks;
using Unity.VisualScripting;

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
        Debug.Log("Getting " + dataTitle);
        reference.Child("users").Child(userId).Child(dataTitle).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Got snapshot");
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
                Debug.Log("Result1 is " + result);
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
            Debug.Log("Result is " + result);
            return result;
        }
    }
    public Task<object> ReadDirectDatabaseAsync(string userId, string dataTitle)
    {
        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        Debug.Log("Getting " + dataTitle);
        reference.Child("users").Child(userId).Child(dataTitle).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Got snapshot");


                Debug.Log("task.Result: \ntask.Result: " + task.Result + "\ntask.Result.isUnityNull: " + task.Result.IsUnityNull()
                    + "\ntask.Result.Value:-" + task.Result.Value + "-task.Result.Value == null " );
                
                if (task.Result.Value == null)
                {
                    Debug.Log("value is null");
                    tcs.SetResult(null);
                }
                else
                {
                    Debug.Log("value is NOT null");
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("snapshot is: " + snapshot.Value);
                    object rawData = snapshot.Value;

                    Type dataType = snapshot.Value.GetType();

                    //Debug.Log("Snapshot of " + dataTitle + " is: Type:" + dataType + " Value:" + rawData);
                    if (dataType == typeof(string))
                    {
                        string data = (string)rawData;
                        tcs.SetResult(data);
                    }
                    else if (dataType == typeof(long))
                    {
                        long data = (long)rawData;
                        tcs.SetResult(data);
                    }
                    else
                    {
                        object data = (object)rawData;
                        tcs.SetResult(data);
                    }
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error reading data: " + task.Exception);
                tcs.SetResult(null);
            }
        });

        return tcs.Task;
    }
    public async void ReadTestData()
    {
        object result = await ReadDirectDatabaseAsync("896dGp2direZew6A1kwP1DG4zzv2", "Player PRofile Manager");
        Debug.Log("Player PRofile Manager: " + result);

        result = await ReadDirectDatabaseAsync("896dGp2direZew6A1kwP1DG4zzv2", "gold");
        Debug.Log("gold: " + result);

        result = await ReadDirectDatabaseAsync("896dGp2direZew6A1kwP1DG4zzv2", "test1");
        Debug.Log("test2: " + result);

        {
            /*
            reference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Got snapshot");
                    DataSnapshot snapshot = task.Result;
                    var rawData = snapshot.Value;

                    Type dataType = snapshot.Value.GetType();

                    if (dataType == typeof(string))
                    {
                        string data = (string)rawData;
                        result = data;
                    }
                    else if (dataType == typeof(long))
                    {
                        long data = (long)rawData;
                        result = data.ToString();
                    }
                    else
                    {
                        object data = (object)rawData;
                        result = data.ToString();
                    }
                    Debug.Log("Result1 is " + result);
                    //Debug.Log("Read data from database:: Title: " + dataTitle + " Data: " + result + " Type: " + dataType);

                }
                else if (task.IsFaulted)
                {

                    Debug.LogError("Error reading data: " + task.Exception);
                    result = null;
                }
            });
            */
        }
        if (result == null)
        {
            
            Debug.Log("result is null");
        }
        else
        {
            Debug.Log("Final Result is " + result);
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
