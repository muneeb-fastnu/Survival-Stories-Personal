using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PocketPortal : MonoBehaviour
{
    public TMP_Text ppCount;
    public static PocketPortal instance;

    public GameObject mainCharacter;
    public Vector3 defaultPlayerPosition;

    bool HasPressedPortal = false;

    [SerializeField] CharacterBehaviour characterBehaviourScript;
    [SerializeField] AICharacterBehaviour aiCharacterBehaviour;
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
        InvokeRepeating(nameof(IncreaseQuantity), IncreaseIntervalHours * 3600, IncreaseIntervalHours * 3600); // Invoke repeating every 2 hours

        //IncreaseQuantity();
    }

    void Update()
    {
        if (HasPressedPortal)
        {
            if (!CircleZoom.instance.triggered)
            {
                StartCoroutine(WaitForSecondsCoroutine(2f));
            }
        }
    }

    public void PortalAnimation()
    {

        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = false;
        InventorySystem.instance.isIdle = true;
        characterBehaviourScript = mainCharacter.GetComponent<CharacterBehaviour>();
        

        HasPressedPortal = true;
        CircleZoom.instance.CircleZoomTrigger();
    }
    public void GoToHomeMap()
    {
        mainCharacter.transform.position = defaultPlayerPosition;
    }
    IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HasPressedPortal = false;
        CircleZoom.instance.ResetCircleSize();
        
        GoToHomeMap();
        aiCharacterBehaviour.CurrentTarget = null;
        characterBehaviourScript.currentTargetInfo = null;
        
        //characterBehaviourScript.StopNavAgent();
        characterBehaviourScript.ResetAnimation();


        PromptManager.Instance.mainPlayerController.isPlayerAllowedMove = true;
        //InventorySystem.instance.isIdle = false;
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
}
