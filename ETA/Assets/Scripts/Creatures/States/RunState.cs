using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RunState : State
{
    [SerializeField] public AnimationClip _anim;
    [SerializeField] public float _maxSpeed;
    [SerializeField] public float _speed;        // animator  속도와 동일하게 세팅

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
