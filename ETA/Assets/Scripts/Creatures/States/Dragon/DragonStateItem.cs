using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : DragonState
    {
        public IdleState(DragonController controller) : base(controller)
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
                //Debug.Log("IDLE TO CHASE");
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
    public class ChaseState : DragonState
    {
        public ChaseState(DragonController controller) : base(controller)
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
                //Debug.Log("CHASE TO IDLE");
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_detector.Target != null && _detector.IsArriveToTarget())
            {
                //Debug.Log("CHASE TO IDLE_BATTLE");
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
    public class IdleBattleState : DragonState
    {
        public IdleBattleState(DragonController controller) : base(controller)
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
                //Debug.Log("IDLE_BATTLE TO IDLE");
                _controller.ChangeState(_controller.CHASE_STATE);
            }
            else if (IsStayForSeconds(2.0f))
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
    public class AttackState : DragonState
    {
        public AttackState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            // 3가지 모션
            if (_controller.AttackCnt % _controller.ChangeAttackCount == 2)
            {
                InitTime(_animData.TailAttackAnim.length);
                _animator.CrossFade(_animData.TailAttackParamHash, 0.1f);
            }
            else if (_controller.AttackCnt % _controller.ChangeAttackCount == 1)
            {
                InitTime(_animData.SwingAttackAnim.length);
                _animator.CrossFade(_animData.SwingAttackParamHash, 0.1f);
            }
            else if (_controller.AttackCnt % _controller.ChangeAttackCount == 0)
            {
                InitTime(_animData.DownAttackAnim.length);
                _animator.CrossFade(_animData.DownAttackParamHash, 0.1f);
            }

            _controller.AttackCnt++;
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
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

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : DragonState
    {
        public DieState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            //if (PhotonNetwork.IsMasterClient) _controller.ChangeToDieState();
            _agent.isStopped = true;
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
            //Managers.Sound.Play("Monster/KnightG/KnightGDie_SND", Define.Sound.Effect);
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
    public class GroggyState : DragonState
    {
        ParticleSystem ps;
        float groggyTime = 0;
        public GroggyState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            // Breath에 실패하면 Groggy 상태
            //if (_controller.PrevState is CounterEnableState)
            //{
            //    groggyTime = 3.0f;
            //    ps = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1, _controller.transform);
            //    ps.transform.SetParent(_controller.transform);
            //    ps.transform.localPosition = new Vector3(0, 1.0f, 0);

            //    ps = Managers.Effect.Play(Define.Effect.Groggy, groggyTime, _controller.transform);
            //    ps.transform.SetParent(_controller.transform);
            //    ps.transform.localPosition = new Vector3(0, 3.0f, 0);
            //}
            //else
            //{
            //    groggyTime = 0;
            //}


            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);
        }

        public override void Execute()
        {
            //if (PhotonNetwork.IsMasterClient == false) return;
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

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : DragonState
    {
        public GlobalState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;

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
