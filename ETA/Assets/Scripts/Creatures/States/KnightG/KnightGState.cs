using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KnightG 상태
public class KnightGState : State
{
    protected float _animTime;                          // 각 상태의 animation 수행 시간
    protected float _threadHold;                        // 각 상태의 animation 끝나는 시간
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

    // ----------------------------- Common Functions -------------------------------------
    public bool IsStayForSeconds(float seconds = 0.5f)
    {
        if (ExecuteTime < seconds) return false;
        return true;
    }

    public void InitTime(float animLength)
    {
        _animTime = 0;
        _threadHold = animLength;
    }

    public void LookAtEnemy()
    {
        if (_detector.Target == null) return;
        Vector3 dir = _detector.Target.position;
        dir.y = _controller.transform.position.y;
        _controller.transform.LookAt(dir);
    }

    // ----------------------------- Global Functions -------------------------------------
    
}
