using Photon.Pun.Demo.Cockpit.Forms;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class State : IState
{
    protected float _animTime;                          // 각 상태의 animation 수행 시간
    protected float _threadHold;                        // 각 상태의 animation 끝나는 시간

    private bool _isComplete;                           // 현재 상태가 끝났나?
    private float _startTime;                           // 현재 상태의 시작 시간
    public bool IsComplete { get => _isComplete; set => _isComplete = value; }
    public float StartTime { get => _startTime; set => _startTime = value; }
    public float ExecuteTime { get => Time.time - StartTime; }  // 현재 상태가 된 시점으로부터 얼마나 지났는지


    //--------------------------- Common Property From BaseController -----------------------------------
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected IDetector _detector;
    protected StateMachine _stateMachine;
    protected Stat _stat;

    public State(BaseController controller)
    {
        _animator = controller.Animator;
        _agent = controller.Agent;
        _detector = controller.Detector;
        _stateMachine = controller.StateMachine;
        _stat = controller.Stat;
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

    //--------------------------- Each State Common Functions -----------------------------------
    public bool IsStayForSeconds(float seconds = 0.5f)
    {
        if (ExecuteTime < seconds) return false;
        return true;
    }

    public void InitTime(float animLength)
    {
        _animTime = 0;
        _threadHold = animLength;
    }

    public void LookAtEnemy()
    {
        if (_detector.Target == null) return;
        Vector3 dir = _detector.Target.position;
        dir.y = _animator.transform.position.y;
        _animator.transform.LookAt(dir);
    }
}
