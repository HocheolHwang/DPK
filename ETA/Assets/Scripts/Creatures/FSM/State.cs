using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class State : IState
{
    private bool _isComplete;   // 현재 상태가 끝났나?
    private float _startTime;   // 현재 상태의 시작 시간
    private float _executeTime; // 현재 상태가 된 시점으로부터 얼마나 지났는지
    public bool IsComplete { get => _isComplete; set => _isComplete = value; }
    public float StartTime { get => _startTime; set => _startTime = value; }
    public float ExecuteTime { get => _executeTime; set => _executeTime = Time.time - StartTime; }


    // Base
    protected BaseController _controller;
    protected Animator _animator => _controller.animator;
    protected NavMeshAgent _agent => _controller.agent;

    
    protected void ChangeState(State newState)
    {
        _controller.Machine.ChangeState(newState);
    }


    public void GetBaseMemberVariable(BaseController controller)
    {
        _controller = controller;
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
