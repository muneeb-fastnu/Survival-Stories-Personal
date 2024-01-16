using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool LoadorNot;
    static string str;


    public delegate void GameLostDelegate();
    public static event GameLostDelegate showLostPanel;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("LoadorNot"))
        {

            str = PlayerPrefs.GetString("LoadorNot");
            LoadorNot = JsonConvert.DeserializeObject<bool>(str);

        }
        else
        {
            LoadorNot = true;
        }
        DontDestroyOnLoad(this);
        showLostPanel += ShowLostPanelEnd;
    }
    public static void GameLost()
    {

        //  Time.timeScale = 0;
        // show game lost screen
        Debug.Log("GameOverr");
        LoadorNot = true;
        //LoadorNot = true;
        str = JsonConvert.SerializeObject(LoadorNot);
        PlayerPrefs.SetString("LoadorNot", str);
        showLostPanel?.Invoke();
        //InventorySystem.instance.gameOverScreen.SetActive(true);
        //Invoke(nameof(GameRestart), 5f
        GameRestart();
    }
    public void ShowLostPanelEnd()
    {
        Debug.Log("Game endddddddd");
    }
    
    public static void GameRestart()
    {
        //GameEndSur.instance.RestartGame();
        SceneManager.LoadScene(1);
    }
    public void GameStart()
    {

    }
}
