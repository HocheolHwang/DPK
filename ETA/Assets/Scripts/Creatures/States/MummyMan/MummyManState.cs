using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

enum EMummyManPattern
{
    RangedAutoAttack        = 0,
    MeleeAutoAttack,
    WindMill,
    Jump,
    Shouting,
    Rush,
    CounterEnable,
    Buff,
    MAX_LEN
}

public class MummyManState : State
{
    protected const int MaxSummonCount = 1;         // 첫 조우 이후에 Buffer와 Warrior가 살아날 수 있는 횟수
    protected const float RushTime = 2.0f;

    protected MummyManController _controller;
    protected MummyManAnimationData _animData;
    protected SummonSkill _summonSkill;

    public MummyManState(MummyManController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
        _summonSkill = controller.SummonSkill;
    }

    // -------------------------- ATTACK FUNCTIONS -----------------------------------

    #region ATTACK FUNCTIONS
    protected void ControlChangeState() // 근접 + 3타 중간에 있는 상태 전환 조건
    {
        if (_controller.Target == null)
        {
            _controller.ChangeState(_controller.IDLE_STATE);
        }

        if (_detector.IsArriveToTarget(_controller.Target, _controller.AttackRange))
        {
            _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
        }
        else
        {
            _controller.ChangeState(_controller.CHASE_STATE);
        }
    }
    #endregion

    // -------------------------- IDLE_BATTLE FUNCTIONS -----------------------------------
    #region IDLE_BATTLE FUNCTIONS

    protected void SetDetector()
    {
        if (_controller.IsRangedAttack && (_summonSkill.WarriorDeathCount == MaxSummonCount))
        {
            _controller.IsRangedAttack = false;
        }

        // Target이 null인 문제를 해결하기 위해서 어느 Detector를 사용하는지 지정
        if (_controller.IsRangedAttack)
        {
            _controller.GetComponent<MeleeDetector>().enabled = false;
            _controller.GetComponent<RangedDetector>().enabled = true;
            _detector = _controller.GetComponent<RangedDetector>();

            _agent.stoppingDistance = _detector.AttackRange;
            _controller.Target = _detector.Target;
            _controller.AttackRange = _detector.AttackRange;
        }
        else
        {
            _controller.GetComponent<MeleeDetector>().enabled = true;
            _controller.GetComponent<RangedDetector>().enabled = false;
            _detector = _controller.GetComponent<MeleeDetector>();

            _agent.stoppingDistance = _detector.AttackRange;
            _controller.Target = _detector.Target;
            _controller.AttackRange = _detector.AttackRange;
        }
    }

    protected bool IsSommoningMonster()
    {
        // 플레이어를 만났고
        if (_controller.MeetPlayer)
        {
            // 한 번씩 소환했으며
            if (_summonSkill.BufferSummonCount == MaxSummonCount && _summonSkill.WarriorSummonCount == MaxSummonCount)
            {
                // 모두 한 번만 죽은 경우
                if (_summonSkill.BufferDeathCount == MaxSummonCount && _summonSkill.WarriorDeathCount == MaxSummonCount)
                    return true;
            }
        }
        return false;
    }

    protected void SommonMonsterEvent()
    {
        // 둘 다 한 번씩 죽었을 때, 한 번 더 살려낸다.
        if (IsSommoningMonster())
        {
            if ((_controller.CurState == _controller.IDLE_STATE) || (_controller.CurState == _controller.IDLE_BATTLE_STATE) || (_controller.CurState == _controller.CHASE_STATE))
                _controller.ChangeState(_controller.CLAP_STATE);
        }
    }

    protected bool IsDeadBuffer()
    {
        if (_controller.MeetPlayer && (_summonSkill.BufferDeathCount == MaxSummonCount) && !_controller.IsRush)
        {
            _controller.IsRush = true;
            return true;
        }
        return false;
    }
    #endregion

    // -------------------------- JUMP && BACK_LOCATION FUNCTIONS -----------------------------------

    #region JUMP AND BACK_LOCATION FUNCTIONS

    // 시간 없어서 pattern으로 안 옮김
    // 상태에는 상태에 관련된 함수만 가지고 싶음
    protected void JumpToTarget(float deltaTime)   // 점프 상태일 때는 forward지만, BACK_LOCATION 상태일 때는 뒤로 돌고 forward이다.
    {
        if (Vector3.Distance(_controller.StartPos, _controller.DestPos) <= 0.1f)
        {
            _controller.transform.position = _controller.DestPos;
            return;
        }

        // destPos 방향을 바라본다.
        _controller.transform.LookAt(_controller.DestPos);

        Vector3 moveStopPos = Vector3.Lerp(_controller.StartPos, _controller.DestPos, deltaTime);
        _controller.transform.position = moveStopPos;
    }

    protected void SetStartAndDestPos(Vector3 startPos, Vector3 destPos)
    {
        _controller.StartPos = startPos;
        _controller.DestPos = destPos;
    }
    #endregion

    // -------------------------- RUSH FUNCTIONS -----------------------------------
    #region RUSH
    public bool IsPreviousState()
    {
        // wind mill 추가
        
        if (_controller.PrevState == _controller.JUMP_STATE) return true;
        if (_controller.PrevState == _controller.RUSH_STATE) return true;
        if (_controller.PrevState == _controller.WIND_MILL_STATE) return true;

        return false;
    }

    public float CalcSpeedFromDestTime(Vector3 Destination)
    {
        float remainDist = Vector3.Distance(Destination, _controller.transform.position);
        
        return remainDist / RushTime;
    }

    #region Temp
    //protected float CalcTimeToDest(Vector3 Destination)
    //{
    //    NavMeshPath path = new NavMeshPath();
    //    Destination.y = _controller.transform.position.y;

    //    // 현재 위치에서 목적지까지의 경로 계산
    //    if (_agent.CalculatePath(Destination, path))
    //    {
    //        float pathLen = GetPathLength(path);
    //        return pathLen / _agent.speed;
    //    }

    //    return -1;
    //}
    //protected float GetPathLength(NavMeshPath path)
    //{
    //    float len = 0;

    //    if (path.corners.Length < 2)
    //    {
    //        Debug.Log("유효하지 않은 경로");
    //        return len;
    //    }

    //    // 경로상의 모든 코너점을 순회하면서 두 점 사이의 거리를 구함
    //    for (int i = 0; i < path.corners.Length - 1; ++i)
    //    {
    //        len += Vector3.Distance(path.corners[i], path.corners[i + 1]);
    //    }

    //    return len;
    //}
    #endregion

    #endregion

    // -------------------------- GLOBAL FUNCTIONS -----------------------------------
    #region GLOBAL_FUNCTIONS
    public void CheckGlobal()
    {
        if (_stat.Hp <= 0)
        {
            _controller.ChangeState(_controller.DIE_STATE);
        }
    }

    protected void UpdateTarget()
    {
        if (_controller.IsRangedAttack)
        {
            _detector = _controller.GetComponent<RangedDetector>();
            _controller.Target = _detector.Target;
        }
        else
        {
            _detector = _controller.GetComponent<MeleeDetector>();
            _controller.Target = _detector.Target;
        }
    }
    #endregion
}
