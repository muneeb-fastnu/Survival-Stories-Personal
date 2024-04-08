using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class NewTutorial : MonoBehaviour
{
    public Button MainTutorialButton;
    public Button ActualTutorialButton;

    public int step = -1;
    private int hasPlayedTutorial = 0;

    [SerializeField] NPCTutorial npcTutorialScript;

    [SerializeField] GameObject Controller;
    [SerializeField] GameObject TutorialPanel;

    [SerializeField] GameObject halfOverlay;
    [SerializeField] GameObject FullOverlay;

    [SerializeField] GameObject DarkOverlay;

    [SerializeField] GameObject StartTut;

    public List<GameObject> XXX;

    public GameObject ManTextBox;
    public TMP_Text manTMP;
    public GameObject PlayerTextBox;
    public TMP_Text playerTMP;

    [SerializeField] GameObject CircleHighlight;

    public List<GameObject> XXXX;

    [SerializeField] Button HidePanelButton;

    [SerializeField] Button ProfilePanelIcon;
    [SerializeField] Button InventoryPanelIcon;
    [SerializeField] Button SkillsPanelIcon;
    [SerializeField] Button MarketPanelIcon;

    public List<GameObject> XXXXX;

    [SerializeField] GameObject ProfilePanel;
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject SkillsPanel;
    [SerializeField] GameObject MarketPanel;

    public List<GameObject> XXXXXX;

    [SerializeField] GameObject IconDialogue;
    [SerializeField] TMP_Text iconDialogueTXT;
    [SerializeField] GameObject rightArrow;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject TapAnywhere;

    [SerializeField] GameObject iconDiaglogueArrow;
    [SerializeField] TMP_Text iconArrowTXT;
    [SerializeField] GameObject iconDiaglogueArrownoArrow;

    public List<GameObject> XXXXXXX;
    [SerializeField] Button ProfilePanelClose;

    public CraftableItemData SpiritAxe;

    public List<GameObject> XXXXXXXX;

    [SerializeField] Button CraftingButton;
    [SerializeField] Button ConstructionButton;
    [SerializeField] GameObject CraftingButtonGO;
    [SerializeField] GameObject ConstructionButtonGO;

    public List<GameObject> XXXXXXXXX;
    public GameObject MainPlayer;
    [SerializeField] MainPlayerController mainPlayerController;
    [SerializeField] GameObject MainPlayerControllerSorroundCheck;

    public static bool inTutorial = false;

    void Start()
    {
        //mainPlayerController = MainPlayer.GetComponent<MainPlayerController>();

        MainTutorialButton.enabled = false;
        ActualTutorialButton.enabled = false;
        ActualTutorialButton.gameObject.SetActive(false);

        npcTutorialScript.TurnOffNPC();
        hasPlayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial", 0);
        //hasPlayedTutorial = 0;
        Debug.Log("HasPlayedTutorial script start");
        if (hasPlayedTutorial == 0)
        {
            Debug.Log("starting tutorial");
            //start tutorial
            StartTutorial();
        }
        else
        {
            Debug.Log("skipping tutorial");
            //skip tutorial
            npcTutorialScript.TurnOffNPC();
            Controller.SetActive(true);
            TutorialPanel.SetActive(false);
        }
    }
    public void StartTutorial()
    {
        Controller.SetActive(false);
        TutorialPanel.SetActive(true);
        step++;
        mainPlayerController.isPlayerAllowedMove = false;
        inTutorial = true;
    }
    public void EndTutorial()
    {
        Controller.SetActive(true);
        TutorialPanel.SetActive(false);
        step++;
        //hasplayedturoeial
        PlayerPrefs.SetInt("HasPlayedTutorial", 1);
        mainPlayerController.isPlayerAllowedMove = true;
        inTutorial = false;

    }
    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //usage: Indide another IEnumerator: yield return StartCoroutine(WaitForSecondsCoroutine(5));
    }
    // Update is called once per frame
    void Update()
    {

        


        if(step == 0)
        {
            StartCoroutine(Sequence_1());
        }

        if(step == 2)
        {
            StartCoroutine(Sequence_2());
        }

        if(step == 4)
        {
            StartCoroutine(Sequence_3());
        }

        if (step == 6)
        {
            StartCoroutine(Sequence_4());
        }
        if (step == 8)
        {
            StartCoroutine(Sequence_5());
        }
        if(step == 10)
        {
            StartCoroutine(Sequence_6());
        }
        if (step == 12)
        {
            StartCoroutine(Sequence_7());
        }
        if (step == 14)
        {
            StartCoroutine(Sequence_8());
        }
        if (step == 16)
        {
            StartCoroutine(Sequence_9());
        }
        if (step == 18)
        {
            StartCoroutine(Sequence_10());
        }
        if (step == 20)
        {
            StartCoroutine(Sequence_11());
        }
        if (step == 22)
        {
            CraftingSystem.CraftButtonClicked(SpiritAxe);
            EndTutorial();
        }

    }






    private void SetDialoguePosition(Button icon, GameObject dialogue, int xAdd, int yAdd)
    {
        int xOffset = xAdd - 375;
        int yOffset = yAdd - 50;
        dialogue.transform.position = new Vector3(icon.transform.position.x + xOffset, icon.transform.position.y + yOffset, icon.transform.position.z);
    }






    private void ClickOnThing(Button icon, GameObject panel, int xAdd, int yadd)
    {
        CircleHighlight.transform.position = new Vector3(icon.transform.position.x + xAdd, icon.transform.position.y + yadd, icon.transform.position.z);
        CircleHighlight.SetActive(true);
    }
    private void SetActualTutorialButtonPos(Button icon, int xAdd, int yadd)
    {
        ActualTutorialButton.gameObject.transform.position = new Vector3(icon.transform.position.x + xAdd, icon.transform.position.y + yadd, icon.transform.position.z);
    }
    public void StopHighlightCircle()
    {
        CircleHighlight.SetActive(false);
    }
    public void IncrementStepCount()
    {
        step++;
    }
    IEnumerator Sequence_1()
    {
        MainTutorialButton.gameObject.SetActive(true);
        ActualTutorialButton.gameObject.SetActive(false);
        step++; //Step 0 -> 1
        halfOverlay.SetActive(true);
        StartTut.SetActive(true);
        yield return StartCoroutine(WaitForSecondsCoroutine(5));
        StartTut.SetActive(false);
        step++; // Step 1 -> 2
    }

    IEnumerator Sequence_2()
    {
        step++; //Step 2 -> 3
        halfOverlay.SetActive(true);

        manTMP.text = "Ooh, you're new here";
        ManTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        ManTextBox.SetActive(false);
        playerTMP.text = "...";
        PlayerTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));
        PlayerTextBox.SetActive(false);
        manTMP.text = "I've been here for many years";
        ManTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        ManTextBox.SetActive(false);
        playerTMP.text = "...";
        PlayerTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        PlayerTextBox.SetActive(false);
        manTMP.text = "Let me teach you a thing or two";
        ManTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        ManTextBox.SetActive(false);

        StartTut.SetActive(false);
        step++; // Step 3 -> 4
    }

    IEnumerator Sequence_3()
    {
        step++; //Step 4 -> 5
        

        iconDialogueTXT.text = "This is where you keep stuff";
        SetDialoguePosition(InventoryPanelIcon, IconDialogue, -50, -75);
        //IconDialogue.transform.position = new Vector3(IconDialogue.transform.position.x + 100, IconDialogue.transform.position.y + 600, 0);
        
        IconDialogue.SetActive(true);
        halfOverlay.SetActive(false);
        ClickOnThing(InventoryPanelIcon, InventoryPanel, -50, -75);
        

        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        SetActualTutorialButtonPos(InventoryPanelIcon, -50, 0);

        TapAnywhere.SetActive(true);

        MainTutorialButton.enabled = true;
        ActualTutorialButton.gameObject.SetActive(true);
        ActualTutorialButton.enabled = true;

        //StopHighlightCircle();

        //
    }
    IEnumerator Sequence_4()
    {
        //Step 6
        step++; // Step 6 -> 7
        MainTutorialButton.enabled = false;
        ActualTutorialButton.enabled = false;
        ActualTutorialButton.gameObject.SetActive(false);

        TapAnywhere.SetActive(false);

        IconDialogue.SetActive(false);
        StopHighlightCircle();

        DarkOverlay.SetActive(true);

        InventoryPanelIcon.onClick.Invoke();
        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));
        iconArrowTXT.text = "And here is where you can Craft or Build stuff";
        iconDiaglogueArrow.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));
        iconDiaglogueArrownoArrow.SetActive(false);
        yield return StartCoroutine(WaitForSecondsCoroutine(1));
        CraftingButtonGO.SetActive(true);
        CraftingButton.onClick.Invoke();
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        CraftingButtonGO.SetActive(false);
        ConstructionButton.onClick.Invoke();
        ConstructionButtonGO.SetActive(true);
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        ConstructionButtonGO.SetActive(false);

        yield return StartCoroutine(WaitForSecondsCoroutine(0.5f));
        
        HidePanelButton.onClick.Invoke();
        iconDiaglogueArrownoArrow.SetActive(true);

        step++; // Step 7 -> 8
    }
    IEnumerator Sequence_5()
    {
        step++; //Step 8 -> 9
        halfOverlay.SetActive(false);

        iconArrowTXT.text = "You get smarter at adventuring the more you do stuff";

        rightArrow.transform.position = new Vector3(rightArrow.transform.position.x - 50, rightArrow.transform.position.y - 50, rightArrow.transform.position.z);
        rightArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        iconDiaglogueArrow.transform.position = new Vector3(iconDiaglogueArrow.transform.position.x + 100, iconDiaglogueArrow.transform.position.y + 400, 0);

        leftArrow.SetActive(false);
        iconDiaglogueArrow.SetActive(true);
        ClickOnThing(ProfilePanelIcon, ProfilePanel, 75, -75);
        

        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        //MainTutorialButton.enabled = false;
        SetActualTutorialButtonPos(ProfilePanelIcon, 75, 0);

        TapAnywhere.SetActive(true);

        MainTutorialButton.enabled = true;
        ActualTutorialButton.gameObject.SetActive(true);
        ActualTutorialButton.enabled = true;

        //StopHighlightCircle();

        //Step : 9
    }
    IEnumerator Sequence_6()
    {
        //Step 10
        step++; //Step 10 -> 11
        MainTutorialButton.enabled = false;
        ActualTutorialButton.enabled = false;
        ActualTutorialButton.gameObject.SetActive(false);

        TapAnywhere.SetActive(false);

        IconDialogue.SetActive(false);
        StopHighlightCircle();

        DarkOverlay.SetActive(true);

        ProfilePanelIcon.onClick.Invoke();
        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        leftArrow.SetActive(true);
        iconArrowTXT.text = "You can see all your stats here";

        rightArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        rightArrow.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        rightArrow.transform.position = new Vector3(rightArrow.transform.position.x + 50, rightArrow.transform.position.y + 50, rightArrow.transform.position.z);

        leftArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        iconDiaglogueArrow.transform.position = new Vector3(iconDiaglogueArrow.transform.position.x - 85, iconDiaglogueArrow.transform.position.y - 1000, 0);

        iconDiaglogueArrow.SetActive(true);
        yield return StartCoroutine(WaitForSecondsCoroutine(5));
        rightArrow.transform.localScale = new Vector3(1, 1, 1);
        iconDiaglogueArrow.SetActive(false);
        ProfilePanelClose.onClick.Invoke();


        step++; // Step 11 -> 12
    }
    IEnumerator Sequence_7()
    {
        step++; //Step 12 -> 13
        halfOverlay.SetActive(false);

        iconDialogueTXT.text = "You unlock special skills when you do stuff enough times";
        SetDialoguePosition(SkillsPanelIcon, IconDialogue, -50, -75);
        //IconDialogue.transform.position = new Vector3(IconDialogue.transform.position.x, IconDialogue.transform.position.y - 200, 0);

        IconDialogue.SetActive(true);
        ClickOnThing(SkillsPanelIcon, SkillsPanel, -50, -75);
        

        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        SetActualTutorialButtonPos(SkillsPanelIcon, -50, 0);

        TapAnywhere.SetActive(true);

        MainTutorialButton.enabled = true;
        ActualTutorialButton.gameObject.SetActive(true);
        ActualTutorialButton.enabled = true;

        //StopHighlightCircle();


        //Step : 13
    }
    IEnumerator Sequence_8()
    {
        //Step 14
        step++; //Step 14 -> 15
        MainTutorialButton.enabled = false;
        ActualTutorialButton.enabled = false;
        ActualTutorialButton.gameObject.SetActive(false);

        TapAnywhere.SetActive(false);

        IconDialogue.SetActive(false);
        StopHighlightCircle();

        DarkOverlay.SetActive(true);

        SkillsPanelIcon.onClick.Invoke();
        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        leftArrow.SetActive(true);
        rightArrow.transform.position = new Vector3(rightArrow.transform.position.x, rightArrow.transform.position.y + 1100, 0);
        leftArrow.transform.position = new Vector3(rightArrow.transform.position.x + 900, rightArrow.transform.position.y, 0);
        rightArrow.SetActive(true);
        iconArrowTXT.text = "Remember to use your Skill Points wisely";

        iconDiaglogueArrow.transform.position = new Vector3(iconDiaglogueArrow.transform.position.x, iconDiaglogueArrow.transform.position.y - 600 , 0);

        iconDiaglogueArrow.SetActive(true);
        yield return StartCoroutine(WaitForSecondsCoroutine(5));
        iconDiaglogueArrow.SetActive(false);
        HidePanelButton.onClick.Invoke();

        step++; // Step 15 -> 16
    }

    IEnumerator Sequence_9()
    {
        step++; //Step 16 -> 17
        halfOverlay.SetActive(false);

        iconDialogueTXT.text = "There are people here who would like stuff from you!";
        SetDialoguePosition(SkillsPanelIcon, IconDialogue, -50, -250);
        //IconDialogue.transform.position = new Vector3(IconDialogue.transform.position.x, IconDialogue.transform.position.y - 200, 0);

        IconDialogue.SetActive(true);
        ClickOnThing(MarketPanelIcon, MarketPanel, -50, -75);
        

        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        SetActualTutorialButtonPos(MarketPanelIcon, -50, 0);

        TapAnywhere.SetActive(true);

        MainTutorialButton.enabled = true;
        ActualTutorialButton.gameObject.SetActive(true);
        ActualTutorialButton.enabled = true;

        //StopHighlightCircle();

        //Step : 17
    }
    IEnumerator Sequence_10()
    {
        //Step 18
        step++; //Step 18 -> 19
        MainTutorialButton.enabled = false;
        ActualTutorialButton.enabled = false;
        ActualTutorialButton.gameObject.SetActive(false);

        TapAnywhere.SetActive(false);

        IconDialogue.SetActive(false);
        StopHighlightCircle();

        DarkOverlay.SetActive(true);

        MarketPanelIcon.onClick.Invoke();
        yield return StartCoroutine(WaitForSecondsCoroutine(0.1f));

        leftArrow.SetActive(true);
        rightArrow.SetActive(false);
        leftArrow.transform.position = new Vector3(leftArrow.transform.position.x - 100, leftArrow.transform.position.y - 800, 0);

        iconArrowTXT.text = "Lookout for trades with good payoffs";

        iconDiaglogueArrow.transform.position = new Vector3(iconDiaglogueArrow.transform.position.x + 100, iconDiaglogueArrow.transform.position.y + 200, 0);

        iconDiaglogueArrow.SetActive(true);
        yield return StartCoroutine(WaitForSecondsCoroutine(5));
        iconDiaglogueArrow.SetActive(false);
        HidePanelButton.onClick.Invoke();

        step++; // Step 19 -> 20
    }

    IEnumerator Sequence_11()
    {
        step++; //Step 20 -> 21
        halfOverlay.SetActive(true);

        manTMP.text = "And that's all. Here's an axe for you to start your journey";
        ManTextBox.SetActive(true);

        

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        ManTextBox.SetActive(false);
        playerTMP.text = "Look's Good";
        PlayerTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));
        PlayerTextBox.SetActive(false);
        manTMP.text = "Oh and be careful of the monsters!";
        ManTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        ManTextBox.SetActive(false);
        playerTMP.text = "...";
        PlayerTextBox.SetActive(true);

        yield return StartCoroutine(WaitForSecondsCoroutine(3));

        PlayerTextBox.SetActive(false);

        step++; // Step 21 -> 22
    }
}
