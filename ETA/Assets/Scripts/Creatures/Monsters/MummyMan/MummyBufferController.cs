using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyBufferStateItem;
using System;

public class MummyBufferController : BaseMonsterController
{
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State BUFF_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    private MummyBufferAnimationData _animData;
    public MummyBufferAnimationData AnimData { get => _animData; }
    public Action OnDeath;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Start()
    {
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<MummyBufferAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        BUFF_STATE = new BuffState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Detector.DetectRange *= 2;
        Agent.stoppingDistance = Detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
        UnitType = Define.UnitType.MummyManBuffer;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 죽었을 경우 보내는 ACTION
        OnDeath?.Invoke();
        Debug.Log("Buffer Die");

        base.DestroyEvent();
    }
}
