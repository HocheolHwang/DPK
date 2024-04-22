using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoarStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : BoarState
    {
        public IdleState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if ( IsStayForSeconds() && _detector.Target != null)
            {
                if (_controller.IsArriveToTarget())
                {
                    _controller.ChangeState(_controller.ATTACK_STATE);
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

    // -------------------------------------- CHASE ------------------------------------------------
    #region CHASE
    public class ChaseState : BoarState
    {
        public ChaseState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.speed = _monsterStat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if ( _detector.Target != null)
            {
                if (_controller.IsArriveToTarget())
                {
                    _controller.ChangeState(_controller.ATTACK_STATE);
                }
                _agent.SetDestination(_detector.Target.position);
            }
            else
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
            _agent.ResetPath();
        }
    }
    #endregion

    // -------------------------------------- ATTACK ------------------------------------------------
    #region ATTACK
    public class AttackState : BoarState
    {
        float _attackCnt;
        float _threadHold;

        public AttackState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _attackCnt = 0;
            _threadHold = _animData.AttackAnim.length;


            _controller.transform.LookAt(_detector.Target);
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);   // Idle과 Attack 애니메이션 모션 차이 때문에 들썩이는 모습을 막을 수 없는 것 같다.
        }

        public override void Execute()
        {
            _attackCnt += Time.deltaTime;

            if (_detector.Target == null )
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if ( !_controller.IsArriveToTarget() )
            {
                _controller.ChangeState(_controller.CHASE_STATE);
            }

            if (_attackCnt > _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            } 
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- HIT ------------------------------------------------
    #region Hit
    public class HitState : BoarState
    {
        float _hitCnt;
        float _threadHold;

        public HitState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            _hitCnt = 0;
            _threadHold = _animData.HitAnim.length;

            _animator.CrossFade(_animData.HitParamHash, 0.1f);
        }

        public override void Execute()
        {
            _hitCnt += Time.deltaTime;
            
            if (_hitCnt > _threadHold)
            {
                _controller.RevertToPrevState();
            }
        }
        public override void Exit()
        {
            _controller.IsDamaged = false;
        }
    }
    #endregion

    // -------------------------------------- DIE ------------------------------------------------
    #region DIE
    public class DieState : BoarState
    {
        public DieState(BoarController controller) : base(controller)
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

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : BoarState
    {
        public GlobalState(BoarController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.HIT_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_controller.IsDie)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            else if (_controller.IsDamaged) // 레퍼런스인 쿠키런 킹덤은 피격 모션이 없다.
            {
                _controller.ChangeState(_controller.HIT_STATE);
            }
        }
    }
    #endregion
}
