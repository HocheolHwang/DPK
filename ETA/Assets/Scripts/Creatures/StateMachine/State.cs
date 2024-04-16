using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 상태가 가져야 할 정보와 동작을 다룬다.
/// </summary>
public abstract class State : MonoBehaviour
{
    public bool isCompelete { get; private set; }   // 현재 상태가 끝났나?
    protected float startTime;                      // 현재 상태의 시작 시간
    public float time => Time.time - startTime;     // 현재 상태가 된 시점으로부터 얼마나 지났는지
 
    // SMCore
    protected StateMachineCore machineCore;
    protected Animator animator => machineCore.animator;
    protected OneDetector detector => machineCore.detector;


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
        isCompelete = false;
        startTime = Time.time;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { }
}
