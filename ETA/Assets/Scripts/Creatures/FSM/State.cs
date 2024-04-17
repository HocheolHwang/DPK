using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class State : IState
{
    public bool isComplete { get; protected set; }      // 현재 상태가 끝났나?
    protected float startTime;                          // 현재 상태의 시작 시간
    public float time => Time.time - startTime;         // 현재 상태가 된 시점으로부터 얼마나 지났는지
 
    // Base
    protected BaseController machineCore;
    protected Animator animator => machineCore.animator;
    protected NavMeshAgent agent => machineCore.agent;



    protected void ChangeState(State newState, bool forceReset = false)
    {
        machineCore.machine.ChangeState(newState, forceReset);
    }


    public void GetBaseMemberVariable(BaseController _machineCore)
    {
        machineCore = _machineCore;
    }


    public void Initialize()
    {
        isComplete = false;
        startTime = Time.time;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { }
}
