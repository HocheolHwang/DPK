using MummyManStateItem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                _controller.MeetPlayer = true;
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
            else if (_controller.BuffTime >= _controller.ThreadHoldBuff)
            {
                //Debug.Log("IDLE_BATTLE TO BUFF");
                _controller.ChangeState(_controller.BUFF_STATE);
            }
            else if (_controller.PatternOneCnt == _controller.ThreadHoldPatternOne)
            {
                //Debug.Log("IDLE_BATTLE TO PATTERN_ONE_ENABLE");
                _controller.ChangeState(_controller.PATTERN_ONE_ENABLE_STATE);
            }
            else if (_controller.CounterTime >= _controller.ThreadHoldCounter)
            {
                //Debug.Log("IDLE_BATTLE TO COUNTER_ENABLE");
                _controller.ChangeState(_controller.COUNTER_ENABLE_STATE);
            }
            else if (_controller.PatternTwoTime >= _controller.ThreadHoldPatternTwo)
            {
                //Debug.Log("IDLE_BATTLE TO PATTERN_TWO");
                _controller.ChangeState(_controller.PATTERN_TWO_STATE);
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

    // -------------------------------------- BUFF ------------------------------------------------
    #region BUFF
    // 자신한테만 누적되는 버프 수치( ATK += 2, DEF += 1, Shield: HP 15% 부여, COOL_TIME: 20초 )
    public class BuffState : IprisState
    {
        public BuffState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.BuffAnim.length);
            _animator.CrossFade(_animData.BuffParamHash, 0.1f);

            StartCast((int)EIprisPattern.BUFF);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                //Debug.Log("BUFF TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _controller.BuffTime = 0;
        }
    }
    #endregion

    // -------------------------------------- PATTERN_ONE_ENABLE ------------------------------------------------
    #region PATTERN_ONE_ENABLE
    public class PatternOneEnableState : IprisState
    {
        public PatternOneEnableState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.PatternOneCnt++;

            InitTime(_animData.PatternOneEnableAnim.length);
            _animator.SetFloat("PatternOneEnableSpeed", 0.5f);
            _animator.CrossFade(_animData.PatternOneEnableParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_controller.IsHitCounter)
            {
                _controller.IsCounterTrigger = true;
            }

            if (_animTime >= (_threadHold * 2.0f))
            {
                //Debug.Log("PATTERN_ONE_ENABLE TO PATTERN_ONE");
                _controller.ChangeState(_controller.PATTERN_ONE_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- PATTERN_ONE ------------------------------------------------
    #region PATTERN_ONE
    public class PatternOneState : IprisState
    {
        public PatternOneState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.PatternOneAnim.length);
            _animator.SetFloat("PatternOneSpeed", 0.5f);
            _animator.CrossFade(_animData.PatternOneParamHash, 0.1f);

            if (_controller.IsCounterTrigger)
            {
                // 더 강한 공격
            }
            else
            {
                // 보통 PATTER ONE
            }
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= (_threadHold * 2.0f))
            {
                //Debug.Log("PATTERN_ONE TO IDLE_BATTLE");
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
    public class CounterEnableState : IprisState
    {
        public CounterEnableState(IprisController controller) : base(controller)
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
    public class CounterAttackState : IprisState
    {
        public CounterAttackState(IprisController controller) : base(controller)
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

    // -------------------------------------- PATTERN_TWO ------------------------------------------------
    #region PATTERN_TWO
    public class PatternTwoState : IprisState
    {
        public PatternTwoState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.WindMillCnt++;

            InitTime(_animData.PatternTwoAnim.length);
            _animator.SetFloat("PatternTwoSpeed", 0.5f);
            _animator.CrossFade(_animData.PatternTwoParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= (_threadHold * 2.0f) && _controller.WindMillCnt >= _controller.ThreadHoldWindMill)
            {
                //Debug.Log("PATTERN_TWO TO WIND_MILL");
                _controller.ChangeState(_controller.PATTERN_TWO_WINDMILL_STATE);
            }
            else if (_animTime >= (_threadHold * 2.0f))
            {
                //Debug.Log("PATTERN_TWO TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _controller.PatternTwoTime = 0;
        }
    }
    #endregion

    // -------------------------------------- PATTERN_TWO_WINDMILL ------------------------------------------------
    #region PATTERN_TWO_WINDMILL
    public class PatternTwoWindMillState : IprisState
    {
        public PatternTwoWindMillState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.WindMillCnt = 0;

            InitTime(_animData.PatternTwoWindMillAnim.length);
            _animator.SetFloat("WindMillSpeed", 0.5f);
            _animator.CrossFade(_animData.PatternTwoWindMillParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= (_threadHold * 2.0f))
            {
                //Debug.Log("PATTERN_TWO_WINDMILL TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
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

    // -------------------------------------- GLOBAL_DIE ------------------------------------------------
    #region DIE
    public class DieState : IprisState
    {
        public DieState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.DieAnim.length);
            _animator.SetFloat("DieSpeed", 0.5f);
            _animator.CrossFade(_animData.DieParamHash, 0.1f);
        }

        public override void Execute()
        {
            _stat.Hp = (int)(_stat.MaxHp * 0.1f);

            if (IsStayForSeconds(_threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.TO_DRAGON_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- TO_DRAGON ------------------------------------------------
    #region TO_DRAGON
    public class ToDragonState : IprisState
    {
        public ToDragonState(IprisController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.ToDragonAnim.length);
            _animator.SetFloat("ToDragonSpeed", 0.5f);
            _animator.CrossFade(_animData.ToDragonParamHash, 0.1f);

            // 산화하는 애니메이션 재생
        }

        public override void Execute()
        {
            _stat.Hp = (int)(_stat.MaxHp * 0.1f);

            if (IsStayForSeconds(_threadHold * 2.0f))
            {
                GameObject.Destroy(_controller.gameObject);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GLOBAL_GROGGY ------------------------------------------------
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


            if (_controller.PrevState is CounterEnableState)      // Counter Enable
            {
                groggyTime = 1.7f;
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
                _controller.BuffTime += Time.deltaTime;
                _controller.PatternTwoTime += Time.deltaTime;
            }

            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;            
            if (_controller.CurState == _controller.TO_DRAGON_STATE) return;            

            // GLOBAL_STATE로 전환하는 로직
            if (_stat.Hp <= (_stat.MaxHp * 0.1f))
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
            else if (_controller.PatternOneCnt == 0 && _stat.Hp <= (_stat.MaxHp * 0.5f))
            {
                _controller.PatternOneCnt = 1;
            }
        }
    }
    #endregion
}
