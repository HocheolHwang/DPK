using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class MummyManState : State
{
    protected static bool _meetPlayer;                     // 플레이어와 첫 조우 여부
    protected static bool _isRangedAttack = true;          // 원거리 디텍터를 활성화한 상태
    protected const int MaxSummonCount = 1;                // 첫 조우 이후에 Buffer와 Warrior가 살아날 수 있는 횟수

    protected float _curAttackRange;                         // 비활성화 상태에서 가장 맨 위에 있는 컴포넌트의 attack range가 적용됨

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
    protected void ControlChangeState()
    {
        if (_detector.Target == null)
        {
            _controller.ChangeState(_controller.IDLE_STATE);
        }

        if (_detector.IsArriveToTarget())
        {
            _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
        }
        else
        {
            _controller.ChangeState(_controller.CHASE_STATE);
        }
    }

    // -------------------------- GLOBAL FUNCTIONS -----------------------------------
    public void CheckGlobal()
    {
        SetDetector();

        if (_stat.Hp <= 0)
        {
            _controller.ChangeState(_controller.DIE_STATE);
        }

        // 둘 다 한 번씩 죽었을 때, 한 번 더 살려낸다.
        if (IsSummoningMonster())
        {
            if ((_controller.CurState == _controller.IDLE_STATE) || (_controller.CurState == _controller.IDLE_BATTLE_STATE) || (_controller.CurState == _controller.CHASE_STATE))
                _controller.ChangeState(_controller.CLAP_STATE);
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
            _curAttackRange = _detector.AttackRange;
            Debug.Log($"range: {_detector.AttackRange} | isRangedAttack: {_isRangedAttack}");
        }
        else
        {
            _controller.GetComponent<MeleeDetector>().enabled = true;
            _controller.GetComponent<RangedDetector>().enabled = false;
            _detector = _controller.GetComponent<MeleeDetector>();

            _agent.stoppingDistance = _detector.AttackRange;
            _curAttackRange = _detector.AttackRange;
            Debug.Log($"range: {_detector.AttackRange} | isRangedAttack: {_isRangedAttack}");
        }
    }

    private bool IsSummoningMonster()
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
}
