using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PocketPortal : MonoBehaviour
{
    public TMP_Text ppCount;
    public static PocketPortal instance;

    public GameObject mainCharacter;
    public Vector3 defaultPlayerPosition;

    bool HasPressedPortal = false;

    [SerializeField] CharacterBehaviour characterBehaviourScript;
    [SerializeField] AICharacterBehaviour aiCharacterBehaviour;

    public GameObject PPTime;
    public TextMeshProUGUI pptimeTXT;
    //public Button PPButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public int Quantity { get; private set; }
    private const int IncreaseIntervalHours = 2;

    private void Start()
    {
        LoadQuantity();
        InvokeRepeating(nameof(SaveQuantity), 1, 5);
        InvokeRepeating(nameof(IncreaseQuantity), IncreaseIntervalHours * 3600, IncreaseIntervalHours * 3600); // Invoke repeating every 2 hours
        InvokeRepeating(nameof(StartCountdownTimer), 0, IncreaseIntervalHours * 3600);
        
        //IncreaseQuantity();
    }

    void Update()
    {
        if (HasPressedPortal)
        {
            if (!CircleZoom.instance.triggered)
            {
                HasPressedPortal = false;
                Debug.Log("Starting pocket portal routine Pre");
                StartCoroutine(WaitForSecondsCoroutine(2f));
            }
        }
    }

    public void PortalAnimation()
    {
        Debug.Log("Portal Animation Started");
        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = false;
        InventorySystem.instance.isIdle = true;
        characterBehaviourScript = mainCharacter.GetComponent<CharacterBehaviour>();
        Debug.Log("Set target null before");
        //aiCharacterBehaviour.SetCurrentTargetNull();
        Debug.Log("Set target null after");

        HasPressedPortal = true;
        CircleZoom.instance.CircleZoomTrigger();
    }
    public void GoToHomeMap()
    {
        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = true;
        MapEdgeManager.SetLevelNum(-1);
        MapRandomizer.instance.GoHome();
        mainCharacter.transform.position = defaultPlayerPosition;
    }
    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        Debug.Log("Started pocket portal routine vefore wait 2 sec");

        yield return new WaitForSeconds(seconds);
        Debug.Log("Started pocket portal routine after wait 2 sec");

        HasPressedPortal = false;
        CircleZoom.instance.ResetCircleSize();
        
        GoToHomeMap();
        //CharacterAIReset();
        MapEdgeManager.instance.uiBlocker.SetActive(false);

        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = true;
        InventorySystem.instance.isIdle = false;
        Debug.Log("eNDED pocket portal routine");
        MapEdgeManager.instance.SetHomeCheckOnce(true);
    }
    public void CharacterAIReset()
    {
        aiCharacterBehaviour.CurrentTarget = null;
        characterBehaviourScript.currentTargetInfo = null;

        //characterBehaviourScript.StopNavAgent();
        characterBehaviourScript.ResetAnimation();
    }
    //=====================================================================================
    public void IncreaseQuantity()
    {
        Debug.Log("Increased Portal");
        Quantity++;
        SaveQuantity();
    }

    public void UsePortal() //used in portal button
    {
        if (Quantity > 0)
        {
            Quantity--;
            SaveQuantity();
            Debug.Log("Portal Used! Remaining Quantity: " + Quantity);
            // Implement your portal functionality here
            MapEdgeManager.instance.uiBlocker.SetActive(true);
            if (MapEdgeManager.instance.surroundingCheck == null)
            {
                Debug.Log("MEM: null surround check");
                MapEdgeManager.instance.surroundingCheck = GameObject.FindObjectOfType<SurroundingCheck>();
            }
            MapEdgeManager.instance.surroundingCheck.EnemiesInTrigger.Clear();

            PortalAnimation();

        }
        else
        {
            Debug.Log("No more portals left!");
            // Implement what happens if no more portals are available
        }
    }

    private void SaveQuantity()
    {
        PlayerPrefs.SetInt("PortalQuantity", Quantity);
        UpdateQuantityText();
    }

    private void LoadQuantity()
    {
        Quantity = PlayerPrefs.GetInt("PortalQuantity", 0);
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        ppCount.text = Quantity.ToString();
    }
    public void LongPressFoo()
    {
        PPTime.SetActive(true);
        //pptimeTXT.text = GetFormattedTimeSpan(new TimeSpan(0, 2, 0, 0)); // Display initial 2-hour countdown
        
        CancelInvoke(nameof(TurnOffPPTime));
        Invoke(nameof(TurnOffPPTime), 3f);
        //PPButton.interactable = false;
    }

    void TurnOffPPTime()
    {
        PPTime.SetActive(false);
        //PPButton.interactable = true;
    }

    // Countdown Timer Methods
    private void StartCountdownTimer()
    {
        StartCoroutine(UpdateTimerCoroutine());
    }

    private IEnumerator UpdateTimerCoroutine()
    {
        TimeSpan timerDuration = TimeSpan.FromHours(2);
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.Add(timerDuration);

        while (DateTime.Now < endTime)
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            pptimeTXT.text = GetFormattedTimeSpan(remainingTime);
            yield return new WaitForSeconds(1f); // Update timer every second
        }

        // Reset the timer once completed
        
    }

    private string GetFormattedTimeSpan(TimeSpan timeSpan)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
