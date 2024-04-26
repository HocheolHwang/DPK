using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KnightGStateItem;

public class MummyManController : BaseController
{
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State THROW_STATE;

    public State CLAP_STATE;

    public State SHOUTING_STATE;
    public State JUMP_STATE;

    public State FORE_SHADOWING_STATE;
    public State RUSH_STATE;
                                            // 돌아오는 상태 추가 -> 원래 자리를 저장하는 변수 필요, 멀리가는 패턴임을 bool 값으로 관리해서 true가 되면 현재 자리를 저장
    public State WIND_MILL_STATE;
    
    public State DIE_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;

    private KnightGAnimationData _animData;
    public KnightGAnimationData KnightGAnimData { get => _animData; }

    public static event Action OnBossDestroyed;

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
        _animData = GetComponent<KnightGAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        //IDLE_STATE = new IdleState(this);
        //IDLE_BATTLE_STATE = new IdleBattleState(this);
        //CHASE_STATE = new ChaseState(this);
        //ATTACK_STATE = new AttackState(this);
        //// Skill
        //TWO_SKILL_TRANSITION_STATE = new TwoSkillTransitionState(this);
        //TWO_SKILL_ENERGY_STATE = new TwoSkillEnergyState(this);
        //TWO_SKILL_ATTACK_STATE = new TwoSkillAttackState(this);
        //// Counter
        //COUNTER_ENABLE_STATE = new CounterEnableState(this);
        //COUNTER_ATTACK_STATE = new CounterAttackState(this);
        //// Phase
        //PHASE_TRANSITION_STATE = new PhaseTransitionState(this);
        //PHASE_ATTACK_STATE = new PhaseAttackState(this);
        //PHASE_ATTACK_ING_STATE = new PhaseAttackingState(this);
        //// Global
        //DIE_STATE = new DieState(this);
        //GROGGY_STATE = new GroggyState(this);
        //GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        agent.stoppingDistance = detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();

        base.DestroyEvent();
    }
}
