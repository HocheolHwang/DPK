using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 현재 상태가 가져야 할 정보와 동작을 다룬다.
/// </summary>
public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }   // 현재 상태가 끝났나?
    protected float startTime;                      // 현재 상태의 시작 시간
    public float time => Time.time - startTime;     // 현재 상태가 된 시점으로부터 얼마나 지났는지
 
    // SMCore
    protected StateMachineCore machineCore;
    protected Animator animator => machineCore.animator;
    protected NavMeshAgent agent => machineCore.agent;
    protected Detector detector => machineCore.detector;

    /// <summary>
    /// [ machineCore가 가진 machine의 ChangeState를 호출 ]
    /// 여기서 machineCore는 각 Agent( Player, Monster )가 들고 있다.
    /// 개발 편의성
    /// </summary>
    protected void ChangeState(State newState, bool forceReset = false)
    {
        machineCore.machine.ChangeState(newState, forceReset);
    }

    /// <summary>
    /// Core에서 관리하는 Component와 GameObject를 가져오기 위함
    /// </summary>
    public void SetCore(StateMachineCore _machineCore)
    {
        machineCore = _machineCore;
    }

    /// <summary>
    /// 현재 상태의 멤버 변수를 초기화한다.
    /// </summary>
    public void Initialize(StateMachine _parent)
    {
        isComplete = false;
        startTime = Time.time;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { }
}
