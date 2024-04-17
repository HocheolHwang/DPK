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

        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        //animator.Play(anim.name);
    }

    public override void Execute()
    {


        if (detector.target != null)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit IdleState");

        agent.isStopped = false;
    }
}
