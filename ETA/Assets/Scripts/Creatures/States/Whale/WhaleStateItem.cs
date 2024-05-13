using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WhaleStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : WhaleState
    {
        public IdleState(WhaleController controller) : base(controller)
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
    public class IdleBattleState : WhaleState
    {
        public IdleBattleState(WhaleController controller) : base(controller)
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
            // Counter
            if (counterTime >= threadHoldCounter)
            {
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
            } 
            else if (IsStayForSeconds())
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
    public class ChaseState : WhaleState
    {
        public ChaseState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToChaseState();
            counterTimeTrigger = 0;

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
            else
            {
                _agent.SetDestination(_detector.Target.position);
            }
            
            if (_detector.IsArriveToTarget())
            {
                    _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
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
    public class AttackState : WhaleState
    {
        public AttackState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToAttackState();
            counterTimeTrigger = 0;
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절

            InitTime(_animData.AttackAnim.length);
            _animator.CrossFade(_animData.AttackParamHash, 0.4f);

            StartCast((int)EWhalePattern.FirstAuto);
            
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
            {
                if (_detector.Target == null)
                {
                    _controller.ChangeState(_controller.IDLE_STATE);
                }
                else if (_detector.IsArriveToTarget())
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

    // -------------------------------------- COUNTER_ENABLE ------------------------------------------------
    #region COUNTER_ENABLE
    public class CounterEnableState : WhaleState
    {
        public CounterEnableState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToCounterEnableState();
            InitTime(_animData.CounterEnableAnim.length);

            _animator.SetFloat("CounterEnableSpeed", 0.25f);
            _animator.CrossFade(_animData.CounterEnableParamHash, 0.1f);

            StartCast((int)EWhalePattern.CounterEnable);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;

            // 카운터에 맞으면 그로기
            if (_controller.IsHitCounter)
            {
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
            else if (_animTime >= _threadHold * 4.0f)
            {
                _controller.ChangeState(_controller.COUNTER_ATTACK_STATE);
            }
        }
        public override void Exit()
        {
            counterTime = 0;
        }
    }
    #endregion

    // -------------------------------------- COUNTER_ATTACK ------------------------------------------------
    #region COUNTER_ATTACK
    public class CounterAttackState : WhaleState
    {
        public CounterAttackState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToCounterAttackState();
            InitTime(_animData.CounterAttackAnim.length);
            _animator.CrossFade(_animData.CounterAttackParamHash, 0.1f);

            StartCast((int)EWhalePattern.CounterAttack);
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

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : WhaleState
    {
        public DieState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToDieState();
            _agent.isStopped = true;
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
            Managers.Sound.Play("Monster/Whale/WhaleDie_SND", Define.Sound.Effect);
            //_controller.GetComponentInChildren<ParticleSystem>().Stop();
        }

        public override void Execute()
        {
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------
    #region GROGGY
    public class GroggyState : WhaleState
    {
        ParticleSystem ps;
        float groggyTime = 0;
        public GroggyState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToGroggyState();
            ps = Managers.Effect.Play(Define.Effect.Groggy, _controller.transform);
            ps.transform.SetParent(_controller.transform);
            ps.transform.position = new Vector3(0, 3.0f, 0);

            // 보스는 카운터 이외의 공격에는 그로기 상태에 빠지지 않는다.
            if (_controller.PrevState is CounterEnableState)
            {
                groggyTime = 3.0f;
                ps = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1, _controller.transform);
                ps.transform.SetParent(_controller.transform);
                ps.transform.localPosition = new Vector3(0, 1.0f, 0);

                ps = Managers.Effect.Play(Define.Effect.Groggy, groggyTime, _controller.transform);
                ps.transform.SetParent(_controller.transform);
                ps.transform.localPosition = new Vector3(0, 2.0f, 0);
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
            Managers.Effect.Stop(ps);
        }
    }
    #endregion

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : WhaleState
    {
        public GlobalState(WhaleController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return; 
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.

            if (counterTimeTrigger <= 0)
            {
                counterTime += Time.deltaTime;
            }

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.GROGGY_STATE) return;               // 그로기 상태는 특정 상태에서 분기

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= 0)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion
}
