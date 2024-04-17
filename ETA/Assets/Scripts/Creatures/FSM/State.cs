using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class State : IState
{
    public bool _isComplete { get; protected set; }      // 현재 상태가 끝났나?
    protected float _startTime;                          // 현재 상태의 시작 시간
    public float _executeTime => Time.time - _startTime;         // 현재 상태가 된 시점으로부터 얼마나 지났는지
 
    // Base
    protected BaseController _machineCore;
    protected Animator _animator => _machineCore._animator;
    protected NavMeshAgent _agent => _machineCore._agent;



    protected void ChangeState(State newState, bool forceReset = false)
    {
        _machineCore._machine.ChangeState(newState, forceReset);
    }


    public void GetBaseMemberVariable(BaseController machineCore)
    {
        _machineCore = machineCore;
    }


    public void Initialize()
    {
        _isComplete = false;
        _startTime = Time.time;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { }
}
