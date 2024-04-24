using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KnightG 상태
public class KnightGState : State
{
    protected static int attackCnt = 0;
    protected static float counterTime = 0;
    protected const float threadHoldCounter = 45.0f;    // 45초마다 카운터 패턴 공격

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
