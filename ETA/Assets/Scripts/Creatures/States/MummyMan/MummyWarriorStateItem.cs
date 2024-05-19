using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MummyWarriorStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : MummyWarriorState
    {
        public IdleState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToIdleState();
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
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
    public class IdleBattleState : MummyWarriorState
    {
        public IdleBattleState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToIdleBattleState();
            _agent.velocity = Vector3.zero;
            LookAtEnemy();
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (_controller.WindMillTime >= _controller.ThreadHoldWindMill)
            {
                _controller.ChangeState(_controller.WIND_MILL_STATE);
            }

            if (IsStayForSeconds(2.0f))
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
    public class ChaseState : MummyWarriorState
    {
        // 몬스터끼리 뭉쳐지지 말고 경로에 몬스터가 있으면 피해서 이동하도록 수정
        public ChaseState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToChaseState();
            _agent.speed = _stat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (_detector.Target == null)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            if (_detector.IsArriveToTarget())
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
    public class AttackState : MummyWarriorState
    {
        public AttackState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToAttackState();
            InitTime(_animData.AttackAnim.length);
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);

            StartCast((int)EMummyWarriorPattern.MeleeAutoAttack);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime > _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
            {
                if (_detector.Target == null)
                {
                    _controller.ChangeState(_controller.IDLE_STATE);
                }

                if (_detector.IsArriveToTarget())
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

    // -------------------------------------- WIND_MILL ------------------------------------------------
    #region WIND_MILL
    public class WindMillState : MummyWarriorState
    {
        public WindMillState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToWindMillState();
            InitTime(_animData.WindMillAnim.length);
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.WindMillParamHash, 0.1f);

            StartCast((int)EMummyWarriorPattern.WindMill);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (IsStayForSeconds(_threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
            _controller.WindMillTime = 0;
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : MummyWarriorState
    {
        public DieState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToDieState();
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
    public class GlobalState : MummyWarriorState
    {
        public GlobalState(MummyWarriorController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _controller.WindMillTime += Time.deltaTime;

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= 0)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
        }
    }
    #endregion
}
