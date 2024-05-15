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
            _controller.MeetPlayer = true;
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
            else if ( !_controller.IsBreath && _controller.Stat.Hp <= (_controller.Stat.MaxHp * 0.7f))
            {
                //Debug.Log("IDLE_BATTLE TO BREATH_ENABLE_STATE");
                _controller.ChangeState(_controller.BREATH_ENABLE_STATE);
            }
            else if ( !_controller.IsFireball && _controller.Stat.Hp <= (_controller.Stat.MaxHp * 0.3f))
            {
                //Debug.Log("IDLE_BATTLE TO CRY_TO_FIRE_STATE");
                _controller.ChangeState(_controller.CRY_TO_FIRE_STATE);
            }
            else if (_controller.CryDownTime >= _controller.ThreadHoldCryTime)
            {
                //Debug.Log("IDLE_BATTLE TO CRY_TO_DOWN_STATE");
                _controller.ChangeState(_controller.CRY_TO_DOWN_STATE);
            }
            else if (_controller.FearTime >= _controller.ThreadHoldFearTime)
            {
                //Debug.Log("IDLE_BATTLE TO FEAR_ENABLE");
                _controller.ChangeState(_controller.FEAR_ENABLE_STATE);
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

    // -------------------------------------- BREATH_ENABLE ------------------------------------------------
    #region BREATH_ENABLE( DEF+1000 && HitCount )
    public class BreathEnableState : DragonState
    {
        public BreathEnableState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.IsBreath = true;

            InitTime(_animData.BreathEnableAnim.length);
            _animator.SetFloat("BreathEnableSpeed", 0.33f);
            _animator.CrossFade(_animData.BreathEnableParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_controller.HitAttackCnt >= _controller.ThreadHoldHitAttackCnt)
            {
                //Debug.Log("BREATH_ENABLE TO GROGGY");
                _controller.ChangeState(_controller.GROGGY_STATE);
            }
            else if (_animTime >= _threadHold * 3.0f)
            {
                //Debug.Log("BREATH_ENABLE TO BREATH");
                _controller.ChangeState(_controller.BREATH_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- BREATH ------------------------------------------------
    #region BREATH
    public class BreathState : DragonState
    {
        public BreathState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.BreathAnim.length);
            _animator.SetFloat("BreathSpeed", 0.5f);
            _animator.CrossFade(_animData.BreathParamHash, 0.1f);

            _controller.HitAttackCnt = 0;
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold * 2.0f)
            {
                //Debug.Log("BREATH TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
            _controller.HitAttackCnt = 0;
        }
    }
    #endregion

    // -------------------------------------- CRY_TO_FIRE ------------------------------------------------
    #region CRY_TO_FIRE( Two And One Counter )
    public class CryToFireState : DragonState
    {
        public CryToFireState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.IsFireball = true;

            InitTime(_animData.CryToFireAnim.length);
            //_animator.SetFloat("BreathEnableSpeed", 0.33f);
            _animator.CrossFade(_animData.CryToFireParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;
            if (_controller.HitCounterCnt >= _controller.ThreadHoldCryFireball)
            {
                //Debug.Log("BREATH_ENABLE TO CRY");
                _controller.ChangeState(_controller.CRY_STATE);
            }
            else if (_animTime >= _threadHold)
            {
                //Debug.Log("BREATH_ENABLE TO GROUND_TO_SKY");
                _controller.ChangeState(_controller.GROUND_TO_SKY_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- FLY_FIRE_BALL ------------------------------------------------
    #region FLY_FIRE_BALL
    public class FlyFireballState : DragonState
    {
        public FlyFireballState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            //InitTime(_animData.FlyFireBallAnim.length);
            _animator.CrossFade(_animData.FlyFireBallParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (IsStayForSeconds(1.0f))
            {
                //Debug.Log("FLY_FIRE_BALL TO SKY_DOWN_ATTACK_STATE");
                _controller.ChangeState(_controller.SKY_DOWN_ATTACK_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- CRY_TO_DOWN ------------------------------------------------
    #region CRY_TO_DOWN( Two Counter )
    public class CryToDownState : DragonState
    {
        public CryToDownState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.CryToDownAnim.length);
            _animator.SetFloat("CryToDownSpeed", 0.75f);
            _animator.CrossFade(_animData.CryToDownParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            // 카운터에 맞았는지 확인하는 코드가 필요함
            // 여기서 변수를 수정하면 동기화가 어렵다
            //      1. controller에서 dragon 생성한 counter 정보( class )를 가진다.
            //      2. counter class에서 생성한 hit box가 counter를 맞으면 controller에게 정보를 전달
            //      3. controller에서 bool 값으로 관리
            //      4. 두 값이 true가 되면 상태 변경

            if (_controller.HitCounterCnt >= _controller.ThreadHoldCryDown)
            {
                //Debug.Log("CRY_TO_DOWN_STATE TO CRY_STATE");
                _controller.ChangeState(_controller.CRY_STATE);
            }
            // 카운터에 실패하면 공격
            else if (_animTime >= _threadHold * 1.34f)
            {
                //Debug.Log("CRY_TO_DOWN_STATE TO GROUND_TO_SKY_STATE");
                _controller.ChangeState(_controller.GROUND_TO_SKY_STATE);
            }
        }

        public override void Exit()
        {
            _controller.IsCryToDown = true;
            _controller.CryDownTime = 0;
        }
    }
    #endregion

    // -------------------------------------- SKY_DOWN_ATTACK ------------------------------------------------
    #region SKY_DOWN_ATTACK
    public class SkyDownAttackState : DragonState
    {
        public SkyDownAttackState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.SkyDownAttackAnim.length);
            _animator.SetFloat("SkyDownAttackSpeed", 0.5f);
            _animator.CrossFade(_animData.SkyDownAttackParamHash, 0.1f);

            // 내려찍는곳에 넉백 히트박스 생성
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_animTime >= _threadHold * 2.0f)
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
            // 내려온 뒤에 방어력을 원래 값으로 되돌림
            _controller.DecreaseDefense(_controller.AmountDEF);
        }
    }
    #endregion

    // -------------------------------------- FEAR_ENABLE ------------------------------------------------
    #region FEAR_ENABLE( Red Counter )
    public class FearEnableState : DragonState
    {
        public FearEnableState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            // 0.15초 동안 유지되기 떄문에 카운터를 치지 않았음에도 true 조건을 만족하는 경우를 배제
            _controller.IsHitCounter = false;

            InitTime(_animData.FearEnableAnim.length);
            _animator.SetFloat("FearEnableSpeed", 0.33f);
            _animator.CrossFade(_animData.FearEnableParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            // 카운터에 맞으면 기절 부여
            if (_controller.IsHitCounter)
            {
                //Debug.Log("PATTERN_ONE_ENABLE TO FEAR_STRONG_ATTACK");
                _controller.ChangeState(_controller.FEAR_STRONG_ATTACK_STATE);
            }
            // 카운터에 맞지 않아도 공격
            else if (_animTime >= _threadHold * 3.0f)
            {
                //Debug.Log("PATTERN_ONE_ENABLE TO FEAR_ATTACK");
                _controller.ChangeState(_controller.FEAR_ATTACK_STATE);
            }
        }

        public override void Exit()
        {
            _controller.FearTime = 0;
        }
    }
    #endregion

    // -------------------------------------- FEAR_ATTACK ------------------------------------------------
    #region FEAR_ATTACK
    public class FearStrongAttackState : DragonState
    {
        public FearStrongAttackState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.FearAttackAnim.length);
            _animator.SetFloat("FearSpeed", 0.5f);
            _animator.CrossFade(_animData.FearAttackParamHash, 0.1f);

            
        }

        public override void Execute()
        {
            //if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                //Debug.Log("FEAR_ATTACK TO IDLE_BATTLE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- FEAR_STRONG_ATTACK ------------------------------------------------
    #region FEAR_STRONG_ATTACK
    public class FearAttackState : DragonState
    {
        public FearAttackState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            InitTime(_animData.FearAttackAnim.length);
            _animator.SetFloat("FearSpeed", 0.5f);
            _animator.CrossFade(_animData.FearAttackParamHash, 0.1f);


        }

        public override void Execute()
        {
            //if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                //Debug.Log("FEAR_STRONG_ATTACK TO IDLE_BATTLE");
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
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }

        public override void Exit()
        {
        }
    }
    #endregion

    // -------------------------------------- GROUND_TO_SKY ------------------------------------------------
    #region GROUND_TO_SKY
    public class GroundToSkyState : DragonState
    {
        public GroundToSkyState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;

            // 날개되면 DEF가 1000증가
            _controller.IncreaseDefense(_controller.AmountDEF);

            InitTime(_animData.GroundToSkyAnim.length);
            _animator.SetFloat("GroundToSkySpeed", 0.5f);
            _animator.CrossFade(_animData.GroundToSkyParamHash, 0.1f);
        }

        public override void Execute()
        {
            _animTime += Time.deltaTime;

            if (_controller.IsCryToDown && (_animTime >= _threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.SKY_DOWN_ATTACK_STATE);
            }
            else if (_controller.IsFireball && (_animTime >= _threadHold * 2.0f))
            {
                _controller.ChangeState(_controller.FLY_FIRE_BALL_STATE);
            }
        }

        public override void Exit()
        {
            _controller.IsCryToDown = false;
        }
    }
    #endregion

    // -------------------------------------- CRY ------------------------------------------------
    #region CRY
    public class CryState : DragonState
    {
        public CryState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            InitTime(_animData.CryAnim.length);
            _animator.CrossFade(_animData.CryParamHash, 0.1f);
        }

        public override void Execute()
        {
            //if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                //Debug.Log("CRY TO IDLE_BATTLE_STATE");
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _controller.HitCounterCnt = 0;
            _controller.IsCryToDown = false;
        }
    }
    #endregion

    // -------------------------------------- GROGGY ------------------------------------------------
    #region GROGGY
    public class GroggyState : DragonState
    {
        ParticleSystem ps;
        public GroggyState(DragonController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            InitTime(_animData.GroggyAnim.length);

            // Breath에 실패하면 Groggy 상태
            if (_controller.PrevState is BreathEnableState)
            {
                // Hit Effect
                //ps = Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 1, _controller.transform);
                //ps.transform.SetParent(_controller.transform);
                //ps.transform.localPosition = new Vector3(0, 1.0f, 0);

                ps = Managers.Effect.Play(Define.Effect.Groggy, _animData.GroggyAnim.length, _controller.transform);
                ps.transform.SetParent(_controller.transform);
                ps.transform.localPosition = new Vector3(0, 3.0f, 3.5f);
            }

            _animator.CrossFade(_animData.GroggyParamHash, 0.1f);

        }

        public override void Execute()
        {
            //if (PhotonNetwork.IsMasterClient == false) return;
            _animTime += Time.deltaTime;
            if (_animTime >= _threadHold)
            {
                _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
            }
        }
        public override void Exit()
        {
            _controller.HitAttackCnt = 0;
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
            if (_controller.MeetPlayer)
            {
                _controller.FearTime += Time.deltaTime;
                _controller.CryDownTime += Time.deltaTime;
            }

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
