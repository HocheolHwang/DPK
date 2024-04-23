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
//          -> 몬스터 상태 클래스( MonsterState )는 일반 몬스터를 담당하고,
//          -> 보스 몬스터 상태는 따로 각자의 클래스와 Item을 만든다.


//          -> 그러면 MonsterController를 상속 받을 수 없다. 흠.. 아예 따로 BaseController를 상속받는 BOSS만의 Controller를 사용할까?
//          -> 위 방안을 채택할 경우 플레이어가 MonsterController를 이용해서 피해를 줄 수 없다.

// 결론:
// MonsterController로 NormalMonster를 관리하고 BossMonster는 MonsterController를 상속받아서 사용한다.
// MonsterController에서 MonsterStat을 이용하여 Damage 기능을 처리한다.
// 1. 공격 시스템
//      1.1. MonsterController가 통합됐기 때문에 Controller가 아닌 다른 방법이 필요하다.
//      1.2. 저번 프로젝트에서는 Controller는 동일하지만 StateItem을 몬스터별로 만들었다.
//      1.3. MonsterState에 공격 함수를 만든 뒤, Unit Type을 만들어서 ENUM으로 관리할까?
//      1.4. 스킬 시스템이 나온 뒤에 결정해도 될 것 같다.
// 2. 애니메이션
//      2.1. MonsterController에서 사용하는 상태의 이름을 통일해서 사용한다. - animator에서도 통일
//      2.2. MonsterAnimationData를 상속받는 각 보스의 AnimationData를 생성한다. 여기서 보스마다 가진 애니메이션을 관리
// 3. 상태 시스템
//      3.1. 각 보스 몬스터마다 State 클래스와 StateItem을 만든다.
// 4. 스탯 시스템
//      4.1. 보스 몬스터만 가진 스탯은 어떻게 관리할까? 아니 특정 몬스터만 가지는 스탯이 존재하나?
//      4.2. 만약 특정 보스만 가지는 스탯이 존재한다면, 
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
    public override void TakeDamage(int attackDamage)
    {
        // 최소 데미지 = 1
        int damage = Mathf.Abs(attackDamage - monsterStat.Defense);
        if (damage <= 1)
        {
            damage = 1;
        }
        monsterStat.Hp -= damage;

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (monsterStat.Hp <= 0)
        {
            monsterStat.Hp = 0;
            DestroyObject();
        }
    }

    public override void DestroyEvent()
    {
    }
}
