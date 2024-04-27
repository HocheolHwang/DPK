using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyWarriorStateItem;
using System;

public class MummyWarriorController : BaseController
{
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State WIND_MILL_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    private MummyWarriorAnimationData _animData;
    public MummyWarriorAnimationData AnimData { get => _animData; }

    public static event Action<MummyWarriorController> OnDeath;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<MummyWarriorAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        WIND_MILL_STATE = new WindMillState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Agent.stoppingDistance = Detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        base.DestroyEvent();

        // Mummy Man 한테 죽었음을 알려주기
        OnDeath?.Invoke(this);
        // MummyWarriorController.OnDeath -= OnDeath;
    }
}
