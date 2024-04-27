using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManState : State
{
    protected static bool _MeetPlayer;              // 플레이어와 첫 조우 여부
    protected static bool _IsRangedAttack;          // 원거리 디텍터를 활성화한 상태

    protected static bool _IsDeadAllMonster;        // 모든 몬스터가 죽었는가 -> 글로벌에서 이를 감지하여 CLAP으로 전환, 단 공격 및 패턴 상태일 때는 안 됨
    protected static int _DeadAllMonsterCnt = 1;    // 0이면 모든 몬스터가 죽었다가 다시 살아났음을 알려줌

    protected MummyManController _controller;
    protected MummyManAnimationData _animData;

    public MummyManState(MummyManController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }

    // -------------------------- CLAP FUNCTIONS -----------------------------------
    // ACTION으로 buffer와 warrior를 소환한다. -> action 아님 Resource Manager를 이용
    // buffer는 man 뒤의 n 거리 뒤에
    // warrior는 옆에 소환한다. -> 근거리 공격 패턴을 얻은 상태일 수 있기 때문

    // 모든 몬스터가 한 번씩 죽었음을 알 수 있는 함수가 필요


    // -------------------------- ATTACK FUNCTIONS -----------------------------------
    protected void ControlChangeState()
    {
        if (_detector.Target == null)
        {
            _controller.ChangeState(_controller.IDLE_STATE);
        }

        if (_detector.IsArriveToTarget())
        {
            _controller.ChangeState(_controller.IDLE_BATTLE_STATE);
        }
        else
        {
            _controller.ChangeState(_controller.CHASE_STATE);
        }
    }
}
