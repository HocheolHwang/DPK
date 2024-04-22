using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

using BoarStateItem;   // Boar States

public class BoarController : BaseController
{
    // Boar Controller 만 가지는 상태
    public State IDLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State HIT_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    [Header("Each Controller Property")]
    [SerializeField] public Detector detector;
    [SerializeField] public bool isDie;
    [SerializeField] public bool isRevive;      // previous state 테스트용

    [field: Header("Animations")]
    [field: SerializeField] public BoarAnimationData animData;

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
        animData.StringAnimToHash();

        _stateMachine = new StateMachine();
        IDLE_STATE = new IdleState(this);       // using namespace
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        HIT_STATE = new HitState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        // 공격 사거리와 멈추는 거리를 같게 세팅
        agent.stoppingDistance = detector.attackRange;
    }

    //protected override void Update()
    //{
    //    base.Update();
    //}

    // AttackRange 내부에 Target이 있는지 확인
    public bool IsArriveToTarget()
    {
        return Vector3.Distance(detector.Target.position, transform.position) < detector.attackRange;
    }
}
