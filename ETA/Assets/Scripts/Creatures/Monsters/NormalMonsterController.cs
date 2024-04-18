using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

using NormalMonsterStates;   // 이걸로 이름이 중복되도 상관 없으면 Init()에서 사용할 예정

public class NormalMonsterController : BaseController
{
    // Normal Monster Controller만 가지는 상태
    public State IDLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;

    [Header("Each Controller Property")]
    [SerializeField] public Detector detector;
    // 테스트를 위함, 스탯 추가되면 변경될지도?
    [SerializeField] public bool isDie;
    [SerializeField] public bool isRevive;

    private void Start()
    {
        Init();
        ChangeState(IDLE_STATE);
    }

    protected override void Init()
    {
        _stateMachine = new StateMachine();
        IDLE_STATE = new NormalMonsterStates.IdleState(this);
        CHASE_STATE = new NormalMonsterStates.ChaseState(this);
        ATTACK_STATE = new NormalMonsterStates.AttackState(this);
        DIE_STATE = new NormalMonsterStates.DieState(this);
        GLOBAL_STATE = new NormalMonsterStates.GlobalState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        agent.stoppingDistance = detector.attackRange;
    }

    private void Update()
    {
        _stateMachine.Execute();
    }

    public bool IsArriveToTarget()
    {
        return Vector3.Distance(detector.Target.position, transform.position) < detector.attackRange;
    }
}
