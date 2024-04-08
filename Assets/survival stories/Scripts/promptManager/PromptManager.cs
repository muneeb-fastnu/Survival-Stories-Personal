using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem.EnhancedTouch;
//using static UnityEditor.MaterialProperty;
using System.Reflection;

public class PromptManager : MonoBehaviour
{
    public GameObject speechBubble;
    [SerializeField] Transform mainCharacter;
    [SerializeField] TextMeshProUGUI prompt;

    [SerializeField] Traits health;
    [SerializeField] Traits hunger;
    [SerializeField] Traits thirst;
    [SerializeField] Traits stamina;

    // Dictionary to track the last time each prompt type was shown.
    private Dictionary<PromptType, float> promptCooldowns = new Dictionary<PromptType, float>();

    public float promptDuration = 3.0f; // Adjust as needed.
    public float cooldownDuration = 20f;
    public float globalCooldownDuration = 30.0f; // Cooldown duration for all prompts.

    public MainPlayerController mainPlayerController;
    Animator mainCharaterAnimator;

    public static PromptManager Instance;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    // Enum to represent the types of prompts.
    private enum PromptType
    {
        None,
        HealthLow,
        Hunger,
        Thirst,
        StaminaLow,
        EnemySpotted,
        EnemyAttacked,
        PlayerHurt,
        WoodNoAxe,
        WaterNBottle,
        StoneNoPick,
        ItemCrafted,
        BuildingConstructed,
        EatFood,
        DrinkWater,
        InventoryFull,
        StaminaRegained,
        Idle10,
        Idle30,
        Idle60,
        StaminaRegainedFull,
        Slide,
        Sit,
        // Add more prompt types as needed.
    }
    PromptType currentPromptType = PromptType.None;
    // Start is called before the first frame update
    void Start()
    {
        foreach (PromptType promptType in Enum.GetValues(typeof(PromptType)))
        {
            if (promptType != PromptType.None)
            {
                promptCooldowns[promptType] = -promptDuration - globalCooldownDuration; // Initialize to ensure no prompts appear at the beginning.
            }
        }
        //WoodNoAxe();
        mainPlayerController = mainCharacter.GetComponent<MainPlayerController>();
        mainCharaterAnimator = mainCharacter.GetComponent<Animator>();
        if (mainPlayerController == null)
        {
            Debug.Log("COuldnt fetch mainPLayerController");
        }
    }

    // Update is called once per frame
    void Update()
    {
        staminaPercentage = GetStaminaPErcentage();
        if (mainCharacter != null)
        {
            SetBubblePosition(mainCharacter.transform);
        }
        /*
        float lastShownTime;
        if (promptCooldowns.TryGetValue(currentPromptType, out lastShownTime))
        {
            float timeSinceLastShown = Time.time - lastShownTime;
            if (timeSinceLastShown >= promptDuration)
            {
                speechBubble.SetActive(false);
            }
        }
        */
        if ((health.currentQuantity / health.maxQuantity) * 100 < 50)
        {
            HealthLow((health.currentQuantity / health.maxQuantity) * 100);
        }
        if ((hunger.currentQuantity / hunger.maxQuantity) * 100 < 50)
        {
            Hungry((hunger.currentQuantity / hunger.maxQuantity) * 100);
        }
        if ((stamina.currentQuantity / stamina.maxQuantity) * 100 < 50)
        {
            if((stamina.currentQuantity / stamina.maxQuantity) * 100 < 10)
            {
                promptCooldowns[PromptType.StaminaLow] = Time.time - (cooldownDuration + 1);
            }
            StaminaLow((stamina.currentQuantity / stamina.maxQuantity) * 100);
        }
        if ((thirst.currentQuantity / thirst.maxQuantity) * 100 < 50)
        {
            Thirsty((thirst.currentQuantity / thirst.maxQuantity) * 100);
        }


    }
    public void SetBubblePosition(Transform parent = null)
    {
        //speechBubble.gameObject.SetActive(true);
        Vector3 positionwOFfset = new Vector3(parent.position.x + 1f, parent.position.y + 0.5f, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(positionwOFfset);
        speechBubble.transform.position = screenPos;
    }
    private PromptType GetCurrentPromptType()
    {
        return PromptType.None;
    }

    private bool IsOnCooldown(PromptType promptType)
    {
        // Check if the prompt type is on cooldown.
        float lastShownTime;
        if (promptCooldowns.TryGetValue(promptType, out lastShownTime))
        {
            float timeSinceLastShown = Time.time - lastShownTime;
            
            return timeSinceLastShown < cooldownDuration;
        }
        return false;
    }

    private void SetPromptCooldown(PromptType promptType)
    {
        // Set the cooldown for the specified prompt type to 30 seconds after the current time.
        if (promptType != PromptType.None)
        {
            promptCooldowns[promptType] = Time.time;
        }
    }

    void TurnOnBubble(PromptType type)
    {
        Debug.Log("Came from: " + type.ToString());
        speechBubble.SetActive(true);
        StartCoroutine(WaitPromptDuration(type));
    }
    IEnumerator WaitPromptDuration(PromptType type)
    {
        float lastShownTime;
        if (promptCooldowns.TryGetValue(type, out lastShownTime))
        {
            float timeSinceLastShown = Time.time - lastShownTime;

            yield return new WaitForSeconds(promptDuration);
            speechBubble.SetActive(false);
        }
    }

    float GetStaminaPErcentage()
    {
        float stprcnt = ((stamina.currentQuantity / stamina.maxQuantity) * 100);
        return stprcnt;
    }
    bool isStaminaLow = false;
    public float staminaPercentage = 0;
    public void StaminaVeryLow()
    {
        StartCoroutine(CheckStamina());
    }
    IEnumerator CheckStamina()
    {
        if(isStaminaLow)
        {
            mainCharaterAnimator.SetBool("foraging", false);
            mainCharaterAnimator.SetBool("walking", false);
        }

        while (staminaPercentage < 55)
        {
            if (staminaPercentage <= 10 && !isStaminaLow)
            {
                isStaminaLow = true;
                // Perform actions when stamina is very low
                mainPlayerController.isPlayerAllowedMove = false;
                mainCharaterAnimator.SetFloat("foraging float", 0);
                mainCharaterAnimator.SetBool("foraging", false);
                mainCharaterAnimator.SetBool("waling", false);
                InventorySystem.instance.isIdle = true;
            }
            if (staminaPercentage > 48 && isStaminaLow)
            {
                SetPromptCooldown(PromptType.StaminaLow);
                isStaminaLow = false;
                // Perform actions when stamina is back to normal
                mainPlayerController.isPlayerAllowedMove = true;
                StaminaRegained(); 
                break;
            }

            yield return new WaitForSeconds(1f); // Adjust the interval based on your needs
        }
    }

    // Modify the existing prompt methods as needed.____________________________________________________________________________________
    public void HealthLow(float percentage)
    {
        currentPromptType = PromptType.HealthLow;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        if (percentage < 50 && percentage > 20)
        {
            prompt.text = "Ughhh!!";
        }
        else if (percentage < 20 && percentage > 10)
        {
            prompt.text = "Health Low";
        }
        else if (percentage < 10)
        {
            prompt.text = "Is this how I die?";
        }

        SetPromptCooldown(PromptType.HealthLow);

        TurnOnBubble(PromptType.HealthLow);
        Debug.Log("Health Went Here");
        

    }
    public void Hungry(float percentage)
    {
        currentPromptType = PromptType.Hunger;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        if (percentage < 50 && percentage > 20)
        {
            prompt.text = "Stomach's calling..";
        } 
        else if(percentage < 20 && percentage > 10)
        {
            prompt.text = "Been a while since I ate..";
        }
        else if(percentage < 10)
        {
            prompt.text = "Need... food...";
        }
        
        SetPromptCooldown(PromptType.Hunger);
        
        TurnOnBubble(PromptType.Hunger);
        

    }

    public void Thirsty(float percentage)
    {
        currentPromptType = PromptType.Thirst;
        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        if (percentage < 50 && percentage > 20)
        {
            prompt.text = "Could use some water";
        }
        else if (percentage < 20 && percentage > 10)
        {
            prompt.text = "Super thirsty!";
        }
        else if (percentage < 10)
        {
            prompt.text = "Need... Water...";
        }
        SetPromptCooldown(PromptType.Thirst);

        TurnOnBubble(PromptType.Thirst);
        
    }

    public void StaminaLow(float percentage)
    {
        if (percentage < 10)
        {
            
        }
            currentPromptType = PromptType.StaminaLow;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        if (percentage < 50 && percentage > 20)
        {
            prompt.text = "Could take a break..";
        }
        //else if (percentage < 20 && percentage > 10)
        //{
        //    prompt.text = "Need to rest";
        //}
        else if (percentage < 10)
        {
            prompt.text = "Ugh... too tired..";
            StaminaVeryLow();
        }
        SetPromptCooldown(PromptType.StaminaLow);

        TurnOnBubble(PromptType.StaminaLow);
        
    }

    public void EnemySpotted()
    {
        //sorroundCheck
        currentPromptType = PromptType.EnemySpotted;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "What's that?";
        SetPromptCooldown(PromptType.EnemySpotted);

        TurnOnBubble(PromptType.EnemySpotted);
        
    }

    public void WoodNoAxe()
    {
        //Character Behaviour: 294
        currentPromptType = PromptType.WoodNoAxe;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Probably need an axe";
        SetPromptCooldown(PromptType.WoodNoAxe);

        TurnOnBubble(PromptType.WoodNoAxe);
        
    }

    public void EnemyAttacked()
    {
        //aiCharacterBehaviour
        currentPromptType = PromptType.EnemyAttacked;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Take that!";
        SetPromptCooldown(PromptType.EnemyAttacked);

        TurnOnBubble(PromptType.EnemyAttacked);
        
    }
    public void PlayerHurt()
    {
        //aiCharacterBehaviour

        currentPromptType = PromptType.PlayerHurt;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Ouch!";
        SetPromptCooldown(PromptType.PlayerHurt);

        TurnOnBubble(PromptType.PlayerHurt);
        
    }
    public void WaterNBottle()
    {
        currentPromptType = PromptType.WaterNBottle;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "How can I keep this?";
        SetPromptCooldown(PromptType.WaterNBottle);

        TurnOnBubble(PromptType.WaterNBottle);
        //cancel//invoke("TurnOffBubble");
        //invoke("TurnOffBubble", promptDuration);
    }
    public void StoneNoPick()
    {
        currentPromptType = PromptType.StoneNoPick;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "No pickaxe?";
        SetPromptCooldown(PromptType.StoneNoPick);

        TurnOnBubble(PromptType.StoneNoPick);
        
    }
    public void ItemCrafted()
    {
        currentPromptType = PromptType.ItemCrafted;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Looking good!";
        SetPromptCooldown(PromptType.ItemCrafted);

        TurnOnBubble(PromptType.ItemCrafted);
        
    }
    public void BuildingConstructed()
    {

        currentPromptType = PromptType.BuildingConstructed;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "I'm good at this";
        SetPromptCooldown(PromptType.BuildingConstructed);

        TurnOnBubble(PromptType.BuildingConstructed);
        
    }
    public void EatFood()
    {
        currentPromptType = PromptType.EatFood;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "That hit the spot..";
        SetPromptCooldown(PromptType.EatFood);

        TurnOnBubble(PromptType.EatFood);
        //cancel//invoke("TurnOffBubble");
        //invoke("TurnOffBubble", promptDuration);
    }
    public void DrinkWater()
    {
        currentPromptType = PromptType.DrinkWater;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "I feel a little better!";
        SetPromptCooldown(PromptType.DrinkWater);

        TurnOnBubble(PromptType.DrinkWater);
        //cancel//invoke("TurnOffBubble");
        //invoke("TurnOffBubble", promptDuration);
    }
    public void InventoryFull()
    {
        currentPromptType = PromptType.InventoryFull;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "I can't fit that!";
        SetPromptCooldown(PromptType.InventoryFull);

        TurnOnBubble(PromptType.InventoryFull);
        
    }

    public void StaminaRegained()
    {
        currentPromptType = PromptType.StaminaRegained;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Okay, I feel better!";
        SetPromptCooldown(PromptType.StaminaRegained);

        TurnOnBubble(PromptType.StaminaRegained);

    }

    public void StaminaRegainedFull()
    {
        currentPromptType = PromptType.StaminaRegainedFull;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Ready for another one!";
        SetPromptCooldown(PromptType.StaminaRegainedFull);

        TurnOnBubble(PromptType.StaminaRegainedFull);

    }

    public void Idle10()
    {
        currentPromptType = PromptType.Idle10;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Going to sit!";
        SetPromptCooldown(PromptType.Idle10);

        TurnOnBubble(PromptType.Idle10);

    }
    public void Idle30()
    {
        currentPromptType = PromptType.Idle30;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Whistle!";
        SetPromptCooldown(PromptType.Idle30);

        TurnOnBubble(PromptType.Idle30);

    }
    public void Idle60()
    {
        currentPromptType = PromptType.Idle60;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Zzzzzz...";
        SetPromptCooldown(PromptType.Idle60);

        TurnOnBubble(PromptType.Idle60);

    }
    public void PlayerSlided()
    {
        currentPromptType = PromptType.Slide;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "SLIDED!";
        SetPromptCooldown(PromptType.None);

        TurnOnBubble(PromptType.Slide);

    }
    public void SitDown()
    {
        currentPromptType = PromptType.Sit;

        if (IsOnCooldown(currentPromptType))
        {
            return;
        }
        prompt.text = "Sitting Down!";
        SetPromptCooldown(PromptType.None);

        TurnOnBubble(PromptType.Sit);

    }
    //-----------------------------------------------------

}
