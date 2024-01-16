using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class TutorialManager : MonoBehaviour
{
    int step = 0;
    bool doOnce = true;
    int hasPlayedTutorial = 0;

    public TMP_Text tutorialText;
    public GameObject highlight;
    public GameObject tutorialPanel;
    public Button OKbutton;
    [SerializeField] int buttonCount;
    public TMP_Text debugTextLog;

    public GameObject mainTutorialTextBox;
    public TMP_Text mainTutorialText;

    [SerializeField] Button HidePanelButton;

    [SerializeField] Button ProfilePanelIcon;
    [SerializeField] Button InventoryPanelIcon;
    [SerializeField] Button SkillsPanelIcon;
    [SerializeField] Button MarketPanelIcon;

    [SerializeField] GameObject ProfilePanel;
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject SkillsPanel;
    [SerializeField] GameObject MarketPanel;

    [SerializeField] GameObject CircleHighlight;

    bool hasClickedProfileIcon = false;
    bool hasClickedInventoryIcon = false;
    bool hasClickedSkillsIcon = false;
    bool hasClickedMarketIcon = false;

    [SerializeField] GameObject Controller;
    [SerializeField] GameObject DarkOverlay;

    [SerializeField] private List<MenuButton> buttons;

    [SerializeField] Button hidePanelButton;

    [SerializeField] NPCTutorial npcTutorialScript;

    public CraftableItemData SpiritAxe;

    void Start()
    {
        buttonCount = 0;
        // Add a listener to the button click event
        OKbutton.onClick.AddListener(OnButtonClick);

        // Update the count text
        UpdateCountText();

        // Enable the button initially
        EnableButton();
        hasPlayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial", 0);
        
        if (hasPlayedTutorial == 0)
        {
            StartTutorial();
        }
        else
        {
            npcTutorialScript.TurnOffNPC();
        }

        

    }
    void Update()
    {
        if ((InventoryPanel.activeInHierarchy || buttonCount == 3) && doOnce && step == 1)
        {
            if(buttonCount == 3)
            {
                InventoryPanelIcon.onClick.Invoke();
            }
            else if (InventoryPanel.activeInHierarchy)
            {
                OKbutton.onClick.Invoke();
            }
            doOnce = false;
            StopHighlightCircle();
            TurnOffTextandOKButton();
            //And here is where you can craft or build stuff
            StartCoroutine(Sequence_2());
            step++;
        }

        if (ProfilePanel.activeInHierarchy && doOnce && step == 2)
        {
            doOnce = false;
            StopHighlightCircle();

            StartCoroutine(Sequence_3_1());
            
            step++;
        }
        if (SkillsPanel.activeInHierarchy && doOnce && step == 3)
        {
            doOnce = false;
            StopHighlightCircle();

            StartCoroutine(Sequence_4_1());

            step++;
        }
        if (MarketPanel.activeInHierarchy && doOnce && step == 4)
        {
            doOnce = false;
            StopHighlightCircle();

            StartCoroutine(Sequence_5_1());

            step++;
        }
    }
    IEnumerator Sequence_1()
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText1();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        npcTutorialScript.MainCharSpeechBox_ON();
        {
            npcTutorialScript.MainCharDot();
            yield return StartCoroutine(WaitForSecondsCoroutine(2));
        }
        npcTutorialScript.MainCharSpeechBox_OFF();

        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText2();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        npcTutorialScript.MainCharSpeechBox_ON();
        {
            npcTutorialScript.MainCharDot();
            yield return StartCoroutine(WaitForSecondsCoroutine(2));
        }
        npcTutorialScript.MainCharSpeechBox_OFF();

        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText3();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();


        OKbutton.onClick.Invoke();
    }

    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    IEnumerator Sequence_2()
    {
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        PointToCraftNBuild();
    }

    IEnumerator Sequence_3()//profile panel
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText6();
            PointArrowToXP();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        //arrow.SetActive(false);
        doOnce = true;
        ClickOnThing(ProfilePanelIcon, ProfilePanel, 100, -100);

    }
    IEnumerator Sequence_4()//skills
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText7();
            PointArrowToSkills();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        //arrow.SetActive(false);

        doOnce = true;
        ClickOnThing(SkillsPanelIcon, SkillsPanel, -75, -100);

    }
    IEnumerator Sequence_5()
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText8();
            PointArrowToMarket();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        //arrow.SetActive(false);
        doOnce = true;
        ClickOnThing(MarketPanelIcon, MarketPanel, -75, -100);

    }
    IEnumerator Sequence_6()
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText9();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();


        npcTutorialScript.MainCharSpeechBox_ON();
        {
            npcTutorialScript.MainCharOKAY();

            CraftingSystem.CraftButtonClicked(SpiritAxe);

            yield return StartCoroutine(WaitForSecondsCoroutine(2));
        }
        npcTutorialScript.MainCharSpeechBox_OFF();


        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText10();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();

        //___________________________________________________________________Tutorial Ends Here
        Controller.SetActive(true);
        npcTutorialScript.TurnOffNPC();
        PlayerPrefs.SetInt("HasPlayedTutorial", 1);
    }
    IEnumerator Sequence_3_1()
    {
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        mainTutorialText.text = "You can see all your stats here\nPress OK to continue";
        TurnOnTextandOKButton();

    }
    IEnumerator Sequence_4_1()
    {
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        mainTutorialText.text = "\n\n\nRemember to use your Skill Points wisely\nPress OK to continue";
        TurnOnTextandOKButton();

    }
    IEnumerator Sequence_5_1()
    {
        yield return StartCoroutine(WaitForSecondsCoroutine(2));
        mainTutorialText.text = "\n\n\nLookout for trades with good payoffs\nPress OK to continue";
        TurnOnTextandOKButton();

    }
    private void OnButtonClick()
    {
        // Increment the button count
        buttonCount++;

        // Update the count text
        UpdateCountText();
        //DisableButton();

        if (buttonCount == 1)
        {
            tutorialPanel.SetActive(false);
            OKbutton.gameObject.SetActive(false);

            StartCoroutine(Sequence_1());


        }
        else if (buttonCount == 2)
        {

            ClickOnThing(InventoryPanelIcon, InventoryPanel, -75, -100);

            TurnOnTextandOKButton();
            mainTutorialText.text = "This is where you keep stuff. \nClick OK to Continue";


            //--------------------------------------
            //tutorialPanel.SetActive(false);
            //OKbutton.gameObject.SetActive(false);
            //ClickOnThing(ProfilePanelIcon, ProfilePanel, 100, -100);

            //someEvent when ProfileIcon is clicked

        }
        else if(buttonCount == 4)
        {
            HidePanelButton.onClick.Invoke();
            TurnOffTextandOKButton();
            Controller.SetActive(false);
            CraftnBuildOBJ.SetActive(false);

            //You get smarter at adventuring the more you do stuff"
            //___________________________________________________Inventory Ends, Profile STarts


            StartCoroutine(Sequence_3());
            

        }
        else if(buttonCount == 5)
        {
            arrow.SetActive(false);
            ProfilePanel.SetActive(false);
            TurnOffTextandOKButton();
            CloseButton();

            //___________________________________Profile ENds, Skills Start

            StartCoroutine(Sequence_4());

        }
        else if(buttonCount == 6)
        {
            arrow.SetActive(false);
            HidePanelButton.onClick.Invoke();
            TurnOffTextandOKButton();
            Controller.SetActive(false);

            //___________________________________________Skills End, Market STart

            StartCoroutine(Sequence_5());

        }
        else if(buttonCount == 7)
        {
            arrow.SetActive(false);
            HidePanelButton.onClick.Invoke();
            TurnOffTextandOKButton();
            Controller.SetActive(false);

            //___________________________________________Skills End, Market STart

            StartCoroutine(Sequence_6());
        }
        
    }

    private void UpdateCountText()
    {
        // Update the UI text to display the current count
        //debugTextLog.text = "Button Count: " + buttonCount;
    }

    private void EnableButton()
    {
        // Enable the button
        OKbutton.interactable = true;
    }

    private void DisableButton()
    {
        // Disable the button
        OKbutton.interactable = false;
    }

    
    private void StartTutorial()
    {
        Controller.SetActive(false);
        tutorialPanel.SetActive(true);
        OKbutton.gameObject.SetActive(true);
        step++;
    }
    public int xOffset;
    public int yOffset;
    private void ClickOnThing(Button icon, GameObject panel, int xAdd, int yadd)
    {
        CircleHighlight.transform.position = new Vector3(icon.transform.position.x+xAdd, icon.transform.position.y+yadd, icon.transform.position.z);
        CircleHighlight.SetActive(true);

    }

    [SerializeField] GameObject CraftnBuildOBJ;
    public void PointToCraftNBuild()
    {
        CraftnBuildOBJ.SetActive(true);
        mainTutorialText.text = "And here is where you can craft or build stuff\nPress OK to continue";
        TurnOnTextandOKButton();
    }

    public void StopHighlightCircle()
    {
        CircleHighlight.SetActive(false);
    }
    public void TurnOnTextandOKButton()
    {
        mainTutorialTextBox.SetActive(true);
        OKbutton.gameObject.SetActive(true);
    }
    public void TurnOffTextandOKButton()
    {
        mainTutorialTextBox.SetActive(false);
        OKbutton.gameObject.SetActive(false);
    }
    public void CloseButton()
    {
        Controller.SetActive(true);
        DarkOverlay.SetActive(false);
    }

    public GameObject arrow;
    public void PointArrowToXP()
    {
        arrow.transform.position = new Vector3(ProfilePanelIcon.transform.position.x + 300, ProfilePanelIcon.transform.position.y + -600, ProfilePanelIcon.transform.position.z);
        arrow.SetActive(true);
    }

    public void PointArrowToSkills()
    {
        arrow.transform.position = new Vector3(SkillsPanelIcon.transform.position.x + -500, SkillsPanelIcon.transform.position.y -400, SkillsPanelIcon.transform.position.z);
        arrow.transform.rotation = Quaternion.Euler(0, 0, 135);
        arrow.SetActive(true);
    }

    public void PointArrowToMarket()
    {
        arrow.transform.position = new Vector3(MarketPanelIcon.transform.position.x + -500, MarketPanelIcon.transform.position.y - 400, MarketPanelIcon.transform.position.z);
        arrow.transform.rotation = Quaternion.Euler(0, 0, 135);
        arrow.SetActive(true);
    }

    //________________________________________________________________________________________________________________________________________________
    

    public void SecondTutorialTrigger()
    {
        if(PlayerPrefs.GetInt("HasPlayedTutorialExt", 0) == 0)
        {
            Debug.Log("Starting Second tutorial");
            npcTutorialScript.AllignNPC();
            npcTutorialScript.TurnOnNPC();

            StartCoroutine(Sequence_7());
        }
    }
    /*
     Second turoeial
     */
    IEnumerator Sequence_7()
    {
        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText11();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();


        npcTutorialScript.MainCharSpeechBox_ON();
        {
            npcTutorialScript.MainCharDot();
            yield return StartCoroutine(WaitForSecondsCoroutine(2));
        }
        npcTutorialScript.MainCharSpeechBox_OFF();


        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText12();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();


        npcTutorialScript.MainCharSpeechBox_ON();
        {
            npcTutorialScript.MainCharDot();
            yield return StartCoroutine(WaitForSecondsCoroutine(2));
        }
        npcTutorialScript.MainCharSpeechBox_OFF();


        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText13();
            //pockt portal
            PocketPortal.instance.IncreaseQuantity();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();


        npcTutorialScript.NPCSpeechBox_ON();
        {
            npcTutorialScript.NPCText14();
            yield return StartCoroutine(WaitForSecondsCoroutine(5));
        }
        npcTutorialScript.NPCSpeechBox_OFF();



        //___________________________________________________________________Tutorial Ends Here
        Controller.SetActive(true);
        npcTutorialScript.TurnOffNPC();
        PlayerPrefs.SetInt("HasPlayedTutorialExt", 1);
    }



}
