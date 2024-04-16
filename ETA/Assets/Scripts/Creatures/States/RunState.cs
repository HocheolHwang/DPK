using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// animation만 재생한다.
/// </summary>
public class RunState : State
{
    [SerializeField] public AnimationClip anim;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float speed;        // animator  속도와 동일하게 세팅

    public override void Enter()
    {
        Debug.Log("Start MoveState");
        //animator.Play(anim.name);
    }

    public override void Execute()
    {
        // animator speed와 agent speed를 맞춘다.
        machineCore.agent.SetDestination(machineCore.detector.target.position);

        // HP가 0인 경우 멈춘다.
        // isComplete = true;
    }
}
