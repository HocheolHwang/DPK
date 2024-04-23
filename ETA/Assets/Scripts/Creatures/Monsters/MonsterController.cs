using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MonsterStateItem;

// MosnterStateItem을 만들어서 공통으로 상태를 관리한다면 생기는 문제점이 있나?
// 1. 각 몬스터마다 다른 공격 시스템은 어떻게 구현?             -> 각 몬스터 controller에서 공격 함수를 만든 뒤에 이를 상태에서 실행( IAttack )
// 2. 각 몬스터마다 다른 애니메이션은 어떻게 재생?              -> animation data에 clip을 세팅한 뒤, 이를 실행하면 됨
// 3. 각 몬스터마다 다른 스탯은?                               -> 각 몬스터 controller에서 관리
// 4. 보스 몬스터의 상태 관리는 MonsterStateItem을 사용하나?
//          -> 상태 중간에 코드가 필요하기 때문에 보스는 따로 상태를 가진다.
//          -> 몬스터 상태 클래스( MonsterState )를 정의한 뒤, 이를 상속받는 보스 상태 클래스( BossState )를 만들어서 상태 분기 함수를 만든다.
//          -> 상태 분기 함수로 다른 상태로 이동


//          -> 그러면 MonsterController를 상속 받을 수 없다. 흠.. 아예 따로 BaseController를 상속받는 BOSS만의 Controller를 사용할까?
//          -> 위 방안을 채택할 경우 플레이어가 MonsterController를 이용해서 피해를 줄 수 없다.
public class MonsterController : BaseController
{
    // Monster가 공통으로 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    [Header("Each Controller Property")]
    [SerializeField] public MonsterStat monsterStat;
    [SerializeField] public MonsterAnimationData animData;

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
        // ----------------------------- Component -------------------------------------
        monsterStat = GetComponent<MonsterStat>();

        // ----------------------------- Animation && State -------------------------------------
        animData.StringAnimToHash();

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
    public override void TakeDamage(int damage)
    {
        monsterStat.Hp -= damage;

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (monsterStat.Hp <= 0 && _isDie == false)
        {
            monsterStat.Hp = 0;
            DestroyObject();
        }
    }

    public override void DestroyEvent()
    {
    }
}
