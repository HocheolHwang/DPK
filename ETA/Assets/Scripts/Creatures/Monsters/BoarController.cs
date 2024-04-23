using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

using BoarStateItem;   // Boar States

public class BoarController : BaseController, IDamageable
{
    // Boar Controller 만 가지는 상태
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    [Header("Each Controller Property")]
    [SerializeField] public bool _isDie;
    [SerializeField] public NormalMonsterStat monsterStat;
    [SerializeField] public BoarAnimationData animData;
    

    public bool IsDie { get => _isDie; set => _isDie = value; }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        ChangeState(IDLE_STATE);
    }

    protected override void Init()
    {
        // ----------------------------- Component -------------------------------------
        monsterStat = GetComponent<NormalMonsterStat>();

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

    // ---------------------------------- Detector ------------------------------------------
    public bool IsArriveToTarget()
    {
        return Vector3.Distance(detector.Target.position, transform.position) < detector.attackRange;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public void TakeDamage(int damage)
    {
        monsterStat.Hp -= damage;

        Debug.Log($"{gameObject.name} has taken {damage} damage.");
        if (monsterStat.Hp < 0 && _isDie == false)
        {
            monsterStat.Hp = 0;
            DestroyEvent();
        }
    }

    public void DestroyEvent()
    {
        _isDie = true;
        Debug.Log($"{gameObject.name} is Die.");
        // 파괴, 이펙트, 소리, UI 등 다양한 이벤트 추가

        // 애니메이션은 상태에서 관리 중
        Destroy(this, 0.5f);
    }
}
