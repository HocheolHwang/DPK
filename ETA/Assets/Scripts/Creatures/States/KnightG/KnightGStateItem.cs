using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightGStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : KnightGState
    {
        public IdleState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (_detector.Target != null)
            {
                _controller.ChangeState(_controller.CHASE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- IDLE BATTLE ------------------------------------------------
    #region IDLE_BATTLE
    public class IdleBattleState : KnightGState
    {
        // CHASE -> IDLE BATTLE -> ATTACK
        public IdleBattleState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.transform.LookAt(_detector.Target);
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (IsStayForSeconds())
            {
                _controller.ChangeState(_controller.ATTACK_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- CHASE ------------------------------------------------
    #region CHASE
    public class ChaseState : KnightGState
    {
        public ChaseState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.speed = _stat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (_detector.Target == null)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            if (_controller.IsArriveToTarget())
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }

            _agent.SetDestination(_detector.Target.position);
        }

        public override void Exit()
        {
            _agent.ResetPath();
        }
    }
    #endregion

    // -------------------------------------- ATTACK ------------------------------------------------
    #region ATTACK
    public class AttackState : KnightGState
    {
        // 한 번 재생한 뒤에 다른 상태로 전환
        // 1. Target NULL -> IDLE
        // 2. IsArriveToTarget() false -> CHASE
        // 3. IsArriveToTarget() true -> IDLE_BATTLE
        float _attackTime;
        float _threadHold;

        public AttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _attackTime = 0;
            _threadHold = _animData.AttackAnim.length;

            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절

            // 2가지 자동 공격 모션이 존재한다.
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);
        }

        public override void Execute()
        {
            _attackTime += Time.deltaTime;
            if (_attackTime > _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
            {
                if (_detector.Target == null)
                {
                    _controller.ChangeState(_controller.IDLE_STATE);
                }

                if (_controller.IsArriveToTarget())
                {
                    _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
                }
                else
                {
                    _controller.ChangeState(_controller.CHASE_STATE);
                }
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : KnightGState
    {
        public DieState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.isStopped = true;
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
        }

        public override void Execute()
        {
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------
    #region GROGGY
    public class GroggyState : KnightGState
    {
        public GroggyState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Debug.Log("KnightG Groggy");
            _agent.isStopped = true;
            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (IsStayForSeconds())
            {
                _controller.RevertToPrevState();
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : KnightGState
    {
        public GlobalState(KnightGController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.GROGGY_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= 0)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            if (_controller.IsStun)
            {
                Debug.Log($"{_controller.IsStun}");
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
        }
    }
    #endregion
}
