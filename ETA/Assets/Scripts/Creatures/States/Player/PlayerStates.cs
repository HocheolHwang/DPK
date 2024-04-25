using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerStates
{
    public class PlayerState : State
    {
        protected PlayerController _playerController;
        protected PlayerState(PlayerController playerController) : base(playerController)
        {
            this._playerController = playerController;
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
            float moveSpeed = _playerController.stat.MoveSpeed;
            //_agent.Move(dir * Time.deltaTime * moveSpeed);

            //float dist = Vector3.Distance(_playerController.transform.position, dest);

            if(_detector.Target != null)
            {
                float dist = Vector3.Distance(_detector.Target.transform.position, _playerController.transform.position);
                if (dist <= _detector.AttackRange)
                {
                    _agent.velocity = Vector3.zero;
                    _agent.isStopped = true;
                    _playerController.ChangeState(_playerController.ATTACK_STATE);

                }
            }
            else
            {
                _agent.isStopped = false;
                _agent.speed = moveSpeed + Vector3.Distance(_playerController.transform.position, dest);
                _agent.SetDestination(dest);

            }


        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class AttackState : PlayerState
    {
        float tmp = 0;
        
        public AttackState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            tmp = 0;
            _animator.CrossFade("NORMAL_ATTACK", 0.05f);
            _detector.Target.GetComponent<MonsterController>().TakeDamage(_playerController.stat.AttackDamage);
      

        }

        public override void Execute()
        {
            tmp += Time.deltaTime;
            if( tmp > 1.5f)
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

    public class SkillState : PlayerState
    {
        float tmp = 0;
        
        public SkillState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            tmp = 0;

            _playerController.SkillSlot.CastSkill(_playerController._usingSkill);
            _animator.CrossFade("SKILL1", 0.05f);
            _detector.Target.GetComponent<MonsterController>().TakeDamage(50);

        }

        public override void Execute()
        {
            tmp += Time.deltaTime;
            if (tmp > 1.5f)
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

    public class CollavoState : PlayerState
    {
        float tmp = 0;

        public CollavoState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            tmp = 0;
            _animator.CrossFade("SKILL2", 0.05f);
            Collider[] enemies = Physics.OverlapSphere(_playerController.transform.position, 6.0f, LayerMask.GetMask("Monster"));

            foreach (Collider enemy in enemies)
            {
                enemy.GetComponent<MonsterController>().TakeDamage(50);
            }


        }

        public override void Execute()
        {
            tmp += Time.deltaTime;
            if (tmp > 1.5f)
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

    public class DieState : PlayerState
    {

        public DieState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {


            _animator.CrossFade("DIE", 0.05f);
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }

    public class GlobalState : PlayerState
    {
        public GlobalState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Execute()
        {
            if (_playerController.CurState is DieState) return;

            if(_playerController.stat.Hp <= 0)
            {
                _playerController.ChangeState(_playerController.DIE_STATE);
            }
        }


    }

}
