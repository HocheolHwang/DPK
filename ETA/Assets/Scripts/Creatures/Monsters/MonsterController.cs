using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MonsterStateItem;

public class MonsterController : BaseController
{
    // Monster가 공통으로 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    [Header("Monster Controller Property")]
    protected MonsterAnimationData _animData;

    public MonsterAnimationData AnimData { get => _animData; }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected virtual void Start()
    {
        ChangeState(IDLE_STATE);
    }

    protected override void Init()
    {
        _stat = GetComponent<MonsterStat>();
        _animData = GetComponent<MonsterAnimationData>();

        // ----------------------------- Animation && State -------------------------------------
        _animData.StringAnimToHash();

        _stateMachine = new StateMachine();
        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        agent.stoppingDistance = detector.attackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
    }
}
