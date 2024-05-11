using MummyManStateItem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IprisStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : IprisState
    {
        public IdleState(IprisController controller) : base(controller)
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
                Debug.Log("IDLE TO CHASE");
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
    public class ChaseState : IprisState
    {
        // IDLE_BATTLE animation clip을 이용해서 CHASE 상태를 유지한다.

        public ChaseState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.speed = _stat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (_detector.Target == null)
            {
                Debug.Log("CHASE TO IDLE");
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_detector.Target != null && _detector.IsArriveToTarget())
            {
                Debug.Log("CHASE TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
            else
            {
                //Debug.Log("CHASE");
                _agent.SetDestination(_detector.Target.position);
            }
        }

        public override void Exit()
        {
            _agent.ResetPath();
        }
    }
    #endregion

    // -------------------------------------- IDLE BATTLE ------------------------------------------------
    #region IDLE_BATTLE
    public class IdleBattleState : IprisState
    {
        public IdleBattleState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            LookAtEnemy();
            _animator.CrossFade(_animData.IdleBattleParamHash, 0.25f);
        }

        public override void Execute()
        {
            if (!_detector.IsArriveToTarget())
            {
                Debug.Log("IDLE_BATTLE TO IDLE");
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_controller.CounterTime >= _controller.ThreadHoldCounter)
            {
                Debug.Log("IDLE_BATTLE TO COUNTER_ENABLE");
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
            }
            else if (IsStayForSeconds(1.0f))
            {
                //Debug.Log("IDLE_BATTLE TO ATTACK");
                _controller.ChangeState(_controller.ATTACK_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- ATTACK ------------------------------------------------
    #region ATTACK
    public class AttackState : IprisState
    {
        public AttackState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            InitTime(_animData.AttackFirstAnim.length + _animData.AttackSecondAnim.length);
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                //Debug.Log("ATTACK TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- COUNTER ENABLE ------------------------------------------------
    #region COUNTER_ENABLE
    public class CounterEnable : IprisState
    {
        public CounterEnable(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.CounterEnableAnim.length);
            _animator.SetFloat("CounterEnableSpeed", 0.5f);
            _animator.CrossFade(_animData.CounterEnableParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_controller.IsHitCounter)
            {
                Debug.Log("COUNTER_ENABLE TO GROGGY");
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
            else if (_animTime >= _threadHold * 2.0f)
            {
                Debug.Log("COUNTER_ENABLE TO COUNTER_ATTACK");
                _controller.ChangeState(_controller.COUNTER_ATTACK_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- COUNTER ATTACK ------------------------------------------------
    #region COUNTER_ATTACK
    public class CounterAttack : IprisState
    {
        public CounterAttack(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.CounterAttackAnim.length);
            _animator.SetFloat("CounterAttackSpeed", 0.5f);
            _animator.CrossFade(_animData.CounterAttackParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)
            {
                Debug.Log("COUNTER_ATTACK TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _controller.CounterTime = 0;
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------

    // DIE => TO_DRAGON
    #region DIE
    public class DieState : IprisState
    {
        public DieState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.DieAnim.length);
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.TO_DRAGON_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------

    // if문 조건 수정 필요
    #region GROGGY
    public class GroggyState : IprisState
    {
        ParticleSystem ps;
        float groggyTime = 0f;

        public GroggyState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {


            if (_controller.PrevState is CounterEnable)      // Counter Enable
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
            if (IsStayForSeconds(groggyTime))
            {
                _controller.RevertToPrevState();
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : IprisState
    {
        public GlobalState(IprisController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            if (_controller.MeetPlayer)
            {
                _controller.CounterTime += Time.deltaTime;
            }

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
