using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndSur : MonoBehaviour
{

    public static GameEndSur instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);  
    }  
    public void MainMenu()
    {
        SceneManager.LoadScene(0);  
    }
}
    