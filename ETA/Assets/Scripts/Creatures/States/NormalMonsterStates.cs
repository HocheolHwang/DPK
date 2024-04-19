using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NormalMonsterStates
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : State
    {
        private NormalMonsterController _controller;
        private Detector _detector;

        public IdleState(NormalMonsterController controller) : base(controller)
        {
            _controller = controller;
            _detector = controller.detector;
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            //_animator.CrossFade();
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
    public class ChaseState : State
    {
        private NormalMonsterController _controller;
        private Detector _detector;

        public ChaseState(NormalMonsterController controller) : base(controller)
        {
            _controller = controller;
            _detector = controller.detector;
        }

        public override void Enter()
        {
            Debug.Log("Enter CHASE");
            _agent.speed = 3.0f;
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
    public class AttackState : State
    {
        private NormalMonsterController _controller;
        private Detector _detector;

        public AttackState(NormalMonsterController controller) : base(controller)
        {
            _controller = controller;
            _detector = controller.detector;
        }

        public override void Enter()
        {
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
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- DIE ------------------------------------------------
    #region DIE
    public class DieState : State
    {
        private NormalMonsterController _controller;

        public DieState(NormalMonsterController controller) : base(controller)
        {
            _controller = controller;
        }

        public override void Enter() 
        {
            _agent.isStopped = true;
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
            _controller.isDie = false;
            _controller.isRevive = false;
        }
    }
    #endregion

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : State
    {
        private NormalMonsterController _controller;

        public GlobalState(NormalMonsterController controller) : base(controller)
        {
            _controller = controller;
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_controller.isDie)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            // 피격 상태도 여기에 넣을 수 있을 것 같다.
            // Attack 또는 Chase 중에 피격 받았을 경우, Hit 상태로 전환했다가 PrevState를 통해 다시 Attack 또는 Chase로 돌아갈 수 있기 때문이다.
        }
    }
    #endregion
}
