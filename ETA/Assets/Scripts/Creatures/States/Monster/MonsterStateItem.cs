using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterStateItem
{
    // -------------------------------------- IDLE ------------------------------------------------
    #region IDLE
    public class IdleState : MonsterState
    {
        public IdleState(MonsterController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _animator.CrossFade(_animData.IdleParamHash, 0.1f);
        }

        public override void Execute()
        {
            if ( _detector.Target != null)
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
    public class IdleBattleState : MonsterState
    {
        // CHASE -> IDLE BATTLE -> ATTACK
        public IdleBattleState(MonsterController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.velocity = Vector3.zero;
            _controller.transform.LookAt(_detector.Target);
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
    public class ChaseState : MonsterState
    {
        // 몬스터끼리 뭉쳐지지 말고 경로에 몬스터가 있으면 피해서 이동하도록 수정 - Enter에서 하는 경우 Detector가 계속 
        // 몬스터가 Target을 향해 바로 회전하도록 수정
        public ChaseState(MonsterController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _agent.speed = _monsterStat.MoveSpeed;
            _animator.CrossFade(_animData.ChaseParamHash, 0.1f);
        }

        public override void Execute()
        {
            if (_detector.Target == null)
            {
                _controller.ChangeState(_controller.IDLE_STATE);
            }
            if (_controller.IsArriveToTarget())
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
    public class AttackState : MonsterState
    {
        // 한 번 재생한 뒤에 다른 상태로 전환
        // 1. Target NULL -> IDLE
        // 2. IsArriveToTarget() false -> CHASE
        // 3. IsArriveToTarget() true -> IDLE_BATTLE
        float _attackCnt;
        float _threadHold;

        public AttackState(MonsterController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _attackCnt = 0;
            _threadHold = _animData.AttackAnim.length;

            _animator.SetFloat("AttackSpeed", 0.5f);                // 원래 시간의 1/2 동안 공격 애니메이션을 재생할 수 있도록 속도 조절
            _animator.CrossFade(_animData.AttackParamHash, 0.2f);   // Idle과 Attack 애니메이션 모션 차이 때문에 들썩이는 모습을 막을 수 없는 것 같다.
        }

        public override void Execute()
        {
            _attackCnt += Time.deltaTime;
            if (_attackCnt > _threadHold * 2.0f)                    // 애니메이션 재생 시간이 2배 늘어난다.
            {
                if (_detector.Target == null)
                {
                    _controller.ChangeState(_controller.IDLE_STATE);
                }
                
                if (_controller.IsArriveToTarget())
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

    // -------------------------------------- DIE ------------------------------------------------
    #region DIE
    public class DieState : MonsterState
    {
        public DieState(MonsterController controller) : base(controller)
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

    // -------------------------------------- GLOBAL ------------------------------------------------
    #region GLOBAL
    public class GlobalState : MonsterState
    {
        public GlobalState(MonsterController controller) : base(controller)
        {
        }

        public override void Execute()
        {
            // curState가 GLOBAL_STATE 상태가 관리하는 상태인 경우 Execute() 로직을 수행하지 않는다.
            if (_controller.CurState == _controller.DIE_STATE) return;

            // GLOBAL_STATE로 전환하는 로직
            if (_controller.IsDie)
            {
                _controller.ChangeState(_controller.DIE_STATE);
            }
        }
    }
    #endregion
}
