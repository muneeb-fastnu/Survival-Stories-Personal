using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SceneManagerSurvivalStories : MonoBehaviour
{

    public static void GoToGamePlayScene()
    {
        SceneManager.LoadScene(1);
    }
    public static void GoToMainMenuScene()
    {

        SceneManager.LoadScene(0);
    }

}
