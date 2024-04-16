using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 근접 공격
/// </summary>
public class MeleeAttackState : State
{
    [SerializeField] public AnimationClip anim;



    public override void Enter()
    {
        Debug.Log("Start MeleeState");
        //animator.Play(anim.name);
    }

    public override void Execute()
    {

        if (time > 1.0f)
        {
            isComplete = true;
        }
        // 공격을 1회 수행한다. 
        // isComplete = true;
    }
}
