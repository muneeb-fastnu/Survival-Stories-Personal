using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class KeyPress : MonoBehaviour
{
    public GamepadInputManager gamepadInputManager;
    public float changeAmount = 0.0001f;

    public GameObject mainCharacter;

    private Camera mainCamera;
    private float thresholdDistance = 0.5f; // Adjust this value as needed

    public static KeyPress instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        mainCamera = Camera.main;
        //InvokeRepeating(nameof(TriggerMinuteChange), 1f, 3f); // Invoke the function repeatedly with a delay
        Invoke(nameof(TriggerKeyPub), 0.1f);
    }

    private void Update()
    {
        TriggerMinuteChange();
    }
    public void TriggerMinuteChange()
    {
        Vector3 playerScreenPos = mainCamera.WorldToViewportPoint(mainCharacter.transform.position);
        float distanceFromCenter = Vector2.Distance(new Vector2(playerScreenPos.x, playerScreenPos.y), Vector2.one * 0.5f);

        // Check if the mainCharacter is not approximately in the center of the camera view
        if (distanceFromCenter > thresholdDistance)
        {
            StartCoroutine(TriggerKey());
        }
    }
    public void TriggerKeyPub()
    {
        StartCoroutine(TriggerKey());
    }
    public IEnumerator TriggerKey()
    {
        // Get the current AnalogueValue
        Vector2 currentValue = gamepadInputManager.AnalogueValue;
        Debug.Log("currentValue: " + currentValue.x + ", " + currentValue.y);
        // Generate a random offset within the specified range
        float offsetX = Random.Range(-changeAmount, changeAmount);
        float offsetY = Random.Range(-changeAmount, changeAmount);

        // Apply the offset to the current value
        Vector2 newValue = currentValue + new Vector2(offsetX, offsetY);
        Debug.Log("Offsets: " + offsetX + ", " + offsetY);
        Debug.Log("newValue: " + newValue.x + ", " + newValue.y);
        // Clamp the new value to ensure it stays within the valid range (-1 to 1)
        newValue.x = Mathf.Clamp(newValue.x, -1f, 1f);
        newValue.y = Mathf.Clamp(newValue.y, -1f, 1f);

        Debug.Log("brand newValue: " + newValue.x + ", " + newValue.y);
        // Update the AnalogueValue of the GamepadInputManager
        gamepadInputManager.AnalogueValue = newValue;

        float minimumVal = Mathf.Min(offsetX, offsetY);
        yield return new WaitForSeconds(minimumVal);

        newValue = Vector2.zero;
        gamepadInputManager.AnalogueValue = newValue;
    }

}
