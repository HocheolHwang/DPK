using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MonsterStateItem;
using UnityEngine.AI;

public class MonsterController : BaseMonsterController
{
    // Monster가 공통으로 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    private MonsterAnimationData _animData;

    public MonsterAnimationData AnimData { get => _animData; }

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
        _animData = GetComponent<MonsterAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Agent.stoppingDistance = Detector.AttackRange - 0.3f;      // 멈추는 거리는 공격 사거리의 0.3만큼 뺀 값이다. 이러지 않으면 범위 끝에서 CHASE 상태에서 변하지 않는 현상이 존재
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        base.DestroyEvent();
        // ENUM이 아니라 네이밍 컨벤션을 통해서 Resource Manager를 잘 다루기
    }

    public override void AttackedEvent()
    {
        switch (UnitType)
        {
            case Define.UnitType.Boar:
                Managers.Sound.Play("Monster/Boar/BoarHit_SND", Define.Sound.Effect);
                break;
            case Define.UnitType.Porin:
                Managers.Sound.Play("Monster/Porin/PorinHit_SND", Define.Sound.Effect);
                break;
        }
    }
}
