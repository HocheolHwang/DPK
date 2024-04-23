using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class BoarState : State
{
    protected BoarController _controller;
    protected BoarAnimationData _animData;
    protected NormalMonsterStat _monsterStat;

    private float _seconds = 0.5f;

    public BoarState(BoarController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.animData;
        _monsterStat = controller.monsterStat;
    }

    // _seconds 만큼 기다림
    public bool IsStayForSeconds()
    {
        // Debug.Log($"{StartTime} | {ExecuteTime}");
        if (ExecuteTime < _seconds) return false;
        return true;
    }
}
