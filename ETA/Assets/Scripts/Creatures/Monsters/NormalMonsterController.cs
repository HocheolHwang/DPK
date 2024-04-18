using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 일반 몬스터 : 근거리 공격, 1인 타격, 가장 가까운 적을 타겟팅
/// </summary>

public enum MonsterState
{
    IDLE = 0,
    //CHASE,
    //ATTACK,
    //GLOBAL_DIE,

    GlOBAL,
    MAX_LEN
}

public class NormalMonsterController : BaseController
{
    [Header("Each Controller Property")]
    [SerializeField] public Detector detector;

    // Normal Monster만 가진 상태 또는 전략
    private State[] _states;


    private void Start()
    {
        Init();
        ChangeState(_states[(int)MonsterState.IDLE]);
    }

    protected override void Init()
    {
        Machine = new StateMachine();
        _states = new State[(int)MonsterState.MAX_LEN];
        _states[(int)MonsterState.IDLE] = new NormalMonster.IdleState();

        foreach (State state in _states)
        {
            state.GetBaseMemberVariable(this);      // BaseController를 각 State로 넘기기 위해서 enum list 사용
            Debug.Log(state.ToString());
        }
        agent.stoppingDistance = detector.attackRange;
    }

    private void Update()
    {
        Machine.Execute();
    }

    public bool IsArriveToTarget()
    {
        if (detector.Target == null) return true;
        return agent.remainingDistance <= agent.stoppingDistance;
    }
}
