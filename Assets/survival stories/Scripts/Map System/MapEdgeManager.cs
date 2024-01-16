using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdgeManager : MonoBehaviour
{
    public GameObject mainCharacter;
    public Vector3 defaultPlayerPosition;
    public static MapEdgeManager instance;

    public TutorialManager tutorialManager;
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
                PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = true;
                InventorySystem.instance.isIdle = false;
            }
        }
    }

    public bool HasTouchedEdge = false;

    public void PlayerTouchedEdge()
    {
        

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
        mainCharacter.transform.position = defaultPlayerPosition;
    }
    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HasTouchedEdge = true;
        CircleZoom.instance.CircleZoomTrigger();
    }
}
