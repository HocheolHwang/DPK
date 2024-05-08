using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyManStateItem;

public class MummyManController : BaseMonsterController
{
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State THROW_STATE;

    public State CLAP_STATE;

    public State SHOUTING_STATE;
    public State JUMP_STATE;
    public State BACK_LOCATION_STATE;

    public State FORE_SHADOWING_STATE;
    public State RUSH_STATE;

    public State WIND_MILL_STATE;
    
    public State DIE_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;

    private MummyManAnimationData _animData;
    private SummonSkill _summonSkill;
    public MummyManAnimationData AnimData { get => _animData; }
    public SummonSkill SummonSkill { get => _summonSkill; }

    public Action OnBossDestroyed;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<MummyManAnimationData>();
        _animData.StringAnimToHash();
        _summonSkill = GetComponent<SummonSkill>();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        // BASE
        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        // SOMMON
        CLAP_STATE = new ClapState(this);

        SHOUTING_STATE = new ShoutingState(this);
        JUMP_STATE = new JumpState(this);
        BACK_LOCATION_STATE = new BackLocationState(this);
        RUSH_STATE = new RushState(this);
        WIND_MILL_STATE = new WindMillState(this);

        // Global
        DIE_STATE = new DieState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);
        // 공격 사거리와 멈추는 거리를 같게 세팅 -> StateItem의 SetDetector에서 세팅
        UnitType = Define.UnitType.MummyMan;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();

        base.DestroyEvent();
    }
}
