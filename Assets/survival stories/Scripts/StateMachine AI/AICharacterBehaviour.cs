using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AICharacterBehaviour : MonoBehaviour
{
    public HitboxTrigger hbTrigger;
    public CharacterStats stats;
    public CharacterData data;
    public Animator animator;
    public bool alive = true;
    public float currenHealth;
    public GameObject CurrentTarget;
    public float resetTime;
    //private float resetTimeMultiplier = 0.5f;
    public float timeLeft;
    public float originalHealth;

    public GameObject healthBarPrefab;
    private bool isHealthOn = false;
    public GameObject healthBar = null;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currenHealth = stats.health.maxQuantity;
        hbTrigger.hitEvent += GiveDamage;
        originalHealth = currenHealth;
    }
    private void Start()
    {
        hbTrigger.gameObject.SetActive(alive);
    }
    private void Update()
    {
        if(isHealthOn && healthBar != null) 
        {
            float currentHealthPercentage = currenHealth / originalHealth;
            GameObject actualLoading = healthBar.transform.GetChild(0).gameObject;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            actualLoading.transform.position = screenPos;

            Slider healthBarSlider = healthBar.transform.GetChild(0).GetComponent<Slider>();
            healthBarSlider.value = currentHealthPercentage;
        }
        if(NewTutorial.inTutorial && gameObject.tag == "NewEnemy")
        {
            gameObject.transform.GetChild(0).GetComponent<SurroundingCheckAction>().EnemiesInTrigger.Clear();
        }
        if (!alive)
        {
            StateController stateController = GetComponent<StateController>();
            if (stateController != null)
            {
                stateController.agent.isStopped = true;
            }
        }
    }
    public void GiveDamage(GameObject gam)
    {
        
        AICharacterBehaviour en = gam.GetComponent<AICharacterBehaviour>();


        en.CurrentTarget = gameObject;

        if (en.GetComponent<ObjectInfo>().playerType == PlayerType.User)
        {
            PromptManager.Instance.PlayerHurt();

            en.stats.health.currentQuantity -= stats.baseAttackDamage;
            en.currenHealth = en.stats.health.currentQuantity;

            if (en.stats.health.currentQuantity <= 0)
            {
                CurrentTarget = null;
                en.currenHealth = stats.health.maxQuantity;
                en.stats.health.currentQuantity = en.stats.health.maxQuantity;
                en.animator.SetLayerWeight(1, 0);
                en.Die();

            }
        }
        else
        {
            en.animator.SetBool("Hit", true);
            InventoryItem data = InventorySystem.HasSword();
            if (data != null)
            {
                CraftingSystem.LosingDurability(data, 0);
                en.currenHealth -= (data.data as ToolData).Damage + stats.baseAttackDamage;
                SFXManager.instance.PlayWeaponHit();
                if (alive && en.alive)
                {
                    Debug.Log("Cuurent = " + gameObject.name + "  en.->" + en.gameObject.name);
                    DisplayHealthOnHead();
                    en.DisplayHealthOnHead();
                }
            }
            else
            {
                if (alive && en.alive)
                {
                    //Debug.Log("Cuurent = " + gameObject.name + "  en.->" + en.gameObject.name);
                    DisplayHealthOnHead();
                    en.DisplayHealthOnHead();
                }
                en.currenHealth -= stats.baseAttackDamage;
                SFXManager.instance.PlayBareHit();
            }
            if (en.currenHealth > 4 && en.currenHealth < 8)
            {
                if (en.gameObject.transform.rotation.y == -180)
                {
                    StartCoroutine(Knockback(Vector3.right, en));
                }
                else
                {
                    StartCoroutine(Knockback(Vector3.left, en));
                }
                InventoryItem data2 = InventorySystem.HasSword();
                if (data2 != null)
                {
                    SFXManager.instance.PlayWeaponKB();
                }
                else
                {
                    SFXManager.instance.PlayBareKB();
                }
            }
            PromptManager.Instance.EnemyAttacked();

            if (en.currenHealth <= 0)
            {
                CurrentTarget = null;
                StateController stateController = GetComponent<StateController>();
                if(stateController != null)
                {
                    stateController.surroundCheck.EnemiesInTrigger.Clear();
                    stateController.surroundCheck.gameObject.SetActive(false);
                    stateController.agent.speed = 0.001f;
                }
                en.Die();

            }
        }

        
        if(isHealthOn && (en.GetComponent<ObjectInfo>().playerType == PlayerType.User))
        {
            turnoffhealth = StartCoroutine(TurnOffHealthAfter(1, healthBar));
        }
    }
    private Coroutine turnoffhealth;
    Slider healthBarSlider = null;
    public void DisplayHealthOnHead()
    {
        if(!alive)
        {
            return;
        }
        
        float currentHealthPercentage = currenHealth / originalHealth;
        
        if (!isHealthOn)
        {
            if (healthBar == null)
            {
                //Debug.Log("DHOH instantiate before");
                //Debug.Log("DHOH "+ gameObject.name);
                //healthBar = Instantiate(healthBarPrefab, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z), transform.rotation, transform);
                healthBar = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
                //Debug.Log("DHOH instantiate after");

                // Set the position of the loading bar within the Canvas
                RectTransform loadingBarRect = healthBar.transform.GetChild(0).GetComponent<RectTransform>();
                loadingBarRect.anchoredPosition = new Vector2(loadingBarRect.anchoredPosition.x, 100f);
                healthBarSlider = healthBar.transform.GetChild(0).GetComponent<Slider>();
            }
            else
            {
                //Debug.Log("DHOH update");
                GameObject actualLoading = healthBar.transform.GetChild(0).gameObject;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
                actualLoading.transform.position = screenPos;
            }
            isHealthOn = true;
            healthBar.SetActive(true);
            turnoffhealth =  StartCoroutine(TurnOffHealthAfter(1, healthBar));
        }
        if (healthBarSlider == null)
        {
            healthBarSlider = healthBar.transform.GetChild(0).GetComponent<Slider>();
        }
        //Debug.Log("Slider value changed for " + GetComponent<ObjectInfo>().playerType + "  :  " + currentHealthPercentage);
        healthBarSlider.value = currentHealthPercentage;
        
    }
    IEnumerator TurnOffHealthAfter(float seconds, GameObject healthBar)
    {
        yield return new WaitForSeconds(seconds);
        healthBar.SetActive(false);
        isHealthOn = false;
        KeyPress.instance.TriggerKeyPub();
    }
    private float knockbackDistance = 1f;
    private float knockbackDuration = 0.5f;
    
    IEnumerator Knockback(Vector3 direction, AICharacterBehaviour en)
    {
        Debug.Log("KnockBack2");
        Vector3 startPosition = en.gameObject.transform.position;
        Vector3 targetPosition = en.gameObject.transform.position + direction * knockbackDistance;

        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            en.gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        en.gameObject.transform.position = targetPosition;
    }
    public void GiveXponDeath()
    {

    }
    public void GameLostFoo()
    {
        Debug.Log("About to GameLostFoo");
        GameManager.GameLost();
    }
    public void Die()
    {
        if (alive)
        {
            if (data.objectType == ObjectType.Enemy)
            {
                InventoryItem swordData = InventorySystem.HasSword();
                if (swordData != null)
                {
                    
                    SFXManager.instance.PlayWeaponKill();
                }
                else
                {
                    SFXManager.instance.PlayBareKill();
                }

                PlayerProfileSystem.AddXp(5);
                Debug.Log("xp rewarded");
                if(gameObject.tag == "NewEnemy")
                {

                    Invoke(nameof(ResetValuesAfterTime), 3f);
                }

            }
            else
            {
                CircleZoom.instance.CircleZoomTrigger();
                Debug.Log("Triggered circle zoom for death");

                Invoke(nameof(GameLostFoo), 1.5f);
               
            }

            CurrentTarget = null;
            alive = false;
            foreach (var item in data.ContainedRes)
            {
                Debug.Log("Adding in inventory: " + item.objectData.displayName);
                InventorySystem.AddItem(item.objectData, item.quantity);
            }
            animator.SetBool("PlayerDead", true);

        }

    }

    public void GoToMainMenuScene()
    {

        SceneManager.LoadScene(0);
    }

    public void ResetValuesAfterTime()
    {
        Debug.Log("Turning off character");
        if (gameObject.TryGetComponent<StateController>(out StateController con))
        {
            con.enabled = false;

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(0).GetComponent<SurroundingCheckAction>().EnemiesInTrigger.Clear(); ;



        }
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        alive = true;
        CurrentTarget = null;
        StartCoroutine(StartResetting(resetTime));  //enemy respawn time, respawn multiplier
    }
    private IEnumerator StartResetting(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        gameObject.GetComponent<Animator>().enabled = true;
        animator.SetBool("PlayerDead", false);
        currenHealth = originalHealth;
        if (gameObject.TryGetComponent<StateController>(out StateController con))
        {
            con.enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;



    }
    public void GiveKillReward()
    {

    }
    //PocketPortal Button
    public void SetCurrentTargetNull()
    {
        CurrentTarget = null;
    }
}
