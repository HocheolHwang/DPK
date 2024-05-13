using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EWhalePattern
{
    FirstAuto       = 0,
    CounterEnable,
    CounterAttack,
    MAX_LEN,
}

// KnightG 상태
public class WhaleState : State
{
    protected static int counterTimeTrigger = 1;        // 처음 플레이어를 직면한 후에 counterTime을 계산하기 시작한다.
    protected static float counterTime = 0;             // 카운터 공격을 할 수 있는 시간을 관리
    protected const float threadHoldCounter = 15.0f;    // 15초마다 카운터 패턴 공격

    protected WhaleController _controller;
    protected WhaleAnimationData _animData;

    public WhaleState(WhaleController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.WhaleAnimData;
    }
}
