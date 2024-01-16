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
    private void Start()
    {

        myCharacterBehaviour = GetComponent<CharacterBehaviour>();
        surroundingCheck = GetComponentInChildren<SurroundingCheck>();
        mainCharaterAnimator = GetComponent<Animator>();
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        cam = Camera.main;
        InventorySystem.instance._currentDefaults.player = this.gameObject;
        stats = GetComponent<AICharacterBehaviour>().stats;
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
            chracterRb.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (inputState.x > 0)
        {
            chracterRb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
}
