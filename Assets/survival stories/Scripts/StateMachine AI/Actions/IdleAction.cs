using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Actions/IdleAction")]
public class IdleAction : Actions
{
    public override void Act(StateController Controller)
    {
        StayIdle(Controller);
    }

    public void StayIdle(StateController Controller)
    {
        Controller.agent.speed = 0.5f;
        // Controller.agent.enabled = false;
        // Controller.agent.SetDestination(Controller.lastKnownMyLocation);
        // any idle behaviour if needed

        // If the enemy does not see the player, patrol
        
        if (Vector3.Distance(Controller.transform.position, Controller.lastKnownMyLocation) < 0.7f)
        {
            // If the enemy is at lastKnownMyLocation, patrol to patrolPoint

            CharacterBehaviour.ChageDirection(Controller.transform, Controller.patrolPoint);
            Controller.agent.SetDestination(Controller.patrolPoint);
        }
        else if (Vector3.Distance(Controller.transform.position, Controller.patrolPoint) < 0.7f)
        {

            // If the enemy is at patrolPoint, return to lastKnownMyLocation
            CharacterBehaviour.ChageDirection(Controller.transform, Controller.lastKnownMyLocation);
            Controller.agent.SetDestination(Controller.lastKnownMyLocation);
        }

    }
}
