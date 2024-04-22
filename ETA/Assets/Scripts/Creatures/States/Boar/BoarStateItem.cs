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

    // -------------------------------------- CHASE ------------------------------------------------
    #region CHASE
    public class ChaseState : BoarState
    {
        // 몬스터끼리 뭉쳐지지 말고 경로에 몬스터가 있으면 피해서 이동하도록 수정 - Enter에서 하는 경우 Detector가 계속 
        // 몬스터가 Target을 향해 바로 회전하도록 수정
        public ChaseState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enter CHASE");
            _agent.speed = 3.0f;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if ( _detector.Target != null)
            {
                _agent.SetDestination(_detector.Target.position);
                if (_controller.IsArriveToTarget())
                {
                    _controller.ChangeState(_controller.ATTACK_STATE);
                }
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
        public AttackState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _animator.CrossFade(_animData.AttackParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (_detector.Target == null )
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if ( !_controller.IsArriveToTarget() )
            {
                _controller.ChangeState(_controller.CHASE_STATE);
            }

            // 한 번 공격한 뒤에 이전 상태 또는 idle 상태로 돌아가기
            // int ATK_CNT로 관리
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
        public HitState(BoarController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _animator.CrossFade(_animData.HitParamHash, 0.1f);
            // 멈추는 동작
        }

        public override void Execute()
        {

            // 이전 상태로 돌아가는 로직
            
        }
        public override void Exit()
        {
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
            
            // 이전 상태로 돌아가는 로직
            if (_controller.isRevive)
            {
                _controller.RevertToPrevState();
            }
        }
        public override void Exit() 
        {
            Debug.Log("Exit DIE");
            _agent.isStopped = false;
            _controller.IsDie = false;
            _controller.isRevive = false;
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

            // GLOBAL_STATE로 전환하는 로직
            if (_controller.IsDie)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            // 피격 상태도 여기에 넣을 수 있을 것 같다.
            // Attack 또는 Chase 중에 피격 받았을 경우, Hit 상태로 전환했다가 PrevState를 통해 다시 Attack 또는 Chase로 돌아갈 수 있기 때문이다.
        }
    }
    #endregion
}
