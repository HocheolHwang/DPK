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
            _machine = controller.Machine;

            // Private
            _detector = controller.detector;
        }

        public override void Enter()
        {
            Debug.Log($"controller name: {_controller.gameObject.name}");
            Debug.Log($"agent null? {_agent.gameObject.name}");
            Debug.Log($"animator null ? {_animator.gameObject.name}");
            Debug.Log($"machine null ? {_controller.gameObject.name}");
            Debug.Log($"curState null ? {_controller.CurState}");

            _agent.velocity = Vector3.zero;

            //_animator.CrossFade();
        }

        public override void Execute()
        {
            // IsComplete가 필요할까?
            if (_detector.Target != null)
            {
                _machine.ChangeState(_controller.CHASE_STATE);
            }
        }

        public override void Exit()
        {
        }

        public bool IsArriveToTarget()
        {
            return _agent.remainingDistance <= _agent.stoppingDistance;
        }
    }
    #endregion

    // -------------------------------------- CHASE ------------------------------------------------
    #region CHASE
    public class ChaseState : State
    {
        [Header("해당 상태에서 사용할 속성")]
        [SerializeField] public AnimationClip anim;
        [SerializeField] public float maxSpeed;
        [SerializeField] public float speed;        // animator  속도와 동일하게 세팅

        private NormalMonsterController _controller;
        public ChaseState(NormalMonsterController controller)
        {
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;
        }

        public override void Enter()
        {
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
    public class GlobalState : State
    {
        private NormalMonsterController _controller;
        public GlobalState(NormalMonsterController controller)
        {
            _controller = controller;
            _agent = controller.agent;
            _animator = controller.animator;
        }

        public override void Enter()
        {
        }

        public override void Execute()
        {
            // GLOBAL_STATE 상태인 경우 종료

            // GLOBAL_STATE로 전환하는 로직
        }

        public override void Exit()
        {
        }
    }
    #endregion
}
