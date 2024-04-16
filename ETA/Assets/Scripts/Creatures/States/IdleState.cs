using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] public AnimationClip anim;

    public override void Enter()
    {
        Debug.Log("Start IdleState");
        //animator.Play(anim.name);
    }

    public override void Execute()
    {

        // 타겟이 null이 아닌 경우 -> Controller에서 관리

        // isComplete = true;
    }
}
