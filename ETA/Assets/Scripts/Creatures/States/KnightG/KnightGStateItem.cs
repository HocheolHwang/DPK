using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KnightGStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : KnightGState
    {
        public IdleState(KnightGController controller) : base(controller)
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
    public class IdleBattleState : KnightGState
    {
        public IdleBattleState(KnightGController controller) : base(controller)
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
            // Counter
            if (counterTime >= threadHoldCounter)
            {
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
            } 
            else if (twoSkillTrigger == 1 && _stat.Hp <= (_stat.MaxHp * 0.6))
            {
                _controller.ChangeState(_controller.TWO_SKILL_TRANSITION_STATE);
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
    public class ChaseState : KnightGState
    {
        public ChaseState(KnightGController controller) : base(controller)
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
            else
            {
                _agent.SetDestination(_detector.Target.position);
            }
            
            if (_detector.IsArriveToTarget())
            {
                if (_controller.IsEnterPhaseTwo)
                {
                    _controller.ChangeState(_controller.PHASE_ATTACK_ING_STATE);
                }
                else
                {
                    _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
                }
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
    public class AttackState : KnightGState
    {
        public AttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            counterTimeTrigger = 0;
            attackCnt++;
            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절

            // 2가지 자동 공격 모션이 존재한다.
            if (attackCnt % 2 == 1)
            {
                InitTime(_animData.AttackAnim.length);
                _animator.CrossFade(_animData.AttackParamHash, 0.4f);

                StartCast((int)EKnightGPattern.FirstAuto);
            }
            else if (attackCnt % 2 == 0)
            {
                InitTime(_animData.AttackUpAnim.length);
                _animator.CrossFade(_animData.AttackUpParamHash, 0.4f);

                StartCast((int)EKnightGPattern.SecondAuto);
            }
            
        }

        public override void Execute()
        {
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

    // -------------------------------------- TWO_SKILL_TRANSITION ------------------------------------------------
    #region TWO_SKILL_TRANSITION
    public class TwoSkillTransitionState : KnightGState
    {
        public TwoSkillTransitionState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.TwoSkillTransitionAnim.length);
            _animator.CrossFade(_animData.TwoSkillTransitionParamHash, 0.2f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.TWO_SKILL_ENERGY_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- TWO_SKILL_ENERGY ------------------------------------------------
    #region TWO_SKILL_ENERGY
    public class TwoSkillEnergyState : KnightGState
    {
        public TwoSkillEnergyState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _animator.SetFloat("SkillEnergySpeed", 0.5f);
            InitTime(_animData.TwoSkillEnergyAnim.length);
            _animator.CrossFade(_animData.TwoSkillEnergyParamHash, 0.2f);

            StartCast((int)EKnightGPattern.TwoSkillEnergy);
        }

        public override void Execute()
        {
            if (IsStayForSeconds(2.0f))
            {
                _controller.ChangeState(_controller.TWO_SKILL_ATTACK_STATE);
            }

            // 특정 스킬에 맞으면 끊기거나 데미지가 줄어든다.
            // 데미지가 줄어드는 경우, 변수를 이용해서 _controller의 Stat에 접근하는 함수가 필요하다.
        }

        public override void Exit()
        {
            twoSkillTrigger = 0;
        }
    }
    #endregion

    // -------------------------------------- TWO_SKILL_ATTACK ------------------------------------------------
    #region TWO_SKILL_ATTACK
    public class TwoSkillAttackState : KnightGState
    {
        public TwoSkillAttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _animator.SetFloat("SkillAttackSpeed", 0.5f);
            InitTime(_animData.TwoSkillAttackAnim.length);
            _animator.CrossFade(_animData.TwoSkillAttackParamHash, 0.2f);

            StartCast((int)EKnightGPattern.TwoSkillAttack);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- COUNTER_ENABLE ------------------------------------------------
    #region COUNTER_ENABLE
    public class CounterEnableState : KnightGState
    {
        public CounterEnableState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.CounterEnableAnim.length);

            _animator.SetFloat("CounterEnableSpeed", 0.25f);
            _animator.CrossFade(_animData.CounterEnableParamHash, 0.1f);

            StartCast((int)EKnightGPattern.CounterEnable);
        }

        public override void Execute()
        {
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
    public class CounterAttackState : KnightGState
    {
        public CounterAttackState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.CounterAttackAnim.length);
            _animator.CrossFade(_animData.CounterAttackParamHash, 0.1f);

            StartCast((int)EKnightGPattern.CounterAttack);
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

    // -------------------------------------- PHASE_TRANSITION ------------------------------------------------
    #region PHASE_TRANSITION
    public class PhaseTransitionState : KnightGState
    {
        public PhaseTransitionState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            InitTime(_animData.PhaseTransitionAnim.length);
            _animator.SetFloat("PhaseTransitionSpeed", 0.5f);
            _animator.CrossFade(_animData.PhaseTransitionParamHash, 0.1f);

            StartCast((int)EKnightGPattern.PhaseTransition);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if ( _animTime >= _threadHold * 2.0f)
            {
                _controller.IsEnterPhaseTwo = true;
                // 타겟팅한 한 명의 적만 계속 공격하는 패턴
                if (_detector.Target == null || !_detector.IsArriveToTarget())
                {
                    _controller.ChangeState(_controller.IDLE_STATE);
                }
                else
                {
                    _controller.ChangeState(_controller.PHASE_ATTACK_ING_STATE);
                }
            }

            // Phase 진입을 끊는 스킬에 맞으면 그로기 상태로 전환
            // 2페이즈 진입 실패로 인해 1페이즈 스킬로만 공격할 수 있음
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- PHASE_ATTACK_ING ------------------------------------------------
    #region PHASE_ATTACK_ING
    public class PhaseAttackingState : KnightGState
    {
        public PhaseAttackingState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            LookAtEnemy();                                  // 동기화 편의성 + 공격하기 직전에만 목표물을 보고 싶기 때문
            InitTime(_animData.PhaseAttackingAnim.length);
            _animator.CrossFade(_animData.PhaseAttackingParamHash, 0.1f);

            StartCast((int)EKnightGPattern.PhaseAttack);
        }

        public override void Execute()
        {   
            _animTime += Time.deltaTime;
            // 타겟팅한 한 명의 적만 계속 공격하는 패턴
            if (_detector.Target == null || !_detector.IsArriveToTarget())
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            else if (_animTime >= _threadHold)  // Loop 활성화 필수
            {
                _controller.ChangeState(_controller.PHASE_ATTACK_ING_STATE, true);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : KnightGState
    {
        public DieState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.isStopped = true;
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
            Managers.Sound.Play("Monster/KnightG/KnightGDie_SND", Define.Sound.Effect);
            _controller.GetComponentInChildren<ParticleSystem>().Stop();
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
    public class GroggyState : KnightGState
    {
        ParticleSystem ps;
        float groggyTime;
        public GroggyState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            ps = Managers.Effect.Play(Define.Effect.Groggy, _controller.transform);
            ps.transform.SetParent(_controller.transform);
            ps.transform.localScale = new Vector3(0, 3.0f, 0);

            Debug.Log(_controller.PrevState);
            if (_controller.PrevState is CounterEnableState)
            {
                Debug.Log("HOHOHO");
                groggyTime = 3.0f;
            }
            else
            {
                groggyTime = 1.0f;
            }

            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);
        }

        public override void Execute()
        {
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
    public class GlobalState : KnightGState
    {
        public GlobalState(KnightGController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;
            if (_controller.CurState == _controller.GROGGY_STATE) return;               // 그로기 상태는 특정 상태에서 분기

            if (counterTimeTrigger <= 0)
            {
                counterTime += Time.deltaTime;
            }            

            

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= 0)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            else if (!_controller.IsEnterPhaseTwo && _stat.Hp <= (_stat.MaxHp * 0.3))
            {
                if ( (_controller.CurState == _controller.IDLE_STATE) || (_controller.CurState == _controller.IDLE_BATTLE_STATE) || (_controller.CurState == _controller.CHASE_STATE))
                {
                    _controller.ChangeState(_controller.PHASE_TRANSITION_STATE);
                }
            }

            // PHASE 2에서 슈퍼 아머 상태를 관리
            if ( _controller.IsEnterPhaseTwo )
            {

            }
        }

        public override void Exit()
        {
        }
    }
    #endregion
}
