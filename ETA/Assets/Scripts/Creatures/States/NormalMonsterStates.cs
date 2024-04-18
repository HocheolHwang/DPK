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

        public IdleState(NormalMonsterController controller)
        {
            // Common
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;

            // Private
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

        public ChaseState(NormalMonsterController controller)
        {
            // Common
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;

            // Private
            _detector = controller.detector;
        }

        public override void Enter()
        {
            _agent.speed = 3.0f;
        }

        public override void Execute()
        {
            if ( _detector.Target == null )
            {
                _controller.ChangeState(_controller.IDLE_STATE);
                _agent.ResetPath();
            }
            else if (_controller.IsArriveToTarget())
            {
                _controller.ChangeState(_controller.ATTACK_STATE);
            }

            _agent.SetDestination(_detector.Target.position);
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- ATTACK ------------------------------------------------
    #region ATTACK
    public class AttackState : State
    {
        private NormalMonsterController _controller;
        private Detector _detector;

        public AttackState(NormalMonsterController controller)
        {
            // Common
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;

            // Private
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

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : State
    {
        private NormalMonsterController _controller;
        public GlobalState(NormalMonsterController controller)
        {
            // Common
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;
        }

        public override void Enter() { }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태인 경우 종료

            // GLOBAL_STATE로 전환하는 로직
        }
        public override void Exit() { }
    }
    #endregion
}
