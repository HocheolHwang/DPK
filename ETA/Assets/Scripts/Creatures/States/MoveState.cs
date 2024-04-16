using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    [SerializeField] public AnimationClip anim;

    public override void Enter()
    {
        Debug.Log("Start MoveState");
        //animator.Play(anim.name);
    }

    public override void Execute()
    {

        // 사정거리에 플레이어 또는 몬스터가 있는 경우 멈춘다.
        // HP가 0인 경우 멈춘다.
        // isComplete = true;
    }
}
