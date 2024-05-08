using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerStates
{
    public class PlayerState : State
    {
        protected PlayerController _playerController;
        protected PlayerState(PlayerController playerController) : base(playerController)
        {
            this._playerController = playerController;
        }

        public override void Enter()
        {
            base.Enter();
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
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
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
            if(_playerController.photonView.IsMine) _playerController.ChangeToMoveState();
            _animator.CrossFade("MOVE", 0.05f);
            _agent.isStopped = false;
            
        }

        public override void Execute()
        {
            base.Execute();
            if (_playerController.photonView.IsMine == false) return;
            Vector3 dest = _playerController._destination.position;
            float moveSpeed = _playerController.Stat.MoveSpeed;
            //_agent.Move(dir * Time.deltaTime * moveSpeed);

            

            if (_detector.Target != null)
            {
                _agent.speed = moveSpeed;
                _agent.SetDestination(_detector.Target.transform.position);

                if (_detector.IsArriveToTarget())
                {
                    if (_playerController.CurState is MoveState)
                    {
                        _playerController.ChangeState(_playerController.ATTACK_STATE);
                        return;

                        
                    }

                }
            }
            else
            {
                //_agent.isStopped = false;
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
        
        public AttackState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            if (_playerController.photonView.IsMine) _playerController.ChangeToAttackState();
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
            LookAtEnemy();
            _playerController.SkillSlot.NormalAttack();
            
      

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

    public class SkillState : PlayerState
    {
        
        public SkillState(PlayerController playerController) : base(playerController)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            if (_playerController.photonView.IsMine) _playerController.ChangeToSkillState();
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;

            _playerController.SkillSlot.CastSkill(_playerController._usingSkill);
            //_animator.CrossFade("SKILL1", 0.05f);
            //_detector.Target.GetComponent<BaseController>().TakeDamage(50);

        }

        public override void Execute()
        {
            //tmp += Time.deltaTime;
            //if (tmp > 1.5f)
            //{
            //    _playerController.ChangeState(_playerController.MOVE_STATE);
            //}
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class CollavoState : PlayerState
    {
        

        public CollavoState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            base.Enter();
            
            if (_playerController.photonView.IsMine) _playerController.ChangeToCollavoState();
            _playerController.SkillSlot.CastCollavoSkill(_playerController._usingSkill);


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

    public class DieState : PlayerState
    {

        public DieState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {

            _playerController.SkillSlot.CurrentSkill.StopCast();
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;

            if (_playerController.photonView.IsMine) _playerController.ChangeToDieState();
            _animator.CrossFade("DIE", 0.05f);
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }

    public class HoldState : PlayerState
    {
        float startTime;
        ParticleSystem _chargeEffect;
        public HoldState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Enter()
        {
            startTime = Time.time;
            if (_playerController.photonView.IsMine) _playerController.ChangeToHoldState();
            _playerController.SkillSlot.CurrentSkill?.StopCast();
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
            // 콜라보 스킬 정보를 시스템에 저장 해야함
            _animator.CrossFade("HOLD", 0.2f);
            //SwordChargeUp
            _chargeEffect = Managers.Resource.Instantiate("Effect/SwordChargeUp").GetComponent<ParticleSystem>();
            _chargeEffect.transform.position = _playerController.transform.position + _playerController.transform.up;
            _chargeEffect.Play();

            GameObject.FindObjectOfType<CollavoSystem>().AddCurrentSkill(_playerController, _playerController.SkillSlot.Skills[(int)_playerController._usingSkill].CollavoSkillName);
        }

        public override void Execute()
        {
            if (_playerController.photonView.IsMine == false) return;
            GameObject.Find("Collaboration_Slider").GetComponent<Slider>().value = (Time.time - startTime) / 3.0f;

            if (Time.time - startTime >= 3.0f)
            {
                GameObject.FindObjectOfType<CollavoSystem>().RemoveCurrentSkill(_playerController.SkillSlot.Skills[(int)_playerController._usingSkill].CollavoSkillName);
                _playerController.ChangeState(_playerController.SKILL_STATE);
                return;
            }

            if (Input.anyKey)
            {
                Debug.Log("홀딩 중입니다.");
            }
            else // 키를 떄면?
            {
                //_playerController.ChangeState(_playerController.COLLAVO_STATE);
                GameObject.FindObjectOfType<CollavoSystem>().RemoveCurrentSkill(_playerController.SkillSlot.Skills[(int)_playerController._usingSkill].CollavoSkillName);
                _playerController.ChangeState(_playerController.SKILL_STATE);
                return;

            }
        }

        public override void Exit()
        {
            Managers.Resource.Destroy(_chargeEffect.gameObject);
            GameObject.Find("Collaboration_Slider").GetComponent<Slider>().value = 0;
        }
    }

    public class GlobalState : PlayerState
    {
        public GlobalState(PlayerController playerController) : base(playerController)
        {

        }

        public override void Execute()
        {
            return;
            if (_playerController.photonView.IsMine == false) return;
            if (_playerController.CurState is DieState) return;

            if (_playerController.isFinished) _playerController.ChangeState(_playerController.IDLE_STATE); ;

            if(_playerController.Stat.Hp <= 0)
            {
                _playerController.ChangeState(_playerController.DIE_STATE);
            }
        }


    }

}
