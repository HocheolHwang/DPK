using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KnightGStateItem;
// Monster Controller에서 상속 받는 경우
// 1. StateItem을 따로 사용하기 때문에 namespace가 일치하지 않아서 monster가 가지는 IDLE, IDLE_BATTLE, CHASE, ... 등의 상태를 초기화 할 수 없다.
//    또한 StateItem은 일반 몬스터를 제외하면 각 보스 몬스터에 종속되기 때문에 초기화 할 수도 없다.
// 2. Monster와 KnightG의 초기화가 다르기 때문에 발생하는 문제
// 결론: BaseController에서 상속 받는다.
public class KnightGController : BaseController
{
    // KnightG가 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State TWO_SKILL_TRANSITION_STATE;
    public State TWO_SKILL_ENERGY_STATE;
    public State TWO_SKILL_ATTACK_STATE;
    public State COUNTER_ENABLE_STATE;
    public State COUNTER_ATTACK_STATE;
    public State PHASE_TRANSITION_STATE;
    public State PHASE_ATTACK_STATE;
    public State PHASE_ATTACK_ING_STATE;
    public State DIE_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;

    private KnightGAnimationData _animData;
    private bool _isEnterPhaseTwo;                 // Phase 진입 여부
    public KnightGAnimationData KnightGAnimData { get => _animData; }
    public bool IsEnterPhaseTwo { get => _isEnterPhaseTwo; set => _isEnterPhaseTwo = value; }

    public bool IsStun;
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

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        // Skill
        TWO_SKILL_TRANSITION_STATE = new TwoSkillTransitionState(this);
        TWO_SKILL_ENERGY_STATE = new TwoSkillEnergyState(this);
        TWO_SKILL_ATTACK_STATE = new TwoSkillAttackState(this);
        // Counter
        COUNTER_ENABLE_STATE = new CounterEnableState(this);
        COUNTER_ATTACK_STATE = new CounterAttackState(this);
        // Phase
        PHASE_TRANSITION_STATE = new PhaseTransitionState(this);
        PHASE_ATTACK_STATE = new PhaseAttackState(this);
        PHASE_ATTACK_ING_STATE = new PhaseAttackingState(this);
        // Global
        DIE_STATE = new DieState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        agent.stoppingDistance = detector.attackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();
    }
}
