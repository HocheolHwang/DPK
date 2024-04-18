using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RunState : State
{
    [Header("해당 상태에서 사용할 속성")]
    [SerializeField] public AnimationClip anim;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float speed;        // animator  속도와 동일하게 세팅

    public override void Enter()
    {
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }
}
