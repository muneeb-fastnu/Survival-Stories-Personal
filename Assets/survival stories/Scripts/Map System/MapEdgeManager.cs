using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapEdgeManager : MonoBehaviour
{
    public GameObject mainCharacter;
    public Vector3 defaultPlayerPosition;
    public static MapEdgeManager instance;

    public TutorialManager tutorialManager;

    public static int LevelNum = -1;

    public TextMeshProUGUI MapTXT;

    public TextMeshProUGUI countDownTXT;

    public GameObject uiBlocker;

    public int mapLimit;
    public SurroundingCheck surroundingCheck;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
    }
    //top right 14.4 = x
    //top right 8.5 = y
    //bottom left = 
    void Start()
    {
        Debug.Log("LevelNum start" + LevelNum);
        if (PlayerPrefs.GetInt("HasPlayedTutorial", 0) != 0)
        {
            LoadData();
        }
        InvokeRepeating(nameof(SaveData), 1, 1);
    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            Debug.Log("LevelNum before perf" + LevelNum);
            int levelPerf = PlayerPrefs.GetInt("level",-1);
           
            LevelNum = levelPerf;
            Debug.Log("LevelNum after perf" + LevelNum);

            //MapRandomizer.instance.GoHome();
            if (LevelNum > -1)
            {
                MapRandomizer.instance.GetExpectedIndices((LevelNum - 1), out MapRandomizer.instance.baseIndex, out MapRandomizer.instance.decorIndex);
                //for (int i=0; i<LevelNum; i++)
                {
                    Debug.Log("Not home, outside");
                    MapRandomizer.instance.Randomize();
                }
            }
            else
            {
                Debug.Log("Home, a place i can go");
            }
            //MapRandomizer.instance.GetExpectedIndices(LevelNum, out MapRandomizer.instance.baseIndex, out MapRandomizer.instance.decorIndex);

        }
    }
    
    public void SaveData()
    {
        //Debug.Log("LevelNum rep" + LevelNum);
        PlayerPrefs.SetInt("level", LevelNum);
    }
    // Update is called once per frame
    void Update()
    {
        if(HasTouchedEdge)
        {
            if(!CircleZoom.instance.triggered)
            {
                HasTouchedEdge = false;
                CircleZoom.instance.ResetCircleSize();
                GoToNewMap();
                //---
            }
        }

        if (LevelNum == -1)
        {
            MapTXT.text = "Home ";
        }
        else
        {
            MapTXT.text = "Map: " + (LevelNum+1);
        }
    }

    public bool HasTouchedEdge = false;
    bool homeCheckOnce = true;
    public void SetHomeCheckOnce(bool b)
    {
        homeCheckOnce = b;
    }
    public void PlayerTouchedEdge()
    {
        if(LevelNum >= mapLimit && homeCheckOnce)
        {
            homeCheckOnce = false;
            MapEdgeManager.instance.uiBlocker.SetActive(true);
            MapEdgeManager.instance.surroundingCheck.EnemiesInTrigger.Clear();
            PocketPortal.instance.PortalAnimation();
            //mainCharacter.transform.position = defaultPlayerPosition;
            return;
        }
        if (surroundingCheck == null)
        {
            Debug.Log("MEM: null surround check");
            surroundingCheck = GameObject.FindObjectOfType<SurroundingCheck>();
        }
        surroundingCheck.EnemiesInTrigger.Clear();
        uiBlocker.SetActive(true);
        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = false;
        InventorySystem.instance.isIdle = true;

        Debug.Log("Just touched edge");
        tutorialManager.SecondTutorialTrigger();
        Debug.Log("Out of tutorial");
        if (PlayerPrefs.GetInt("HasPlayedTutorialExt", 0) == 0) 
        {
            StartCoroutine(WaitForSecondsCoroutine(25));
        }
        else
        {
            HasTouchedEdge = true;
            CircleZoom.instance.CircleZoomTrigger();
        }
        
    }
    public void GoToNewMap()
    {
        
        if (LevelNum < mapLimit)
        {
            StartCoroutine(CountDownFor());
            mainCharacter.transform.position = defaultPlayerPosition;
            LevelNum++;
            MapRandomizer.instance.Randomize();
        }
        else
        {

            //PocketPortal.instance.GoToHomeMap();
            //mainCharacter.transform.position = defaultPlayerPosition;
        }
    }
    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HasTouchedEdge = true;
        CircleZoom.instance.CircleZoomTrigger();
    }
    public static int GetLevelNum()
    {
        return LevelNum;
    }
    public static void SetLevelNum(int n)
    {
        LevelNum = n;
    }

    public IEnumerator CountDownFor()
    {
        
        countDownTXT.gameObject.SetActive(true);
        countDownTXT.text = "5";
        yield return new WaitForSeconds(1);
        countDownTXT.text = "4";
        yield return new WaitForSeconds(1);
        countDownTXT.text = "3";
        yield return new WaitForSeconds(1);
        countDownTXT.text = "2";
        yield return new WaitForSeconds(1);
        countDownTXT.text = "1";
        yield return new WaitForSeconds(1);
        countDownTXT.gameObject.SetActive(false);
        PocketPortal.instance.CharacterAIReset();
        uiBlocker.SetActive(false);

        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = true;
        InventorySystem.instance.isIdle = false;
        
        StartCoroutine(KeyPress.instance.TriggerKey());
    }
}