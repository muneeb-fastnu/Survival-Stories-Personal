using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/RunAway")]
public class RunAwayAction : Actions
{
    public float offsetDistance;
    private float runAwayDistance = 20f;
    public override void Act(StateController controller)
    {
        RunAway(controller);
    }

    public void RunAway(StateController controller)
    {
        Vector3 runAwayPoint = controller.lastKnownMyLocation;
        //Debug.Log("start walking");
        controller.agent.speed = 1.5f;
        if (controller.surroundCheck.EnemiesInTrigger.Count > 0)
        {
            //CharacterBehaviour.ChageDirection(controller.transform, controller.target.transform.position);
            if (Vector2.Distance(controller.agent.transform.position, controller.target.position) > .6f)
            {

                // Get the direction from the enemy to the player
                Vector3 directionToPlayer = controller.transform.position - controller.target.position;

                // Calculate the destination point away from the player
                runAwayPoint = controller.transform.position + directionToPlayer.normalized * runAwayDistance;

                // Check if the run away point is on a walkable NavMesh
                if (!IsWalkableNavMesh(runAwayPoint))
                {
                    // If not walkable, find the nearest walkable point
                    runAwayPoint = FindNearestWalkablePoint(runAwayPoint, controller);
                }

                // Set the NavMesh agent destination to the run away point
                controller.agent.SetDestination(runAwayPoint);

                // Rotate the enemy towards the run away point
                CharacterBehaviour.ChageDirection(controller.transform, runAwayPoint);


            }


        }
        else
        {
            //if (Vector3.Distance(controller.transform.position, runAwayPoint) < 0.7f)
            {
                CharacterBehaviour.ChageDirection(controller.transform, controller.lastKnownMyLocation);
                controller.agent.SetDestination(controller.lastKnownMyLocation);
            }
        }

    }
    private bool IsWalkableNavMesh(Vector3 point)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas);
    }
    private Vector3 FindNearestWalkablePoint(Vector3 point, StateController controller)
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = controller.target.position - controller.transform.position;

        // Calculate the perpendicular direction
        Vector3 perpendicularDirection = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0f).normalized;

        // Calculate the right and left points from the player
        Vector3 rightPoint = controller.target.position + perpendicularDirection * runAwayDistance; // Adjust the distance as needed
        Vector3 leftPoint = controller.target.position - perpendicularDirection * runAwayDistance; // Adjust the distance as needed

        // Check if the right point is on a walkable NavMesh
        if (IsWalkableNavMesh(rightPoint))
        {
            return rightPoint;
        }
        // Check if the left point is on a walkable NavMesh
        else if (IsWalkableNavMesh(leftPoint))
        {
            return leftPoint;
        }
        else
        {
            // If no walkable point found, return the original point
            return point;
        }
    }
}
