using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/state")]
public class State : ScriptableObject
{
    public float animationBlendTreeFloat;
    public Actions[] action;
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        ExecuteAction(controller);
        CheckForTransition(controller);
    }
    private void ExecuteAction(StateController controller)
    {

        foreach (var item in action)
        {
            item.Act(controller);
        }
    }

    public void CheckForTransition(StateController controller)
    {
        if (controller.currentState.animationBlendTreeFloat == 0 && controller.enemyType == "Bunny")
        {
            bool decision;
            decision = transitions[1].decision.Decide(controller);
            if (decision)
            {
                controller.TransitionToState(transitions[1].trueState);

            }
            else
            {
                controller.TransitionToState(transitions[1].falseState);

            }
        }
        else
        {
            //foreach (var item in transitions)
            if (controller.currentState.animationBlendTreeFloat == 0)
            {
                bool decision;
                decision = transitions[0].decision.Decide(controller);
                if (decision)
                {
                    controller.TransitionToState(transitions[0].trueState);

                }
                else
                {
                    controller.TransitionToState(transitions[0].falseState);

                }
            }
            else
            {
                foreach (var item in transitions)
                {
                    bool decision;
                    decision = item.decision.Decide(controller);
                    if (decision)
                    {
                        controller.TransitionToState(item.trueState);

                    }
                    else
                    {
                        controller.TransitionToState(item.falseState);

                    }
                }
            }
        }
    }
}
