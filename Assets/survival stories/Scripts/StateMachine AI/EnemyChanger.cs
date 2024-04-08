using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AnimatorController = UnityEditorInternal.AnimatorController;

public class EnemyChanger : MonoBehaviour
{
    public RuntimeAnimatorController bunnyAnimator;
    public Sprite bunnySpr;
    public CharacterStats bunnyStat;
    public string bunnyStr = "Bunny";

    public RuntimeAnimatorController slimeAnimator;
    public Sprite slimeSpr;
    public CharacterStats slimeStat;
    public string slimeStr = "Slime";

    public RuntimeAnimatorController moleAnimator;
    public Sprite moleSpr;
    public CharacterStats moleStat;
    public string moleStr = "Mole";

    public AICharacterBehaviour ai_char;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public StateController stateController;

    private void OnEnable()
    {
        ai_char = GetComponent<AICharacterBehaviour>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateController = GetComponent<StateController>();
    }

    void Start()
    {
        
    }

    public void ChangeMeTo(RuntimeAnimatorController newAnimator, Sprite newSprite, CharacterStats newStat, string type)
    {
        ai_char.stats = newStat;

        animator.runtimeAnimatorController = newAnimator;

        spriteRenderer.sprite = newSprite;

        stateController.enemyType = type;

        //stateController.agent.speed = sp;
    }
}
