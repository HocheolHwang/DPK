using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using UnityEngine;

enum EMummyManPattern
{
    RangedAutoAttack        = 0,
    MeleeAutoAttack,
    MAX_LEN
}

public class MummyManState : State
{
    protected static bool _meetPlayer;                     // 플레이어와 첫 조우 여부
    protected static bool _isRangedAttack = true;          // 원거리 디텍터를 활성화한 상태
    protected static bool _isRush;                         // Rush Pattern 수행
    protected const int MaxSummonCount = 1;                // 첫 조우 이후에 Buffer와 Warrior가 살아날 수 있는 횟수
    

    protected static float _shoutingTime;
    protected static float _threadHoldShouting = 14.0f;
    protected static float _jumpTime;
    protected static float _threadHoldJump = 30.5f;        // 30.5초

    protected static Transform _target;
    protected static float _attackRange;
    protected static Vector3 _startPos;
    protected static Vector3 _destPos;

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
        if (_target == null)
        {
            _controller.ChangeState(_controller.IDLE_STATE);
        }

        if (_detector.IsArriveToTarget(_target, _attackRange))
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

    protected bool IsSommoningMonster()
    {
        // 플레이어를 만났고
        if (_meetPlayer)
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

    protected void DeadWarriorEvent()
    {
        if (_meetPlayer && (_summonSkill.WarriorDeathCount == MaxSummonCount))
        {
            _isRangedAttack = false;
            return;
        }
    }

    protected bool IsDeadBuffer()
    {
        if (_meetPlayer && (_summonSkill.BufferDeathCount == MaxSummonCount) && !_isRush)
        {
            _isRush = true;
            return true;
        }
        return false;
    }
    #endregion

    // -------------------------- JUMP && BACK_LOCATION FUNCTIONS -----------------------------------

    // Pattern에서 구현하도록 수정
    #region JUMP AND BACK_LOCATION FUNCTIONS

    // pattern에서 구현할지 결정
    protected void JumpToTarget(float deltaTime)   // 점프 상태일 때는 forward지만, BACK_LOCATION 상태일 때는 뒤로 돌고 forward이다.
    {
        if (Vector3.Distance(_startPos, _destPos) <= 0.1f)
        {
            _controller.transform.position = _destPos;
            return;
        }

        // destPos 방향을 바라본다.
        _controller.transform.LookAt(_destPos);

        // duration초 만큼 이동한다.
        Vector3 moveStopPos = Vector3.Lerp(_startPos, _destPos, deltaTime);
        _controller.transform.position = moveStopPos;
    }

    protected void SetStartAndDestPos(Vector3 startPos, Vector3 destPos)
    {
        _startPos = startPos;
        _destPos = destPos;
    }
    #endregion

    // -------------------------- RUSH FUNCTIONS -----------------------------------
    public bool IsPreviousState()
    {
        // wind mill 추가
        
        if (_controller.PrevState == _controller.JUMP_STATE) return true;
        if (_controller.PrevState == _controller.RUSH_STATE) return true;
        if (_controller.PrevState == _controller.WIND_MILL_STATE) return true;

        return false;
    }

    public float CalcTimeToDest(Vector3 Destination)
    {
        float moveSpeed = _controller.Stat.MoveSpeed;
        if (moveSpeed <= 0.1f)
        {
            Debug.Log($"{_controller.gameObject.name}의 속도({moveSpeed})가 0.1f보다 낮습니다.");
            return -1;
        }
        else if (moveSpeed > 8.0f)
        {
            moveSpeed = 8.0f;
        }

        float remainDist = Vector3.Distance(Destination, _controller.transform.position);
        if (remainDist < 5.0f)
        {
            remainDist = 5.0f;
        }

        float timeToDest = remainDist / moveSpeed;
        return timeToDest;
    }

    // pattern에서 이동 구현
    public void RushToTarget()
    {
        // agent speed를 N배 증가 또는 일정 수치를 할당
        _agent.speed = 10.0f;

        // 목적지까지 agent를 이동
        _agent.SetDestination(_destPos);

        // agent.speed = Stat.MoveSpeed 원상복구
    }

    // -------------------------- GLOBAL FUNCTIONS -----------------------------------
    #region GLOBAL_FUNCTIONS
    public void CheckGlobal()
    {
        SetDetector();

        if (_stat.Hp <= 0)
        {
            _controller.ChangeState(_controller.DIE_STATE);
        }
    }

    protected void SetDetector()
    {
        if (_isRangedAttack && _summonSkill.WarriorDeathCount == 1)
        {
            _isRangedAttack = false;
        }

        // Target이 null인 문제를 해결하기 위해서 어느 Detector를 사용하는지 지정
        if (_isRangedAttack)
        {
            _controller.GetComponent<MeleeDetector>().enabled = false;
            _controller.GetComponent<RangedDetector>().enabled = true;
            _detector = _controller.GetComponent<RangedDetector>();

            _agent.stoppingDistance = _detector.AttackRange;
            _target = _detector.Target;
            _attackRange = _detector.AttackRange;
        }
        else
        {
            _controller.GetComponent<MeleeDetector>().enabled = true;
            _controller.GetComponent<RangedDetector>().enabled = false;
            _detector = _controller.GetComponent<MeleeDetector>();

            _agent.stoppingDistance = _detector.AttackRange;
            _target = _detector.Target;
            _attackRange = _detector.AttackRange;
        }
    }
    #endregion
}
