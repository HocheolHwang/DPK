using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운터 실패시 그로기 상태
// 페이즈 실패시 그로기 상태

// 패턴 공격 발생 기준?
// 체력이 일정 기준 이하일 때 패턴 공격
// 카운터는 내부 쿨타임을 기준으로 수행


// 카운터 패턴 공격 1가지

// 일반 패턴 1가지
// 숨겨진 기믹이 있는 패턴 1가지

// 페이즈

// 페이즈를 끊지 못하면 발생하는 패턴 1가지
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
        // IDLE BATTLE -> ATTACK
        // IDLE BATTLE -> COUNTER ENABLE( counterTime >= threadHoldCounter )
        // 
        public IdleBattleState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            LookAtEnemy();
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (counterTime >= threadHoldCounter)
            {
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
            }

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
                if (_controller.IsEnterPhaseTwo)
                {
                    _controller.ChangeState(_controller.PHASE_ATTACK_STATE);
                }
                else
                {
                    _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
                }
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
        public AttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            attackCnt++;
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절

            // 2가지 자동 공격 모션이 존재한다.
            if (attackCnt % 2 == 0)
            {
                InitTime(_animData.AttackAnim.length);
                _animator.CrossFade(_animData.AttackParamHash, 0.4f);

            }
            else if (attackCnt % 2 == 1)
            {
                InitTime(_animData.AttackUpAnim.length);
                _animator.CrossFade(_animData.AttackUpParamHash, 0.4f);
            }
            
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
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

    // -------------------------------------- COUNTER_ENABLE ------------------------------------------------
    #region COUNTER_ENABLE
    public class CounterEnableState : KnightGState
    {
        public CounterEnableState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.CounterEnableAnim.length);

            _animator.SetFloat("CounterEnableSpeed", 0.5f);
            _animator.CrossFade(_animData.CounterEnableParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)
            {
                _controller.ChangeState(_controller.COUNTER_ATTACK_STATE);
            }

            // 여기서 카운터 스킬에 맞았을 경우 그로기 상태로 전환
        }
        public override void Exit()
        {
            counterTime = 0;
        }
    }
    #endregion

    // -------------------------------------- COUNTER_ATTACK ------------------------------------------------
    #region COUNTER_ATTACK
    public class CounterAttackState : KnightGState
    {
        public CounterAttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.CounterAttackAnim.length);
            _animator.CrossFade(_animData.CounterAttackParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- PHASE_TRANSITION ------------------------------------------------
    #region PHASE_TRANSITION
    public class PhaseTransitionState : KnightGState
    {
        public PhaseTransitionState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.PhaseTransitionAnim.length);
            _animator.CrossFade(_animData.PhaseTransitionParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if ( _animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.PHASE_ATTACK_STATE);
            }

            // Phase 진입을 끊는 스킬에 맞으면 그로기 상태로 전환
            // 2페이즈 진입 실패로 인해 1페이즈 스킬로만 공격할 수 있음
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- PHASE_ATTACK ------------------------------------------------
    #region PHASE_ATTACK
    public class PhaseAttackState : KnightGState
    {
        public PhaseAttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _controller.IsEnterPhaseTwo = true;

            LookAtEnemy();
            InitTime(_animData.PhaseAttackAnim.length);
            _animator.CrossFade(_animData.PhaseAttackParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.PHASE_ATTACK_ING_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- PHASE_ATTACK_ING ------------------------------------------------
    #region PHASE_ATTACK_ING
    public class PhaseAttackingState : KnightGState
    {
        public PhaseAttackingState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _animator.CrossFade(_animData.PhaseAttackingParamHash, 0.1f, -1, 0.0f);
        }

        public override void Execute()
        {
            LookAtEnemy();
            // 타겟팅한 한 명의 적만 계속 공격하는 패턴
            if (_detector.Target == null || !_controller.IsArriveToTarget())
            {
                _controller.ChangeState(_controller.IDLE_STATE);
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
            _agent.velocity = Vector3.zero;
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
            // 카운터 패턴 공격 시간만 여기서 관리
            // 수행하는 코드는 IDLE, IDLE_BATTLE, CHASE 중에서 하나
            counterTime += Time.deltaTime;

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.GROGGY_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= 0)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            if ( !_controller.IsEnterPhaseTwo && _stat.Hp <= (_stat.MaxHp * 0.3))
            {
                _controller.ChangeState(_controller.PHASE_TRANSITION_STATE);
            }
            if (_controller.IsStun)
            {
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
        }
    }
    #endregion
}
