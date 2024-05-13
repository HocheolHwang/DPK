using MummyManStateItem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MummyBufferStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : MummyBufferState
    {
        public IdleState(MummyBufferController controller) : base(controller)
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
    public class IdleBattleState : MummyBufferState
    {
        public IdleBattleState(MummyBufferController controller) : base(controller)
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
            if (_controller.BuffTime >= _controller.ThreadHoldBuff)
            {
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
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
    public class ChaseState : MummyBufferState
    {
        // 몬스터끼리 뭉쳐지지 말고 경로에 몬스터가 있으면 피해서 이동하도록 수정
        public ChaseState(MummyBufferController controller) : base(controller)
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
    public class AttackState : MummyBufferState
    {
        public AttackState(MummyBufferController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToAttackState();
            InitTime(_animData.AttackAnim.length);
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);

            StartCast((int)EBufferPattern.RangedAutoAttack);
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

    // -------------------------------------- COUNTER ENABLE ------------------------------------------------
    #region COUNTER ENABLE
    public class CounterEnableState : MummyBufferState
    {
        public CounterEnableState(MummyBufferController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToCounterEnableState();
            _agent.velocity = Vector3.zero;

            InitTime(_animData.CounterEnableAnim.length);
            _animator.SetFloat("CounterEnableSpeed", 0.25f);
            _animator.CrossFade(_animData.CounterEnableParamHash, 0.1f);

            StartCast((int)EBufferPattern.CounterEnable);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;

            if (_controller.IsHitCounter)
            {
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
            else if (_animTime >= _threadHold * 4.0f)
            {
                _controller.ChangeState(_controller.BUFF_STATE);
            }
        }
        public override void Exit()
        {
            _controller.BuffTime = 0;
        }
    }
    #endregion

    // -------------------------------------- BUFF ------------------------------------------------
    #region BUFF
    public class BuffState : MummyBufferState
    {
        public BuffState(MummyBufferController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToBuffState();
            InitTime(_animData.BuffAnim.length);
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.BuffParamHash, 0.1f);

            StartCast((int)EBufferPattern.Buff);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
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

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------
    #region GROGGY
    public class GroggyState : MummyBufferState
    {
        ParticleSystem ps;
        float groggyTime = 0f;

        public GroggyState(MummyBufferController controller) : base(controller)
        {
        }

        public override void Enter()
        {

            if (PhotonNetwork.IsMasterClient) _controller.ChangeToGroggyState();

            if (_controller.PrevState is CounterEnableState)
            {
                groggyTime = 3.0f;
                ps = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1, _controller.transform);
                ps.transform.SetParent(_controller.transform);
                ps.transform.localPosition = new Vector3(0, 1.0f, 0);

                ps = Managers.Effect.Play(Define.Effect.Groggy, groggyTime, _controller.transform);
                ps.transform.SetParent(_controller.transform);
                ps.transform.position = new Vector3(0, 2.0f, 0);
            }
            else
            {
                groggyTime = 0;
            }

            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (IsStayForSeconds(groggyTime))
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
    public class DieState : MummyBufferState
    {
        public DieState(MummyBufferController controller) : base(controller)
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
    public class GlobalState : MummyBufferState
    {
        public GlobalState(MummyBufferController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _controller.BuffTime += Time.deltaTime;

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
