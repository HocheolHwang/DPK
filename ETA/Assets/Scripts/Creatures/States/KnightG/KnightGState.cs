using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운터 공격은 45초마다 발생: IDLE_BATTLE에서 전환
// GLOBAL에서 시간을 계산한다.
// COUNTER ENABLE이 끝나면 counterTime을 초기화 -> 그로기 상태로 빠질 수 있기 때문

// KnightG 상태
public class KnightGState : State
{
    protected float _animTime;                          // 각 상태의 animation 수행 시간
    protected float _threadHold;                        // 각 상태의 animation 끝나는 시간
    protected static int attackCnt = 0;                 // 평타를 번갈아가면서 공격할 수 있음
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
    public bool IsStayForSeconds()
    {
        if (ExecuteTime < 0.5f) return false;
        return true;
    }

    public void LookAtEnemy()
    {
        Vector3 dir = _detector.Target.position;
        dir.y = _controller.transform.position.y;
        _controller.transform.LookAt(dir);
    }

    // ----------------------------- Global Functions -------------------------------------
    
}
