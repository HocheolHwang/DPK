using Photon.Pun.Demo.Cockpit.Forms;
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


    //--------------------------- Common Property From BaseController -----------------------------------
    protected Animator _animator;
    protected NavMeshAgent _agent;

    public State(BaseController controller)
    {
        _animator = controller.animator;
        _agent = controller.agent;
    }

    //--------------------------- State Functions -----------------------------------
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
