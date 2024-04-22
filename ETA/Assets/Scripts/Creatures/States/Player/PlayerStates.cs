using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerStates
{
    public class PlayerState : State
    {
        protected PlayerController _playerController;
        protected Detector _detector;
        protected PlayerState(PlayerController playerController) : base(playerController)
        {
            this._playerController = playerController;
            this._detector = playerController.detector;
        }
    }

    public class IdleState : PlayerState
    {
        public IdleState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _animator.CrossFade("IDLE", 0, -1, 0);
            
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class MoveState : PlayerState
    {
        public MoveState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _animator.CrossFade("MOVE", 0.05f);
        }

        public override void Execute()
        {
            base.Execute();

            Vector3 dest = _playerController._destination.position;
            float moveSpeed = 3.0f;
            //_agent.Move(dir * Time.deltaTime * moveSpeed);

            //float dist = Vector3.Distance(_playerController.transform.position, dest);

            if(_detector.Target != null)
            {
                float dist = Vector3.Distance(_detector.Target.transform.position, _playerController.transform.position);
                if (dist <= _detector.attackRange)
                {
                    _agent.velocity = Vector3.zero;
                    _agent.isStopped = true;
                    _playerController.ChangeState(_playerController.SKILL_STATE);

                }
            }
            else
            {
                _agent.speed = moveSpeed + Vector3.Distance(_playerController.transform.position, dest);
                _agent.SetDestination(dest);

            }


        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class SkillState : PlayerState
    {
        int tmp = 0;
        public SkillState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            tmp = 0;
            _animator.CrossFade("NORMAL_ATTACK", 0.05f);

        }

        public override void Execute()
        {
            tmp += 1;
            if( tmp > 180)
            {
                _playerController.ChangeState(_playerController.MOVE_STATE);
            }
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
