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
    //DIE,
    MAX_LEN
}

public class NormalMonster : BaseController
{
    // Normal Monster만 가진 상태 또는 전략
    private State[] _states;

    public Detector _detector;


    private void Start()
    {
        Init();
        ChangeState(_states[(int)MonsterState.IDLE]);
    }

    protected override void Init()
    {
        _machine = new StateMachine();
        _states = new State[(int)MonsterState.MAX_LEN];
        _states[(int)MonsterState.IDLE] = new IdleState();

        foreach (State state in _states)
        {
            state.GetBaseMemberVariable(this);      // BaseController를 각 State로 넘기기 위해서 enum list 사용
            Debug.Log(state.ToString());
        }
        _agent.stoppingDistance = _detector._attackRange;
    }

    private void Update()
    {
        StateSelector();
    }

    private void StateSelector()
    {
        //if (_curState._isComplete)
        //{
        //    if (_curState == GlobalState)
        //    {
        //        GlobalState = null;
        //        ChangeState(PrevState);
        //    }
        //}
        //// 현재 상태가 끝난 경우
        //// Idle은 target이 존재하면 isComplete
        //if (machine.curState.isComplete)
        //{
        //    // idle 상태가 끝난 경우
        //    if (machine.curState == idleState)
        //    {
        //        ChangeState(chaseStrategy);
        //    }
        //    // 공격 전략이 끝난 경우
        //    else if (machine.curState == attackStrategy)
        //    {
        //        ChangeState(chaseStrategy);
        //    }
        //}

        //// 추격 전략을 수행하고 있는 경우
        //if (machine.curState == chaseStrategy)
        //{
        //    // 공격 범위에 적이 있으면 공격한다.
        //    if (/*detector.CheckWithinAttackRange()*/IsArriveAgent())
        //    {
        //        ChangeState(attackStrategy);
        //    }
        //}

        //// 적이 없는 경우
        //if (detector.target == null)
        //{
        //    ChangeState(idleState);
        //}

    }

    public bool IsArriveToTarget()
    {
        if (_detector._target == null) return true;
        return _agent.remainingDistance <= _agent.stoppingDistance;
    }
}
