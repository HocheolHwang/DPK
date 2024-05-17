using KnightGStateItem;
using Photon.Pun;
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
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToIdleState();
            SetDetector();
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (!_controller.MeetPlayer && _controller.Target != null) // 첫 조우 때 CLAP으로 시작
            {
                _controller.ChangeState(_controller.CLAP_STATE);
            }
            else if (_controller.Target != null)
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
        public IdleBattleState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToIdleBattleState();
            _agent.velocity = Vector3.zero;

            SetDetector();
            LookAtEnemy();

            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (IsPreviousState())
            {
                _controller.ChangeState(_controller.BACK_LOCATION_STATE);
            }
            else if (IsSommoningMonster())
            {
                SommonMonsterEvent();
            }
            else if (IsDeadBuffer())        // 2번 사용하도록 수정
            {
                _controller.ChangeState(_controller.FORE_SHADOWING_STATE);
            }
            else if (_controller.JumpTime >= _controller.ThreadHoldJump)
            {
                _controller.ChangeState(_controller.JUMP_STATE);
            }
            else if (_controller.ShoutingTime >= _controller.ThreadHoldShouting)
            {
                _controller.ChangeState(_controller.SHOUTING_STATE);
            }
            else if ( !_detector.IsArriveToTarget(_controller.Target, _controller.AttackRange) )
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (IsStayForSeconds(2.0f))
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
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToChaseState();
            _agent.speed = _stat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (_controller.Target == null)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_controller.Target != null && _detector.IsArriveToTarget(_controller.Target, _controller.AttackRange))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
            else
            {
                _agent.SetDestination(_controller.Target.position);
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

        // 현재 detector를 고정시킨 뒤에 로직을 구현

        public AttackState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToAttackState();
            _agent.velocity = Vector3.zero;
            // 근거리 몬스터가 죽고 근거리 디텍터를 활성화한 상태
            if ( !_controller.IsRangedAttack)
            {
                // attack( left, right - 둘 다 len이 같음 ) + wind_mill 연속 공격
                _attackLen = _animData.AttackAnim.length;
                _windMillLen = _animData.WindMillAnim.length;

                _threadHold = _attackLen * 2 + _windMillLen;
                InitTime(_threadHold);
                _animator.CrossFade(_animData.AttackParamHash, 0.2f);

                StartCast((int)EMummyManPattern.MeleeAutoAttack);
            }
            // 원거리 디텍터를 착용한 상태( 초기 공격 수단 )
            else
            {
                InitTime(_animData.ThrowAnim.length);
                _animator.SetFloat("ThrowSpeed", 0.5f);
                _animator.CrossFade(_animData.ThrowParamHash, 0.2f);

                StartCast((int)EMummyManPattern.RangedAutoAttack);
            }
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;

            // 근거리 디텍터
            if ( !_controller.IsRangedAttack && _animTime >= _threadHold)
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
            else if (_controller.IsRangedAttack && _animTime >= _threadHold * 2.0f)
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
        public ClapState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToClapState();
            _controller.MeetPlayer = true;

            _agent.velocity = Vector3.zero;
            InitTime(_animData.ClapAnim.length);
            _animator.CrossFade(_animData.ClapParamHash, 0.1f);

            _summonSkill.Summon();
            Managers.Sound.Play("Sounds/Monster/Mummy/MummyClap_SND", Define.Sound.Effect);
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

    // -------------------------------------- SHOUTING ------------------------------------------------
    #region SHOUTING
    public class ShoutingState : MummyManState
    {
        public ShoutingState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToShoutingState();
            _agent.velocity = Vector3.zero;
            InitTime(_animData.ShoutingAnim.length);

            _animator.SetFloat("ShoutingSpeed", 0.5f);
            _animator.CrossFade(_animData.ShoutingParamHash, 0.1f);

            StartCast((int)EMummyManPattern.Shouting);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;

            if (_animTime >= (_threadHold * 2) && (_controller.PrevState is ForeShadowingState))
            {
                _controller.ChangeState(_controller.RUSH_STATE);
            }
            else if (_animTime >= (_threadHold * 2))
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
            _controller.ShoutingTime = 0;
        }
    }
    #endregion

    // -------------------------------------- JUMP ------------------------------------------------
    #region JUMP
    public class JumpState : MummyManState
    {
        public JumpState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToJumpState();
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;               // BACK_LOCATION에서 true

            SetStartAndDestPos(_controller.transform.position, MonsterManager.Instance.GetCenterPos(_controller.transform));

            InitTime(_animData.JumpAnim.length);
            _animator.SetFloat("JumpSpeed", 0.5f);
            _animator.CrossFade(_animData.JumpParamHash, 0.1f);

            StartCast((int)EMummyManPattern.Jump);
        }

        public override void Execute()
        {

            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            JumpToTarget(_animTime);
            if (_controller.IsRangedAttack && IsStayForSeconds((_threadHold * 2.0f) + 0.5f))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
            else if (!_controller.IsRangedAttack && IsStayForSeconds((_threadHold * 2.0f)))
            {
                _controller.ChangeState(_controller.WIND_MILL_STATE);
            }
        }

        public override void Exit()
        {
            _controller.JumpTime = 0;
        }
    }
    #endregion

    // -------------------------------------- RUSH ------------------------------------------------
    #region RUSH
    public class RushState : MummyManState
    {
        Vector3 destination;
        float tempStopDist;

        public RushState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToRushState();
            tempStopDist = _agent.stoppingDistance;
            _agent.stoppingDistance = 0;

            destination = MonsterManager.Instance.GetBackPosPlayer(_controller.transform);
            SetStartAndDestPos(_controller.transform.position, destination);
            _agent.speed = CalcSpeedFromDestTime(destination);

            InitTime(_animData.RushAnim.length);
            _animator.SetFloat("RushSpeed", 2.0f);
            _animator.CrossFade(_animData.RushParamHash, 0.1f);

            StartCast((int)EMummyManPattern.Rush);

            // _controller.DestPos => destination
            _agent.SetDestination(destination);
            
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (IsStayForSeconds(RushTime))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _agent.ResetPath();
            _agent.stoppingDistance = tempStopDist;
            _agent.speed = _controller.Stat.MoveSpeed;
        }
    }
    #endregion

    // -------------------------------------- WIND_MILL ------------------------------------------------
    #region WIND_MILL
    public class WindMillState : MummyManState
    {
        public WindMillState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToWindMillState();
            _agent.velocity = Vector3.zero;

            InitTime(_animData.WindMillAnim.length);
            _animator.CrossFade(_animData.WindMillParamHash, 0.1f);

            StartCast((int)EMummyManPattern.WindMill);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (IsStayForSeconds(_threadHold))
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- FORE_SHADOWING : COUNTER ENABLE ------------------------------------------------
    #region FORE_SHADOWING
    // 자신한테만 버프 수치( HP 10% 회복, ATK += 10, DEF += 5, TIME: 30초, Shield: 30초 HP 10% 부여 )
    // COUNTER ENABLE
    public class ForeShadowingState : MummyManState
    {
        public ForeShadowingState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToForeShadowingState();
            _agent.velocity = Vector3.zero;

            InitTime(_animData.ForeShadowingAnim.length);
            _animator.SetFloat("ForeShadowingSpeed", 0.5f);
            _animator.CrossFade(_animData.ForeShadowingParamHash, 0.1f);

            StartCast((int)EMummyManPattern.CounterEnable);
        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;

            if (_controller.IsHitCounter)
            {
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
            else if (_animTime >= _threadHold * 2.0f)
            {
                _controller.ChangeState(_controller.SHOUTING_STATE);
            }
        }
        public override void Exit()
        {
            // 여기서 Buff 부여
            StartCast((int)EMummyManPattern.Buff);
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
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToBackLocationState();
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;

            SetStartAndDestPos(_controller.transform.position, _controller.StartPos);

            InitTime(_animData.JumpAnim.length);
            _animator.SetFloat("JumpSpeed", 0.5f);
            _animator.CrossFade(_animData.JumpParamHash, 0.1f);

        }

        public override void Execute()
        {
            if (PhotonNetwork.IsMasterClient == false) return;
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
            if (PhotonNetwork.IsMasterClient) _controller.ChangeToDieState();
            _agent.isStopped = true;
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
            Managers.Sound.Play("Monster/Mummy/MummyDie_SND", Define.Sound.Effect);

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
        float groggyTime = 0f;

        public GroggyState(MummyManController controller) : base(controller)
        {
        }

        public override void Enter()
        {

            if (PhotonNetwork.IsMasterClient) _controller.ChangeToGroggyState();

            if (_controller.PrevState is ForeShadowingState)
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
            if (PhotonNetwork.IsMasterClient == false) return;
            if (_controller.MeetPlayer)
            {
                _controller.ShoutingTime += Time.deltaTime;
                _controller.JumpTime += Time.deltaTime;
            }

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            UpdateTarget();
            CheckGlobal();
        }
    }
    #endregion
}
