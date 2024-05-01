using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EKnightGPattern
{
    FirstAuto       = 0,
    SecondAuto,
    CounterEnable,
    CounterAttack,
    TwoSkillEnergy,
    TwoSkillAttack,
    PhaseTransition,
    PhaseAttack,
    MAX_LEN,
}

// KnightG 상태
public class KnightGState : State
{
    protected static int attackCnt = 0;                 // 평타를 번갈아가면서 공격할 수 있음
    protected static int twoSkillTrigger = 1;           // 1번 사용할 수 있다.
    protected static int counterTimeTrigger = 1;        // 처음 플레이어를 직면한 후에 counterTime을 계산하기 시작한다.
    protected static float counterTime = 0;             // 카운터 공격을 할 수 있는 시간을 관리
    protected const float threadHoldCounter = 10.0f;    // 45초마다 카운터 패턴 공격

    protected KnightGController _controller;
    protected KnightGAnimationData _animData;

    public KnightGState(KnightGController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.KnightGAnimData;
    }

    // ------------------------------ Pattern Functions ----------------------------
    protected void StartCast(int patternIdx)
    {
        _controller.PatternInfo.PatternList[patternIdx].Cast();
    }
}
