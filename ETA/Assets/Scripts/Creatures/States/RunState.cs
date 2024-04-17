using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        Debug.Log("Enter RunState");

        agent.speed = speed;
        
        //animator.Play(anim.name);
    }

    public override void Execute()
    {
        agent.SetDestination(detector.target.position);

        if (machineCore.IsArriveAgent())
        {
            isComplete = true;
            return;
        }

        // HP가 0인 경우 멈춘다.
        // isComplete = true;
    }

    public override void Exit()
    {
        Debug.Log("Exit RunState");
    }

    private void SetDestination()
    {

    }

    
}
