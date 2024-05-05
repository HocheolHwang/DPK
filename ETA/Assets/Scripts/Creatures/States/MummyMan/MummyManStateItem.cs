using KnightGStateItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 동작의 끝에서는 IDLE 상태로 전환
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
        // CHASE -> IDLE BATTLE -> SHOUTING
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
            if (IsPreviousState())
            {
                _controller.ChangeState(_controller.BACK_LOCATION_STATE);
            }
            else if (_jumpTime >= _threadHoldJump)
            {
                _controller.ChangeState(_controller.JUMP_STATE);
            }
            else if (_shoutingTime >= _threadHoldShouting)
            {
                _controller.ChangeState(_controller.SHOUTING_STATE);
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

    // -------------------------------------- SHOUTING ------------------------------------------------
    #region SHOUTING
    public class ShoutingState : MummyManState
    {
        // 3초 동안 기절을 부여한다. -> hit box에 추가
        public ShoutingState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            InitTime(_animData.ShoutingAnim.length);

            _animator.SetFloat("ShoutingSpeed", 0.5f);
            _animator.CrossFade(_animData.ShoutingParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_animTime >= (_threadHold * 2))
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
            _shoutingTime = 0;
        }
    }
    #endregion

    // -------------------------------------- JUMP ------------------------------------------------
    #region JUMP
    public class JumpState : MummyManState
    {
        // 3초 동안 기절을 부여한다. -> hit box에 추가
        public JumpState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;               // BACK_LOCATION에서 true

            SetStartAndDestPos(_controller.transform.position, MonsterManager.Instance.GetCenterPos(_controller.transform));

            InitTime(_animData.JumpAnim.length);
            _animator.SetFloat("JumpSpeed", 0.5f);
            _animator.CrossFade(_animData.JumpParamHash, 0.1f);

        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            JumpToTarget(_animTime);
            if (IsStayForSeconds((_threadHold * 2.0f) + 0.5f))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
            _jumpTime = 0;
        }
    }
    #endregion

    // -------------------------------------- RUSH ------------------------------------------------
    #region RUSH
    public class RushState : MummyManState
    {
        public RushState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            SetStartAndDestPos(_controller.transform.position, _destPos);

            // pattern 수행

            InitTime(_animData.RushAnim.length);
            _animator.SetFloat("RushSpeed", 0.5f);
            _animator.CrossFade(_animData.RushParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_animTime >= (_threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
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
            _agent.enabled = false;

            SetStartAndDestPos(_controller.transform.position, _startPos);

            InitTime(_animData.JumpAnim.length);
            _animator.SetFloat("JumpSpeed", 0.5f);
            _animator.CrossFade(_animData.JumpParamHash, 0.1f);

        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            JumpToTarget(_animTime);

            if (_animTime >= (_threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }
        public override void Exit()
        {
            _agent.enabled = true;
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
        ParticleSystem ps;
        float groggyTime = 3.0f;

        public GroggyState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            ps = Managers.Effect.Play(Define.Effect.Groggy, _controller.transform);
            ps.transform.SetParent(_controller.transform);
            ps.transform.position = new Vector3(0, 3.0f, 0);

            //if (_controller.PrevState is CounterEnableState)
            //{
            //    groggyTime = 3.0f;
            //}
            //else
            //{
            //    groggyTime = 1.0f;
            //}

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
            Managers.Effect.Stop(ps);
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
            if (_meetPlayer)
            {
                _shoutingTime += Time.deltaTime;
                _jumpTime += Time.deltaTime;
            }

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.CLAP_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            CheckGlobal();
        }
    }
    #endregion
}
