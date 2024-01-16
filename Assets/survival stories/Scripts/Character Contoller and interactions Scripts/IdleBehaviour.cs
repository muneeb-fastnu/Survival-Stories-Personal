using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : MonoBehaviour
{
    private float idleStartTime;
    private float idleTime;

    void Start()
    {
        // Initialize the idleTime variable.
        idleTime = 0f;
    }

    void Update()
    {
        // Check for player input or movement here.
        if (1==2/* Your condition to check if the player is idle */)
        {
            // If the player is idle, start or continue tracking idle time.
            StartIdle();
        }
        else
        {
            // If the player is not idle, end idle tracking.
            EndIdle();
        }
    }

    void StartIdle()
    {
        // Start or continue tracking idle time.
        if (idleStartTime == 0f)
        {
            idleStartTime = Time.time;
        }
        // Update idle time at every frame.
        idleTime = Time.time - idleStartTime;
    }

    void EndIdle()
    {
        // End idle tracking.
        if (idleStartTime != 0f)
        {
            // Calculate the total idle time.
            float elapsedTime = Time.time - idleStartTime;
            idleTime += elapsedTime;
            // Reset the start time.
            idleStartTime = 0f;

            // Check the value of idleTime and apply logic for specific time intervals.
            if (idleTime >= 10f && idleTime < 30f)
            {
                // Apply logic when idleTime is 10 seconds.
                // For example: Call a function or set a variable.
            }
            else if (idleTime >= 30f && idleTime < 60f)
            {
                // Apply logic when idleTime is 30 seconds.
                // For example: Call a different function or set another variable.
            }
            else if (idleTime >= 60f && idleTime < 1800f) // 30 minutes in seconds
            {
                // Apply logic when idleTime is 60 seconds.
            }
            else if (idleTime >= 1800f && idleTime < 3600f) // 1 hour in seconds
            {
                // Apply logic when idleTime is 30 minutes.
            }
            else if (idleTime >= 3600f) // 1 hour in seconds
            {
                // Apply logic when idleTime is 1 hour or more.
            }
        }
    }
}
