using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

//[RequireComponent (typeof(SurroundingCheck))]
public class MainPlayerController : MonoBehaviour
{
    public Camera cam;
    public SurroundingCheck surroundingCheck;
    public GamepadInputManager inputManager;
    public CharacterBehaviour myCharacterBehaviour;
    public Rigidbody2D chracterRb;
    public float speed;
    public Animator mainCharaterAnimator;
    public NavMeshAgent agent;
    Vector2 lastposition;
    Vector2 currentposition;
    private CharacterStats stats;

    public bool clearListsOnce;

    public bool isPlayerAllowedMove = true;

    public Vector3 defaultPlayerLocation = new Vector3(0.5f, 0.65f, 0);

    private float directionChangeDelay = 0.4f; // Adjust the delay time as needed
    private float lastDirectionChangeTime;
    private int lastDirection = 0; //0=right, 1=left
    private int currentDirection; //0=right, 1=left

    private void Start()
    {
        //stats.moveSpeed = speed;
        //if (GameManager.LoadorNot)
        {
            Invoke(nameof(LoadData), 0.05f);
        }
        myCharacterBehaviour = GetComponent<CharacterBehaviour>();
        surroundingCheck = GetComponentInChildren<SurroundingCheck>();
        mainCharaterAnimator = GetComponent<Animator>();
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        cam = Camera.main;
        InventorySystem.instance._currentDefaults.player = this.gameObject;
        stats = GetComponent<AICharacterBehaviour>().stats;
        InvokeRepeating(nameof(SaveData), 1, 0.5f);
    }
    private void FixedUpdate()
    {
        if (isPlayerAllowedMove)
        {
            MainCharacterMove();
        }
    }
    private void Update()
    {


        currentposition = transform.position;
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    Debug.Log("this happened");
        //   transform.GetComponent<NavMeshAgent>().enabled = true;
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        //    Debug.Log(hit.transform.name);
        //    if (hit.collider != null)
        //    {
        //        agent.SetDestination(hit.point);
        //        Debug.Log(hit.point);
        //    }

        //}
        Vector2 inputState = inputManager.AnalogueValue;
        //if (Vector2.Distance(lastposition , currentposition) <= 0.02f)
        //{

        //    mainCharaterAnimator.SetBool("walking", false);

        //}
        //else
        //{

        //}
        if (inputState.sqrMagnitude > 0 && isPlayerAllowedMove)
        {
            //Debug.Log(": 76");
            mainCharaterAnimator.SetBool("walking", true);
            transform.GetComponentInChildren<NavMeshAgent>().enabled = false;
            if (surroundingCheck.allowCheck)
            {
                if (Announcements.instance) Announcements.DeactivateHoveringIcon();

                surroundingCheck.allowCheck = false;
            }
            InventorySystem.instance.isIdle = true;
            //MainCharaterAnimator.SetBool("walking", true);
        }
        else
        {
            //if (!myCharacterBehaviour.allowed)
            //{
            //   // MainCharaterAnimator.SetBool("walking", false);
            //}
            mainCharaterAnimator.SetBool("walking", false);
            chracterRb.velocity = new Vector2(0, 0);

        }
        lastposition = transform.position;

        if(!isPlayerAllowedMove)
        {
            transform.GetComponentInChildren<NavMeshAgent>().enabled = false;
            mainCharaterAnimator.SetBool("walking", false);
            mainCharaterAnimator.SetBool("foraging", false);
        }
        if (mainCharaterAnimator.GetBool("walking"))
        {
            SFXManager.instance.StopWoodChopCoroutine();
            SFXManager.instance.StopStoneMiningCoroutine();
            SFXManager.instance.StartWalkGrassCoroutine();
        }
        else
        {
            SFXManager.instance.StopWalkGrassCoroutine();
        }
        if(!mainCharaterAnimator.GetBool("foraging"))
        {
            SFXManager.instance.StopWoodChopCoroutine();
            SFXManager.instance.StopStoneMiningCoroutine();
        }
    }

    public void SaveData()
    {
        string playerPosStr = Vector3ToString(transform.position);
        PlayerPrefs.SetString("PlayerPosition", playerPosStr);
    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            string playerPosStr = PlayerPrefs.GetString("PlayerPosition", Vector3ToString(defaultPlayerLocation));
            transform.position = StringToVector3(playerPosStr);
        }
        else
        {
            transform.position = defaultPlayerLocation;
        }
    }
    // Convert Vector3 to string
    public string Vector3ToString(Vector3 vector)
    {
        return vector.x + "," + vector.y + "," + vector.z;
    }

    // Convert string to Vector3
    public Vector3 StringToVector3(string vectorString)
    {
        string[] components = vectorString.Split(',');

        if (components.Length == 3)
        {
            float x, y, z;

            if (float.TryParse(components[0], out x) &&
                float.TryParse(components[1], out y) &&
                float.TryParse(components[2], out z))
            {
                return new Vector3(x, y, z);
            }
        }

        // Return a default Vector3 if parsing fails
        return Vector3.zero;
    }
    public void MainCharacterMove()
    {

        Vector2 inputState = inputManager.AnalogueValue;

        if (inputState.sqrMagnitude > 0 && isPlayerAllowedMove)
        {
            if (clearListsOnce)
            {// do this to reset the old values
                mainCharaterAnimator.SetBool("foraging", false);

                surroundingCheck.EnemiesInTrigger.Clear();
                surroundingCheck.ResourcesInTrigger.Clear();
                clearListsOnce = false;
                surroundingCheck.transform.position = Vector2.Lerp(surroundingCheck.transform.position, transform.position, 5f);
                surroundingCheck.transform.SetParent(transform);
                myCharacterBehaviour.currentTargetInfo = null;
                
                myCharacterBehaviour.ResetAnimation();
                Debug.Log("MPC: 128");

                mainCharaterAnimator.SetBool("walking", true);

            }
            // add functions later

            myCharacterBehaviour.aiChar.CurrentTarget = null;
            inputState = Vector2.ClampMagnitude(inputState, 1.0f);

            
            
            // Debug.Log(inputState);
            ChangeDirection(inputState);

            chracterRb.velocity = inputState.normalized * stats.moveSpeed * Time.deltaTime;
            


            // MainCharaterAnimator.SetBool("walking", true);
            // chracterRb.MovePosition(chracterRb.position + inputState.normalized * speed * Time.deltaTime);
        }
        else
        {
            if (!surroundingCheck.allowCheck && isPlayerAllowedMove)
            {
                ///s till needs more work
                surroundingCheck.allowCheck = true;
            }
            // MainCharaterAnimator.SetBool("walking", false);
            chracterRb.velocity = new Vector2(0, 0);
            clearListsOnce = true;
            if (clearListsOnce)
            {
                mainCharaterAnimator.SetBool("walking", false);
            }


        }

        
    }

    public void CheckAutoMovementAvailablity()
    {

    }
    private void ChangeDirection(Vector2 inputState)
    {
        
        if (inputState.x < 0)
        {
            currentDirection = 1;
            chracterRb.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (inputState.x > 0)
        {
            currentDirection = 0;
            chracterRb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        
        if (currentDirection != lastDirection)
        {
            if (Time.time - lastDirectionChangeTime <= directionChangeDelay)
            {
                Debug.Log("Player Slide");
                PromptManager.Instance.PlayerSlided();
                //ReduceStamina
                if(PlayerAttributesSystem.HasEnoughStamina())
                {
                    PlayerAttributesSystem.DepleteStamina();
                }
                SFXManager.instance.PlaySlide();
            }
            lastDirection = currentDirection;
            lastDirectionChangeTime = Time.time;
        }
    }

    public void ExecuteAutoMovement()
    {

    }
    public void ResetCamOnPlayer()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WaterColliders")
        {
            MapEdgeManager.instance.PlayerTouchedEdge();
        }
    }
    public void FollowPlayer()
    {
        if (cam == null)
        {
            Debug.LogError("Main camera reference not set.");
            return;
        }

        // Calculate the target position for the camera to follow the player
        Vector3 targetPosition = transform.position;

        // Optionally, you can add an offset or modify the target position based on certain conditions

        // Smoothly move the camera towards the target position
        float smoothSpeed = 5f; // Adjust the smoothness factor as needed
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
