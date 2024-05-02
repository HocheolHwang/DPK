using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MummyManStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : MummyManState
    {
        public IdleState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (!_meetPlayer && _detector.Target != null) // 첫 조우 때 CLAP으로 시작
            {
                _controller.ChangeState(_controller.CLAP_STATE);
            }
            else if (_detector.Target != null)
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
    public class IdleBattleState : MummyManState
    {
        // CHASE -> IDLE BATTLE -> ATTACK
        public IdleBattleState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            LookAtEnemy();
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (IsStayForSeconds())
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
    public class ChaseState : MummyManState
    {
        public ChaseState(MummyManController controller) : base(controller)
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
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_detector.Target != null && _detector.IsArriveToTarget())
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
            else
            {
                _agent.SetDestination(_detector.Target.position);
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
    public class AttackState : MummyManState
    {
        private float _attackLen;
        private float _windMillLen;

        public AttackState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            // 근거리 몬스터가 죽고 근거리 디텍터를 활성화한 상태
            if ( !_isRangedAttack)
            {
                // attack( left, right - 둘 다 len이 같음 ) + wind_mill 연속 공격
                _attackLen = _animData.AttackAnim.length;
                _windMillLen = _animData.WindMillAnim.length;

                _threadHold = _attackLen * 2 + _windMillLen;
                InitTime(_threadHold);
                _animator.CrossFade(_animData.AttackParamHash, 0.2f);
            }
            // 원거리 디텍터를 착용한 상태( 초기 공격 수단 )
            else
            {
                InitTime(_animData.ThrowAnim.length);
                _animator.SetFloat("ThrowSpeed", 0.5f);
                _animator.CrossFade(_animData.ThrowParamHash, 0.2f);
            }
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            // 근거리 디텍터
            if ( !_isRangedAttack && _animTime >= _threadHold)
            {
                // 첫 공격 이후
                if ( _animTime >= (_threadHold - (_attackLen + _windMillLen) ) )
                {
                    // Target에게 데미지 주는 로직 필요( Action 또는 함수 )

                    ControlChangeState();
                }
                // 두 번째 공격 이후
                else if (_animTime >= (_threadHold - _windMillLen) )
                {
                    // Target에게 데미지 주는 로직 필요( Action 또는 함수 )

                    ControlChangeState();
                }
                // 마지막 공격 이후 상태 전환
                else
                {
                    ControlChangeState();
                }
                
            }
            // 원거리 디텍터
            else if (_isRangedAttack && _animTime >= _threadHold * 2.0f)
            {
                ControlChangeState();
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- CLAP( SUMMON_SKILL ) ------------------------------------------------
    #region CLAP
    public class ClapState : MummyManState
    {
        // IDLE -> CLAP
        // 첫 플레이어와 조우하면 몬스터를 소환한다.
        // 소환한 몬스터가 한 번씩 죽으면, 다시 한 번 소환한다.
        public ClapState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _meetPlayer = true;

            _agent.velocity = Vector3.zero;
            InitTime(_animData.ClapAnim.length);
            _animator.CrossFade(_animData.ClapParamHash, 0.1f);

            _summonSkill.Summon();
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_animTime >=  _threadHold)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- BACK_LOCATION ------------------------------------------------
    #region BACK_LOCATION
    public class BackLocationState : MummyManState
    {
        public BackLocationState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
        }

        public override void Execute()
        {
            
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : MummyManState
    {
        // MAN이 죽으면 warrior와 buffer도 죽는다.
        public DieState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
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

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------
    #region GROGGY
    public class GroggyState : MummyManState
    {
        public GroggyState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (IsStayForSeconds())
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
    public class GlobalState : MummyManState
    {
        public GlobalState(MummyManController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.CLAP_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            CheckGlobal();
        }
    }
    #endregion
}
